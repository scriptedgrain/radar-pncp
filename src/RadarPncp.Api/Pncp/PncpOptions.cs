namespace RadarPncp.Api.Pncp;

/// <summary>
/// Configurações do cliente do PNCP, lidas da seção "Pncp"
/// </summary>
public sealed class PncpOptions
{
    /// <summary>
    /// Nome da seção no appsettings
    /// </summary>
    public const string Secao = "Pncp";

    /// <summary>
    /// Tempo de espera entre uma página e outra
    /// </summary>
    public TimeSpan DelayEntrePaginas { get; set; } = TimeSpan.FromSeconds(3);
}