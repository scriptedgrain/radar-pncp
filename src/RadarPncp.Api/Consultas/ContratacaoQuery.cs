namespace RadarPncp.Api.Consultas;

using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data;
using RadarPncp.Api.Data.Entities;

/// <summary>
/// Resumo de uma contratação devolvido pela consulta
/// </summary>
public sealed class ContratacaoQuery(RadarDbContext db)
{
    /// <summary>
    /// Tamanho máximo de página
    /// </summary>
    private const int TamanhoPaginaMaximo = 100;

    /// <summary>
    /// Retorna os dados conforme o resumo 
    /// </summary>
    public async Task<PaginaResponse<ContratacaoResumoResponse>> ConsultarAsync(
    ContratacaoFiltro filtro, CancellationToken cancellationToken)
    {
        //Página e tamanho dentro dos limites
        int pagina = Math.Max(1, filtro.Pagina);
        int tamanho = Math.Clamp(filtro.TamanhoPagina, 1, TamanhoPaginaMaximo);

        //Ponto de partida (sem rastreador de mudança para não ser detectado no savechanges)
        IQueryable<Procurement> query = db.Procurements.AsNoTracking();

        //Filtros: cada um só entra se veio preenchido
        TimeZoneInfo brasilia = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        if (!string.IsNullOrWhiteSpace(filtro.Texto))
            query = query.Where(p => EF.Functions.ILike(p.Object, $"%{filtro.Texto}%"));
        if (!string.IsNullOrWhiteSpace(filtro.Uf))
            query = query.Where(p => p.GovernmentUnit.StateCode.Equals(filtro.Uf.Trim(), StringComparison.InvariantCultureIgnoreCase));
        if (filtro.Modalidade is int modalidade && modalidade > 0)
            query = query.Where(p => p.ModalityId == modalidade);
        if (filtro.DataInicial.HasValue && filtro.DataInicial is DateOnly initialDate && initialDate != DateOnly.MinValue)
        {
            //Meia-noite do dia inicial em Brasília, convertida para UTC
            DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(
            initialDate.ToDateTime(TimeOnly.MinValue), brasilia);

            query = query.Where(p => p.PublishDate >= utcStart);
        }
        if (filtro.DataFinal is DateOnly dataFinal)
        {
            //Meia-noite do dia SEGUINTE ao final, em Brasília, convertida para UTC
            DateTime fimUtc = TimeZoneInfo.ConvertTimeToUtc(
                dataFinal.AddDays(1).ToDateTime(TimeOnly.MinValue), brasilia);

            query = query.Where(p => p.PublishDate < fimUtc);
        }

        //Total, contado ANTES de paginar
        int total = await query.CountAsync(cancellationToken);

        //Ordem estável, a página, e a projeção para o DTO
        List<ContratacaoResumoResponse> itens = await query
            .OrderByDescending(p => p.PublishDate)
            .ThenByDescending(p => p.Id)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .Select(p => new ContratacaoResumoResponse(
                p.PncpControlNumber,
                p.Object,
                p.GovernmentUnit.Name,
                p.GovernmentUnit.StateCode,
                p.ModalityName,
                p.StatusName,
                p.EstimatedTotal,
                p.HomologatedTotal,
                p.PublishDate,
                p.ProposalOpeningDate,
                MontarLinkPncp(p.GovernmentUnit.GovernmentEntity.TaxId, p.Year, p.Sequential)
            ))
            .ToListAsync(cancellationToken);

        //O envelope
        int totalPaginas = (int)Math.Ceiling(total / (double)tamanho);
        return new PaginaResponse<ContratacaoResumoResponse>(itens, pagina, tamanho, total, totalPaginas);
    }

    /// <summary>
    /// Monta o link do PNCP
    /// </summary>
    private static string MontarLinkPncp(string cnpj, int ano, int sequencial)
    => $"https://pncp.gov.br/app/editais/{cnpj}/{ano}/{sequencial}";
}