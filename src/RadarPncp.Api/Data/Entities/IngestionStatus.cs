namespace RadarPncp.Api.Data.Entities;

/// <summary>
/// Resultado de uma execução da ingestão
/// </summary>
public enum IngestionStatus
{
    /// <summary>
    /// Execução concluída sem erro
    /// </summary>
    Success,

    /// <summary>
    /// Execução interrompida por erro
    /// </summary>
    Failure
}