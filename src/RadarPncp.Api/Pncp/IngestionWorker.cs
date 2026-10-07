namespace RadarPncp.Api.Pncp;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RadarPncp.Api.Data;
using RadarPncp.Api.Data.Entities;

/// <summary>
/// Executa a ingestão periodicamente, garantindo sucesso para cada dia e modalidade configurados
/// </summary>
public sealed class IngestionWorker(
    IServiceScopeFactory scopeFactory,
    IOptions<IngestaoOptions> options,
    IOptions<PncpOptions> pncpOptions,
    ILogger<IngestionWorker> logger) : BackgroundService
{
    /// <summary>
    /// Fuso do PNCP: define o que é "ontem"
    /// </summary>
    private static readonly TimeZoneInfo Brasilia = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    /// <summary>
    /// Laço principal: um ciclo ao subir e depois um a cada intervalo
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IngestaoOptions opts = options.Value;

        if (!opts.Habilitada)
        {
            logger.LogInformation("Ingestão automática desabilitada.");
            return;
        }

        using PeriodicTimer timer = new(opts.Intervalo);

        //do/while: o primeiro ciclo roda sem esperar o primeiro tick
        do
        {
            try
            {
                await RunCycleAsync(opts, stoppingToken);
            }
            catch (Exception ex) when (!IsStopping(ex, stoppingToken))
            {
                //Falha fora de uma combinação (ex.: banco fora do ar): aborta só este ciclo
                logger.LogError(ex, "Ciclo de ingestão abortado.");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    /// <summary>
    /// Percorre os dias retroativos e as modalidades, uma combinação de cada vez
    /// </summary>
    private async Task RunCycleAsync(IngestaoOptions opts, CancellationToken stoppingToken)
    {
        DateOnly yesterday = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Brasilia)).AddDays(-1);
        DateOnly first = yesterday.AddDays(-(opts.DiasRetroativos - 1));

        logger.LogInformation("Ciclo de ingestão: {Inicio:dd/MM/yyyy} a {Fim:dd/MM/yyyy}.", first, yesterday);

        //Só pausa se a combinação anterior chamou o PNCP
        bool calledPncp = false;

        for (DateOnly date = first; date <= yesterday; date = date.AddDays(1))
        {
            foreach (int modality in opts.Modalidades)
            {
                if (calledPncp) await Task.Delay(pncpOptions.Value.DelayEntrePaginas, stoppingToken);

                calledPncp = await ProcessAsync(date, modality, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Ingere uma combinação de data e modalidade, se ainda não houver sucesso registrado. Devolve true se chamou o PNCP
    /// </summary>
    private async Task<bool> ProcessAsync(DateOnly date, int modality, CancellationToken stoppingToken)
    {
        //Escopo próprio: DbContext limpo para cada combinação
        using IServiceScope scope = scopeFactory.CreateScope();
        RadarDbContext db = scope.ServiceProvider.GetRequiredService<RadarDbContext>();
        ProcurementIngestionService ingestion = scope.ServiceProvider.GetRequiredService<ProcurementIngestionService>();

        bool alreadyDone = await db.IngestionRuns.AnyAsync(
            r => r.ReferenceDate == date && r.ModalityId == modality && r.Status == IngestionStatus.Success,
            stoppingToken);

        if (alreadyDone)
        {
            logger.LogDebug("{Data:dd/MM/yyyy}, modalidade {Modalidade}: já ingerida.", date, modality);
            return false;
        }

        IngestionRun run = new()
        {
            ReferenceDate = date,
            ModalityId = modality,
            StartedAt = DateTime.UtcNow
        };

        try
        {
            IngestionResult result = await ingestion.IngestAsync(date, modality, stoppingToken);

            run.Status = IngestionStatus.Success;
            run.Read = result.Lidos;
            run.Inserted = result.Inseridos;
            run.Updated = result.Atualizados;
            run.Ignored = result.Ignorados;
            run.Discarded = result.Descartados;

            logger.LogInformation(
                "{Data:dd/MM/yyyy}, modalidade {Modalidade}: {Lidos} lidos, {Inseridos} inseridos, {Atualizados} atualizados, {Ignorados} ignorados, {Descartados} descartados.",
                date, modality, result.Lidos, result.Inseridos, result.Atualizados, result.Ignorados, result.Descartados);
        }
        catch (Exception ex) when (!IsStopping(ex, stoppingToken))
        {
            //Contadores ficam zerados: o serviço não devolve resultado parcial
            run.Status = IngestionStatus.Failure;
            run.Error = ex.Message;

            logger.LogWarning(ex, "{Data:dd/MM/yyyy}, modalidade {Modalidade}: falha na ingestão.", date, modality);
        }

        //Descarta o que a ingestão deixou rastreado sem salvar, para gravar só o registro da execução
        db.ChangeTracker.Clear();

        run.FinishedAt = DateTime.UtcNow;
        db.IngestionRuns.Add(run);
        await db.SaveChangesAsync(stoppingToken);

        return true;
    }

    /// <summary>
    /// Indica se a exceção é o cancelamento do desligamento da aplicação
    /// </summary>
    private static bool IsStopping(Exception ex, CancellationToken stoppingToken) =>
        ex is OperationCanceledException && stoppingToken.IsCancellationRequested;
}