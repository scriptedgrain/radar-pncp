namespace RadarPncp.Api.Consultas;

using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Endpoints públicos de consulta de contratações
/// </summary>
public static class ContratacaoEndpoints
{
    /// <summary>
    /// Mapeia o grupo /api/contratacoes
    /// </summary>
    public static void MapContratacoes(this WebApplication app)
    {
        RouteGroupBuilder grupo = app.MapGroup("/api/contratacoes")
                                     .WithTags("Contratações");

        grupo.MapGet("/", ConsultarAsync)
             .WithName("ConsultarContratacoes")
             .WithSummary("Consulta contratações")
             .WithDescription("Lista as contratações do Radar, da publicação mais recente para a mais antiga. " +
                              "Todos os filtros são opcionais: Texto busca no objeto da contratação (mínimo 3 caracteres), " +
                              "Uf filtra pela sigla do estado da unidade (2 letras), Modalidade pelo código da modalidade, " +
                              "e DataInicial/DataFinal pelo intervalo de publicação (dias inteiros, horário de Brasília). " +
                              "A paginação usa Pagina (a partir de 1) e TamanhoPagina (de 1 a 100, padrão 20).");
    }

    /// <summary>
    /// Valida o filtro e executa a consulta
    /// </summary>
    private static async Task<Results<Ok<PaginaResponse<ContratacaoResumoResponse>>, ValidationProblem>> ConsultarAsync(
        [AsParameters] ContratacaoFiltro filtro, ContratacaoQuery query, CancellationToken cancellationToken)
    {
        //Filtro inválido: 400 com uma mensagem por campo
        Dictionary<string, string[]> erros = Validar(filtro);
        if (erros.Count > 0) return TypedResults.ValidationProblem(erros);

        return TypedResults.Ok(await query.ConsultarAsync(filtro, cancellationToken));
    }

    /// <summary>
    /// Regras de validação do filtro (campos não informados não são validados)
    /// </summary>
    private static Dictionary<string, string[]> Validar(ContratacaoFiltro filtro)
    {
        Dictionary<string, string[]> erros = [];

        if (filtro.Pagina < 1)
            erros[nameof(filtro.Pagina)] = ["Pagina deve ser maior ou igual a 1."];
        if (filtro.TamanhoPagina is < 1 or > 100)
            erros[nameof(filtro.TamanhoPagina)] = ["TamanhoPagina deve estar entre 1 e 100."];
        if (filtro.Uf is not null)
        {
            string uf = filtro.Uf.Trim();
            if (uf.Length != 2 || !uf.All(char.IsAsciiLetter))
                erros[nameof(filtro.Uf)] = ["Uf deve ter exatamente 2 letras."];
        }
        if (filtro.Texto is not null && filtro.Texto.Trim().Length < 3)
            erros[nameof(filtro.Texto)] = ["Texto deve ter pelo menos 3 caracteres."];
        if (filtro.DataInicial is DateOnly dataInicial && filtro.DataFinal is DateOnly dataFinal && dataFinal < dataInicial)
            erros[nameof(filtro.DataFinal)] = ["DataFinal não pode ser anterior a DataInicial."];

        return erros;
    }
}
