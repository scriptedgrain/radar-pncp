namespace Pncp;

using RadarPncp.Api.Common;
using RadarPncp.Api.Data.Entities;
using RadarPncp.Api.Pncp;

/// <summary>
/// Mapper da compra do pncp para a entidade do radar
/// </summary>
public static class ContratacaoMapper
{
    /// <summary>
    /// Método que faz o mapeamento pncp-radar para compras
    /// </summary>
    /// <param name="dto">Compra do PNCP</param>
    /// <returns>Retorna a entidade mapeada</returns>
    public static Result<Procurement> ToProcurement(ContratacaoDto dto)
    {
        //Sem número de controle, ignora
        if (string.IsNullOrWhiteSpace(dto.NumeroControlePNCP))
            return Result<Procurement>.Failure($"Número de controle vazio");

        //Sem sequencial, ignora
        if (dto.SequencialCompra == 0)
            return Result<Procurement>.Failure($"Número sequencial vazio");

        //Sem número, ignora
        if (string.IsNullOrWhiteSpace(dto.NumeroCompra))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: número compra vazio");

        //Sem processo, ignora
        if (string.IsNullOrWhiteSpace(dto.Processo))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: número processo vazio");

        //Sem objeto, ignora
        if (string.IsNullOrWhiteSpace(dto.ObjetoCompra))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: objeto vazio");

        //Sem modalidade, ignora
        if (string.IsNullOrWhiteSpace(dto.ModalidadeNome))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: modalidade vazia");

        //Sem situação, ignora
        if (string.IsNullOrWhiteSpace(dto.SituacaoCompraNome))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: situação vazia");

        //Sem órgão, ignora
        if (dto.OrgaoEntidade is null ||
            string.IsNullOrWhiteSpace(dto.OrgaoEntidade?.Cnpj) ||
            string.IsNullOrWhiteSpace(dto.OrgaoEntidade?.RazaoSocial))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: sem dados do órgão entidade");

        //Sem unidade, ignora
        if (dto.UnidadeOrgao is null ||
            string.IsNullOrWhiteSpace(dto.UnidadeOrgao?.UfSigla) ||
            string.IsNullOrWhiteSpace(dto.UnidadeOrgao?.CodigoUnidade) ||
            string.IsNullOrWhiteSpace(dto.UnidadeOrgao?.NomeUnidade))
            return Result<Procurement>.Failure($"{dto.NumeroControlePNCP}: sem dados da unidade");

        return Result<Procurement>.Success(new Procurement
        {
            PncpControlNumber = dto.NumeroControlePNCP,
            Sequential = dto.SequencialCompra,
            GovernmentUnit = new()
            {
                GovernmentEntity = new()
                {
                    TaxId = dto.OrgaoEntidade.Cnpj,
                    Name = dto.OrgaoEntidade.RazaoSocial,
                },
                PncpUnitCode = dto.UnidadeOrgao.CodigoUnidade,
                Name = dto.UnidadeOrgao.NomeUnidade,
                StateCode = dto.UnidadeOrgao.UfSigla
            },
            Year = dto.AnoCompra,
            Number = dto.NumeroCompra,
            ProcessNumber = dto.Processo,
            Object = dto.ObjetoCompra,
            ModalityId = dto.ModalidadeId,
            ModalityName = dto.ModalidadeNome,
            StatusId = dto.SituacaoCompraId,
            StatusName = dto.SituacaoCompraNome,
            EstimatedTotal = dto.ValorTotalEstimado,
            HomologatedTotal = dto.ValorTotalHomologado,
            IsPriceRegistration = dto.Srp,
            HasParliamentaryAmendment = dto.EmendaParlamentar ?? false,
            PublishDate = dto.DataPublicacaoPncp,
            UpdateDate = dto.DataAtualizacaoGlobal,
            ProposalOpeningDate = dto.DataAberturaProposta,
            ProposalClosureDate = dto.DataEncerramentoProposta,
            Legislation = dto.AmparoLegal?.Nome,
            SolicitationId = dto.TipoInstrumentoConvocatorioCodigo,
            SolicitationName = dto.TipoInstrumentoConvocatorioNome,
            Publisher = dto.UsuarioNome,
            IngestedAt = DateTime.Now
        });
    }
}