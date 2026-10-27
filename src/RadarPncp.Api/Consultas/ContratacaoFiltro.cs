namespace RadarPncp.Api.Consultas;

/// <summary>
/// Filtros da consulta de contratações, lidos da query string
/// </summary>
public sealed record ContratacaoFiltro(
    string? Texto,
    string? Uf,
    int? Modalidade,
    DateOnly? DataInicial,
    DateOnly? DataFinal,
    int Pagina = 1,
    int TamanhoPagina = 20);