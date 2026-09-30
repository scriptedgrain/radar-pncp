namespace RadarPncp.Api.Pncp;

using global::Pncp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RadarPncp.Api.Common;
using RadarPncp.Api.Data;
using RadarPncp.Api.Data.Entities;

/// <summary>
/// Contadores de uma execução da ingestão
/// </summary>
public sealed record IngestionResult(int Lidos, int Inseridos, int Atualizados, int Ignorados, int Descartados);

/// <summary>
/// Lê as contratações do PNCP e grava no banco do radar (upsert página a página)
/// </summary>
public sealed class ProcurementIngestionService(
    PncpClient pncp,
    RadarDbContext db,
    IOptions<PncpOptions> options,
    ILogger<ProcurementIngestionService> logger)
{
    /// <summary>
    /// Ingere as contratações publicadas na data e modalidade informadas
    /// </summary>
    public async Task<IngestionResult> IngestAsync(DateOnly data, int modalidade, CancellationToken cancellationToken = default)
    {
        int lidos = 0, inseridos = 0, atualizados = 0, ignorados = 0, descartados = 0;

        //Mesmo instante de ingestão para toda a execução
        DateTime ingestedAt = DateTime.UtcNow;

        //Cache da execução: guarda só Ids, porque as instâncias são desanexadas a cada página
        Dictionary<string, long> entityIds = [];
        Dictionary<(string TaxId, string Code), long> unitIds = [];

        int page = 1;
        int totalPages = 1;

        while (page <= totalPages)
        {
            //Pausa entre páginas (nunca antes da primeira)
            if (page > 1) await Task.Delay(options.Value.DelayEntrePaginas, cancellationToken);

            PaginaPncpDto<ContratacaoDto> result = await pncp.GetPageAsync(data, data, modalidade, page, cancellationToken);

            //Página vazia: sem resultados ou página além do fim
            if (result.Data.Count == 0) break;

            //Só a primeira resposta define até onde ir
            if (page == 1) totalPages = result.TotalPaginas;

            lidos += result.Data.Count;

            //Mapeia a página; descartados vão para o log e não interrompem a página
            List<Procurement> mapped = [];
            foreach (ContratacaoDto dto in result.Data)
            {
                Result<Procurement> mapping = ContratacaoMapper.ToProcurement(dto, ingestedAt);

                if (mapping.IsSuccess)
                {
                    mapped.Add(mapping.Value);
                    continue;
                }

                logger.LogWarning("Página {Pagina}: contratação descartada. Motivo: {Motivo}", page, mapping.Error);
                descartados++;
            }

            //Uma consulta para as compras já existentes da página
            List<string> numbers = [.. mapped.Select(p => p.PncpControlNumber).Distinct()];
            Dictionary<string, Procurement> existing = await db.Procurements
                                                             .Where(p => numbers.Contains(p.PncpControlNumber))
                                                             .ToDictionaryAsync(p => p.PncpControlNumber, cancellationToken);

            //Consulta o banco só para órgãos e unidades ainda não vistos
            await LoadUnseenAsync(mapped, entityIds, unitIds, cancellationToken);

            //Órgãos e unidades novos da página: a mesma instância é reaproveitada dentro da página
            Dictionary<string, GovernmentEntity> pageEntities = [];
            Dictionary<(string TaxId, string Code), GovernmentUnit> pageUnits = [];

            HashSet<string> processed = [];
            foreach (Procurement procurement in mapped)
            {
                //Número de controle repetido na página: vale a primeira ocorrência
                if (!processed.Add(procurement.PncpControlNumber))
                {
                    ignorados++;
                    continue;
                }

                if (!existing.TryGetValue(procurement.PncpControlNumber, out Procurement? current))
                {
                    //Não existe: insere
                    ResolveUnit(procurement, procurement.GovernmentUnit, entityIds, unitIds, pageEntities, pageUnits);
                    db.Procurements.Add(procurement);
                    inseridos++;
                }
                else if (procurement.UpdateDate > current.UpdateDate)
                {
                    //Existe e o PNCP tem versão mais recente: atualiza
                    CopyTo(procurement, current);
                    ResolveUnit(current, procurement.GovernmentUnit, entityIds, unitIds, pageEntities, pageUnits);
                    atualizados++;
                }
                else
                {
                    //Existe e não mudou: ignora
                    ignorados++;
                }
            }

            await db.SaveChangesAsync(cancellationToken);

            //Depois de gravar, os novos já têm Id: entram no cache da execução
            foreach ((string taxId, GovernmentEntity entity) in pageEntities) entityIds[taxId] = entity.Id;
            foreach (((string TaxId, string Code) key, GovernmentUnit unit) in pageUnits) unitIds[key] = unit.Id;

            //Libera as entidades rastreadas da página
            db.ChangeTracker.Clear();

            page++;
        }

        return new IngestionResult(lidos, inseridos, atualizados, ignorados, descartados);
    }

    /// <summary>
    /// Carrega do banco, para o cache, os órgãos e unidades da página que ainda não foram vistos
    /// </summary>
    private async Task LoadUnseenAsync(
        List<Procurement> mapped,
        Dictionary<string, long> entityIds,
        Dictionary<(string TaxId, string Code), long> unitIds,
        CancellationToken cancellationToken)
    {
        //Órgãos não vistos
        List<string> unseenTaxIds = [.. mapped
            .Select(p => p.GovernmentUnit.GovernmentEntity.TaxId)
            .Distinct()
            .Where(taxId => !entityIds.ContainsKey(taxId))];

        if (unseenTaxIds.Count > 0)
        {
            var entities = await db.GovernmentEntities
                .AsNoTracking()
                .Where(e => unseenTaxIds.Contains(e.TaxId))
                .Select(e => new { e.Id, e.TaxId })
                .ToListAsync(cancellationToken);

            foreach (var entity in entities) entityIds[entity.TaxId] = entity.Id;
        }

        //Unidades não vistas, só de órgãos que já existem no banco
        List<long> unseenUnitEntityIds = mapped
            .Select(p => (p.GovernmentUnit.GovernmentEntity.TaxId, p.GovernmentUnit.PncpUnitCode))
            .Distinct()
            .Where(key => !unitIds.ContainsKey(key) && entityIds.ContainsKey(key.TaxId))
            .Select(key => entityIds[key.TaxId])
            .Distinct()
            .ToList();

        if (unseenUnitEntityIds.Count > 0)
        {
            var units = await db.GovernmentUnits
                .AsNoTracking()
                .Where(u => unseenUnitEntityIds.Contains(u.GovernmentEntityId))
                .Select(u => new { u.Id, u.PncpUnitCode, u.GovernmentEntity.TaxId })
                .ToListAsync(cancellationToken);

            foreach (var unit in units) unitIds[(unit.TaxId, unit.PncpUnitCode)] = unit.Id;
        }
    }

    /// <summary>
    /// Liga a compra à unidade: pelo Id se já existe no banco, ou por uma instância nova compartilhada na página
    /// </summary>
    private static void ResolveUnit(
        Procurement target,
        GovernmentUnit mappedUnit,
        Dictionary<string, long> entityIds,
        Dictionary<(string TaxId, string Code), long> unitIds,
        Dictionary<string, GovernmentEntity> pageEntities,
        Dictionary<(string TaxId, string Code), GovernmentUnit> pageUnits)
    {
        string taxId = mappedUnit.GovernmentEntity.TaxId;
        (string, string) key = (taxId, mappedUnit.PncpUnitCode);

        //Unidade já existe no banco: liga só pela FK
        if (unitIds.TryGetValue(key, out long unitId))
        {
            target.GovernmentUnit = null!;
            target.GovernmentUnitId = unitId;
            return;
        }

        //Unidade nova já criada nesta página: reaproveita a instância
        if (pageUnits.TryGetValue(key, out GovernmentUnit? pageUnit))
        {
            target.GovernmentUnit = pageUnit;
            return;
        }

        //Unidade nova: resolve o órgão da mesma forma
        if (entityIds.TryGetValue(taxId, out long entityId))
        {
            mappedUnit.GovernmentEntity = null!;
            mappedUnit.GovernmentEntityId = entityId;
        }
        else if (pageEntities.TryGetValue(taxId, out GovernmentEntity? pageEntity))
        {
            mappedUnit.GovernmentEntity = pageEntity;
        }
        else
        {
            pageEntities[taxId] = mappedUnit.GovernmentEntity;
        }

        pageUnits[key] = mappedUnit;
        target.GovernmentUnit = mappedUnit;
    }

    /// <summary>
    /// Copia os campos de negócio da compra mapeada para a compra gravada (Id, número de controle e IngestedAt ficam)
    /// </summary>
    private static void CopyTo(Procurement source, Procurement target)
    {
        target.Sequential = source.Sequential;
        target.Year = source.Year;
        target.Number = source.Number;
        target.ProcessNumber = source.ProcessNumber;
        target.Object = source.Object;
        target.ModalityId = source.ModalityId;
        target.ModalityName = source.ModalityName;
        target.StatusId = source.StatusId;
        target.StatusName = source.StatusName;
        target.EstimatedTotal = source.EstimatedTotal;
        target.HomologatedTotal = source.HomologatedTotal;
        target.IsPriceRegistration = source.IsPriceRegistration;
        target.HasParliamentaryAmendment = source.HasParliamentaryAmendment;
        target.PublishDate = source.PublishDate;
        target.UpdateDate = source.UpdateDate;
        target.ProposalOpeningDate = source.ProposalOpeningDate;
        target.ProposalClosureDate = source.ProposalClosureDate;
        target.Legislation = source.Legislation;
        target.SolicitationId = source.SolicitationId;
        target.SolicitationName = source.SolicitationName;
        target.Publisher = source.Publisher;
    }
}
