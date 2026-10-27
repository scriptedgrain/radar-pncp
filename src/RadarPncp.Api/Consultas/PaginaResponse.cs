namespace RadarPncp.Api.Consultas;

/// <summary>
/// Envelope de paginação das respostas do Radar
/// </summary>
public sealed record PaginaResponse<T>(
    IReadOnlyList<T> Itens,
    int Pagina,
    int TamanhoPagina,
    int TotalItens,
    int TotalPaginas);