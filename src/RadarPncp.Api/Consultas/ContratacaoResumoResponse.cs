namespace RadarPncp.Api.Consultas;

/// <summary>
/// Resumo de uma contratação devolvido pela consulta
/// </summary>
public sealed record ContratacaoResumoResponse(
    string NumeroControlePncp,
    string Objeto,
    string Orgao,
    string Uf,
    string Modalidade,
    string Situacao,
    decimal? ValorEstimado,
    decimal? ValorHomologado,
    DateTime DataPublicacao,
    DateTime? DataAberturaProposta,
    string LinkPncp);