namespace RadarPncp.Api.Data.Entities;

public class GovernmentUnit
{
    /// <summary>
    /// Chave primária
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Órgão a que pertence
    /// </summary>
    public long GovernmentEntityId { get; set; }

    /// <summary>
    /// Abreviação da unidade
    /// </summary>
    public required string AbbreviationName { get; set; }

    /// <summary>
    /// Código no PNCP da unidade
    /// </summary>
    public required string PncpUnitCode { get; set; }

    /// <summary>
    /// Nome da unidade compradora
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Sigla UF da unidade
    /// </summary>
    public required string StateCode { get; set; } = null!;
}
