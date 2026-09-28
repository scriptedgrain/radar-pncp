namespace RadarPncp.Api.Pncp;

/// <summary>
/// Representa um envelope da paginação do retorno do PNCP
/// </summary>
public sealed record PaginaPncpDto<T>
{
    /// <summary>
    /// Registros da página atual
    /// </summary>
    public IReadOnlyList<T> Data { get; init; } = [];

    /// <summary>
    /// Total de registros de uma consulta no PNCP
    /// </summary>
    public int TotalRegistros { get; init; }

    /// <summary>
    /// Total de páginas da consulta
    /// </summary>
    public int TotalPaginas { get; init; }

    /// <summary>
    /// Número da página atual
    /// </summary>
    public int NumeroPagina { get; init; }

    /// <summary>
    /// Quantas páginas faltam em relação à atual
    /// </summary>
    public int PaginasRestantes { get; init; }

    /// <summary>
    /// Indica se a página veio vazia
    /// </summary>
    public bool Empty { get; init; }
}