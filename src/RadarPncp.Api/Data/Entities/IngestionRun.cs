namespace RadarPncp.Api.Data.Entities;

/// <summary>
/// Representa uma execução da ingestão de compras do PNCP
/// </summary>
public class IngestionRun
{
    /// <summary>
    /// ID da entidade
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Data de publicação consultada no PNCP
    /// </summary>
    public DateOnly ReferenceDate { get; set; }

    /// <summary>
    /// ID da modalidade consultada
    /// </summary>
    public int ModalityId { get; set; }

    /// <summary>
    /// Situação da execução (sucesso ou falha)
    /// </summary>
    public IngestionStatus Status { get; set; }

    /// <summary>
    /// Quantidade de compras lidas do PNCP
    /// </summary>
    public int Read { get; set; }

    /// <summary>
    /// Quantidade de compras inseridas
    /// </summary>
    public int Inserted { get; set; }

    /// <summary>
    /// Quantidade de compras atualizadas
    /// </summary>
    public int Updated { get; set; }

    /// <summary>
    /// Quantidade de compras ignoradas (sem mudança ou repetidas)
    /// </summary>
    public int Ignored { get; set; }

    /// <summary>
    /// Quantidade de compras descartadas no mapeamento
    /// </summary>
    public int Discarded { get; set; }

    /// <summary>
    /// Início da execução (UTC)
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Fim da execução (UTC)
    /// </summary>
    public DateTime FinishedAt { get; set; }

    /// <summary>
    /// Mensagem da falha, quando houver
    /// </summary>
    public string? Error { get; set; }
}