namespace RadarPncp.Api.Data.Entities;

public class GovernmentEntity
{
    /// <summary>
    /// Chave primária do objeto
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// CNPJ do órgão
    /// </summary>
    public required string TaxId { get; set; }

    /// <summary>
    /// Nome do órgão
    /// </summary>
    public required string Name { get; set; }
}