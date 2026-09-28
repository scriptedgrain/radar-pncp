namespace RadarPncp.Api.Dev;

using System.Diagnostics;
using RadarPncp.Api.Pncp;

/// <summary>
/// Endpoint para validar a paginação do cliente do PNCP: compara o total informado com o total lido, sem gravar nada
/// </summary>
public static class DevPncpEndpoint
{
    /// <summary>
    /// Mapeia GET /dev/pncp?data=aaaa-mm-dd&amp;modalidade=n
    /// </summary>
    public static void MapDevPncp(this WebApplication app)
    {
        app.MapGet("/dev/pncp", async (DateOnly data, int modalidade, PncpClient pncp, CancellationToken cancellationToken) =>
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            //O total informado vem do envelope da primeira página
            PaginaPncpDto<ContratacaoDto> firstPage = await pncp.GetPageAsync(data, data, modalidade, 1, cancellationToken);

            //Percorre todas as páginas contando os itens
            int totalLido = 0;
            await foreach (ContratacaoDto _ in pncp.ListPublishedProcurementsAsync(data, data, modalidade, cancellationToken))
                totalLido++;

            stopwatch.Stop();

            return Results.Ok(new
            {
                totalInformado = firstPage.TotalRegistros,
                totalLido,
                segundos = Math.Round(stopwatch.Elapsed.TotalSeconds, 2)
            });
        });
    }
}
