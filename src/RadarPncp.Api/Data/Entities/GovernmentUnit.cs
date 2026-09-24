namespace RadarPncp.Api.Data.Entities;

/// <summary>
/// Unidade compradora 
/// </summary>
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
    /// Propriedade de navegação do órgão a que pertence
    /// </summary>
    public GovernmentEntity GovernmentEntity { get; set; } = null!;

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
    public required string StateCode { get; set; }
}