namespace RadarPncp.Api.Data.Entities;

/// <summary>
/// Representa uma compra no PNCP
/// </summary>
public class Procurement
{
    /// <summary>
    /// ID da entidade
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Código da compra no PNCP (CNPJ - Sequencial - Ano)
    /// </summary>
    public required string PncpControlNumber { get; set; }

    /// <summary>
    /// Sequencial da compra no PNCP
    /// </summary>
    public required int Sequential { get; set; }

    /// <summary>
    /// ID da unidade da compra
    /// </summary>
    public long GovernmentUnitId { get; set; }

    /// <summary>
    /// Propriedade de navegação da unidade
    /// </summary>
    public GovernmentUnit GovernmentUnit { get; set; } = null!;

    /// <summary>
    /// Ano em que a compra foi realizada
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Número da compra
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Número do processo
    /// </summary>
    public required string ProcessNumber { get; set; }

    /// <summary>
    /// Objeto da compra
    /// </summary>
    public required string Object { get; set; }

    /// <summary>
    /// ID da modalidade
    /// </summary>
    public int ModalityId { get; set; }

    /// <summary>
    /// Nome da modalidade
    /// </summary>
    public required string ModalityName { get; set; }

    /// <summary>
    /// Código da situação da compra
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Nome da situação da compra
    /// </summary>
    public required string StatusName { get; set; }

    /// <summary>
    /// Total estimado para a compra
    /// </summary>
    public decimal? EstimatedTotal { get; set; }

    /// <summary>
    /// Total homologado da compra
    /// </summary>
    public decimal? HomologatedTotal { get; set; }

    /// <summary>
    /// É registro de preço? (SRP)
    /// </summary>
    public bool IsPriceRegistration { get; set; }

    /// <summary>
    /// Tem emenda parlamentar?
    /// </summary>
    public bool HasParliamentaryAmendment { get; set; }

    /// <summary>
    /// Data de publicação no PNCP
    /// </summary>
    public DateTime PublishDate { get; set; }

    /// <summary>
    /// Última atualização no PNCP
    /// </summary>
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// Data de abertura das propostas
    /// </summary>
    public DateTime? ProposalOpeningDate { get; set; }

    /// <summary>
    /// Data de fechamento das propostas
    /// </summary>
    public DateTime? ProposalClosureDate { get; set; }

    /// <summary>
    /// Legislação aplicada na compra
    /// </summary>
    public string? Legislation { get; set; }

    /// <summary>
    /// Modo de disputa da compra
    /// </summary>
    public string? DisputeType { get; set; }

    /// <summary>
    /// Código do tipo de instrumento convocatório
    /// </summary>
    public int SolicitationId { get; set; }

    /// <summary>
    /// Tipo de instrumento convocatório
    /// </summary>
    public string? SolicitationName { get; set; }

    /// <summary>
    /// Sistema que fez a publicação da compra no PNCP
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// Data de consumo da compra no radar
    /// </summary>
    public DateTime IngestedAt { get; set; }
}