namespace RadarPncp.Api.Pncp;

using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;

/// <summary>
/// Cliente tipado da API de consultas do PNCP
/// </summary>
public sealed class PncpClient(HttpClient httpClient)
{
    /// <summary>
    /// Máximo de registros por página aceito pela API
    /// </summary>
    private const int TamanhoPagina = 50;

    /// <summary>
    /// Lista as contratações publicadas no período e modalidade informados, percorrendo todas as páginas
    /// </summary>
    public async IAsyncEnumerable<ContratacaoDto> ListPublishedProcurementsAsync(
        DateOnly initialDate,
        DateOnly finalDate,
        int modalityCode,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        //Filtros de data
        string inicio = initialDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string fim = finalDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

        //Começa pela primeira página
        int pagina = 1;
        //Provisório: o valor real vem da primeira resposta
        int totalPaginas = 1;

        //Enquanto houver páginas...
        while (pagina <= totalPaginas)
        {
            //A URL de consulta pública
            string url = $"v1/contratacoes/publicacao?dataInicial={inicio}&dataFinal={fim}" +
                         $"&codigoModalidadeContratacao={modalityCode}&pagina={pagina}&tamanhoPagina={TamanhoPagina}";

            using HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);

            //204 vem com content-type json e corpo vazio: sem resultados ou página além do fim
            if (response.StatusCode == HttpStatusCode.NoContent) yield break;

            response.EnsureSuccessStatusCode();

            PaginaPncpDto<ContratacaoDto> result =
                await response.Content.ReadFromJsonAsync<PaginaPncpDto<ContratacaoDto>>(cancellationToken)
                ?? throw new InvalidOperationException("Resposta do PNCP sem conteúdo.");

            //Só a primeira resposta define até onde ir
            if (pagina == 1) totalPaginas = result.TotalPaginas;

            foreach (ContratacaoDto contratacao in result.Data)
                yield return contratacao;

            pagina++;
        }
    }
}