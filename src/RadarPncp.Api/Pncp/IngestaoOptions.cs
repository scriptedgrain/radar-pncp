namespace RadarPncp.Api.Pncp;

/// <summary>
/// Configurações de ingestão do Radar
/// </summary>
public sealed class IngestaoOptions
{
    /// <summary>
    /// Nome da seção no appsettings
    /// </summary>
    public const string Secao = "Ingestao";

    /// <summary>
    /// Liga ou desliga a ingestão automática
    /// </summary>
    public bool Habilitada { get; set; }

    /// <summary>
    /// Tempo entre um ciclo de verificação e outro
    /// </summary>
    public TimeSpan Intervalo { get; set; } = TimeSpan.FromHours(12);

    /// <summary>
    /// Quantos dias para trás, a partir de ontem, devem estar ingeridos
    /// </summary>
    public int DiasRetroativos { get; set; } = 4;

    /// <summary>
    /// Códigos das modalidades do PNCP a ingerir
    /// </summary>
    public int[] Modalidades { get; set; } = [];
}