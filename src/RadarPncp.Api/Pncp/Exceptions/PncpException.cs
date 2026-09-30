namespace RadarPncp.Api.Pncp;

/// <summary>
/// Falha ao consultar a API do PNCP
/// </summary>
public sealed class PncpException(string message, Exception? innerException = null)
    : Exception(message, innerException);