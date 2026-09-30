namespace RadarPncp.Api.Pncp;

using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

/// <summary>
/// Cliente tipado da API de consultas do PNCP
/// </summary>
public sealed class PncpClient(HttpClient httpClient, IOptions<PncpOptions> options)
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
        //Começa pela primeira página
        int page = 1;
        //Provisório: o valor real vem da primeira resposta
        int totalPages = 1;

        //Enquanto houver páginas...
        while (page <= totalPages)
        {
            //Pausa entre páginas (nunca antes da primeira)
            if (page > 1) await Task.Delay(options.Value.DelayEntrePaginas, cancellationToken);

            PaginaPncpDto<ContratacaoDto> result =
                await GetPageAsync(initialDate, finalDate, modalityCode, page, cancellationToken);

            //Página vazia: sem resultados ou página além do fim
            if (result.Data.Count == 0) yield break;

            //Só a primeira resposta define até onde ir
            if (page == 1) totalPages = result.TotalPaginas;

            foreach (ContratacaoDto contratacao in result.Data)
                yield return contratacao;

            page++;
        }
    }

    /// <summary>
    /// Busca uma única página de contratações publicadas no período e modalidade informados
    /// </summary>
    public async Task<PaginaPncpDto<ContratacaoDto>> GetPageAsync(
        DateOnly initialDate,
        DateOnly finalDate,
        int modalityCode,
        int page,
        CancellationToken cancellationToken = default)
    {
        //Filtros de data
        string start = initialDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        string end = finalDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

        //A URL de consulta pública
        string url = $"v1/contratacoes/publicacao?dataInicial={start}&dataFinal={end}" +
                     $"&codigoModalidadeContratacao={modalityCode}&pagina={page}&tamanhoPagina={TamanhoPagina}";

        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync(url, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new PncpException(
                $"Falha de conexão com o PNCP na página {page} de {initialDate:dd/MM/yyyy}, modalidade {modalityCode}.", ex);
        }

        using (response)
        {
            //204 vem com content-type json e corpo vazio: devolve uma página vazia
            if (response.StatusCode == HttpStatusCode.NoContent) return new PaginaPncpDto<ContratacaoDto>();

            if (!response.IsSuccessStatusCode)
                throw new PncpException(
                    $"PNCP respondeu {(int)response.StatusCode} na página {page} de {initialDate:dd/MM/yyyy}, modalidade {modalityCode}.");

            return await response.Content.ReadFromJsonAsync<PaginaPncpDto<ContratacaoDto>>(cancellationToken)
                ?? throw new PncpException("Resposta do PNCP sem conteúdo.");
        }
    }
}