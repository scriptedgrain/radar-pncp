namespace RadarPncp.Api.Dev;

using System.Diagnostics;
using RadarPncp.Api.Pncp;

/// <summary>
/// Endpoint que dispara a ingestão de contratações do PNCP para uma data e modalidade
/// </summary>
public static class DevIngestEndpoint
{
    /// <summary>
    /// Mapeia POST /dev/ingest?data=aaaa-mm-dd&amp;modalidade=n
    /// </summary>
    public static void MapDevIngest(this WebApplication app)
    {
        app.MapPost("/dev/ingest",
        async (DateOnly data, int modalidade, ProcurementIngestionService service, CancellationToken cancellationToken) =>
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                IngestionResult result = await service.IngestAsync(data, modalidade, cancellationToken);

                stopwatch.Stop();

                return Results.Ok(new
                {
                    lidos = result.Lidos,
                    inseridos = result.Inseridos,
                    atualizados = result.Atualizados,
                    ignorados = result.Ignorados,
                    descartados = result.Descartados,
                    segundos = Math.Round(stopwatch.Elapsed.TotalSeconds, 2)
                });

            }
            catch (PncpException ex)
            {
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status502BadGateway);
            }
        });
    }
}