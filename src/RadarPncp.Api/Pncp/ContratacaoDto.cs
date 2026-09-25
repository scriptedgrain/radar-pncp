namespace RadarPncp.Api.Pncp;

/// <summary>
/// Representa uma compra no PNCP
/// </summary>
public sealed record ContratacaoDto
{
    /// <summary>
    /// Código da compra no PNCP (CNPJ - Sequencial - Ano)
    /// </summary>
    public required string NumeroControlePNCP { get; init; }

    /// <summary>
    /// Sequencial da compra no PNCP
    /// </summary>
    public int SequencialCompra { get; init; }

    /// <summary>
    /// Órgão dono da compra
    /// </summary>
    public OrgaoEntidadeDto? OrgaoEntidade { get; init; }

    /// <summary>
    /// Unidade compradora
    /// </summary>
    public UnidadeOrgaoDto? UnidadeOrgao { get; init; }

    /// <summary>
    /// Ano em que a compra foi realizada
    /// </summary>
    public int AnoCompra { get; init; }

    /// <summary>
    /// Número da compra
    /// </summary>
    public string? NumeroCompra { get; init; }

    /// <summary>
    /// Número do processo
    /// </summary>
    public string? Processo { get; init; }

    /// <summary>
    /// Objeto da compra
    /// </summary>
    public string? ObjetoCompra { get; init; }

    /// <summary>
    /// ID da modalidade
    /// </summary>
    public int ModalidadeId { get; init; }

    /// <summary>
    /// Nome da modalidade
    /// </summary>
    public string? ModalidadeNome { get; init; }

    /// <summary>
    /// Código da situação da compra
    /// </summary>
    public int SituacaoCompraId { get; init; }

    /// <summary>
    /// Nome da situação da compra
    /// </summary>
    public string? SituacaoCompraNome { get; init; }

    /// <summary>
    /// Total estimado para a compra
    /// </summary>
    public decimal? ValorTotalEstimado { get; init; }

    /// <summary>
    /// Total homologado da compra
    /// </summary>
    public decimal? ValorTotalHomologado { get; init; }

    /// <summary>
    /// É registro de preço? (SRP)
    /// </summary>
    public bool Srp { get; init; }

    /// <summary>
    /// Tem emenda parlamentar? (vem null na maioria das compras)
    /// </summary>
    public bool? EmendaParlamentar { get; init; }

    /// <summary>
    /// Data de publicação no PNCP
    /// </summary>
    public DateTime DataPublicacaoPncp { get; init; }

    /// <summary>
    /// Última atualização no PNCP (considera também itens e documentos)
    /// </summary>
    public DateTime DataAtualizacaoGlobal { get; init; }

    /// <summary>
    /// Data de abertura das propostas
    /// </summary>
    public DateTime? DataAberturaProposta { get; init; }

    /// <summary>
    /// Data de fechamento das propostas
    /// </summary>
    public DateTime? DataEncerramentoProposta { get; init; }

    /// <summary>
    /// Legislação aplicada na compra
    /// </summary>
    public AmparoLegalDto? AmparoLegal { get; init; }

    /// <summary>
    /// Modo de disputa da compra
    /// </summary>
    public string? ModoDisputaNome { get; init; }

    /// <summary>
    /// Código do tipo de instrumento convocatório
    /// </summary>
    public int TipoInstrumentoConvocatorioCodigo { get; init; }

    /// <summary>
    /// Tipo de instrumento convocatório
    /// </summary>
    public string? TipoInstrumentoConvocatorioNome { get; init; }

    /// <summary>
    /// Sistema que fez a publicação da compra no PNCP
    /// </summary>
    public string? UsuarioNome { get; init; }
}

/// <summary>
/// Órgão dono da compra no PNCP
/// </summary>
public sealed record OrgaoEntidadeDto
{
    /// <summary>
    /// CNPJ do órgão
    /// </summary>
    public string? Cnpj { get; init; }

    /// <summary>
    /// Nome do órgão
    /// </summary>
    public string? RazaoSocial { get; init; }
}

/// <summary>
/// Unidade compradora no PNCP
/// </summary>
public sealed record UnidadeOrgaoDto
{
    /// <summary>
    /// Código no PNCP da unidade
    /// </summary>
    public string? CodigoUnidade { get; init; }

    /// <summary>
    /// Nome da unidade compradora
    /// </summary>
    public string? NomeUnidade { get; init; }

    /// <summary>
    /// Sigla UF da unidade
    /// </summary>
    public string? UfSigla { get; init; }
}

/// <summary>
/// Amparo legal da compra no PNCP
/// </summary>
public sealed record AmparoLegalDto
{
    /// <summary>
    /// Nome da legislação (ex.: "Lei 14.133/2021, Art. 28, I")
    /// </summary>
    public string? Nome { get; init; }
}