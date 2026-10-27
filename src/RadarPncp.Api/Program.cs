using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Consultas;
using RadarPncp.Api.Data;
using RadarPncp.Api.Dev;
using RadarPncp.Api.Pncp;
using Scalar.AspNetCore;

//Construtor do app
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Cria o contexto com o banco e aplica as configurações
builder.Services.AddDbContext<RadarDbContext>(options =>
{
    //Contexto com o banco PostgreSQL com convenção por snake_case
    options.UseNpgsql(builder.Configuration.GetConnectionString("Radar") ?? throw new InvalidOperationException("Connection string 'Radar' não configurada."))
           .UseSnakeCaseNamingConvention();

    //Só exibe os dados no log se for ambiente de desenvolvimento
    if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
});

//Configurações do cliente do PNCP (seção "Pncp")
builder.Services.AddOptions<PncpOptions>().BindConfiguration(PncpOptions.Secao);

//Configurações da ingestão no Radar (seção "Ingestao")
builder.Services.AddOptions<IngestaoOptions>()
                .BindConfiguration(IngestaoOptions.Secao)
                .Validate(o => o.Intervalo > TimeSpan.Zero, "Ingestao:Intervalo deve ser maior que zero.")
                .Validate(o => o.DiasRetroativos > 0, "Ingestao:DiasRetroativos deve ser maior que zero.")
                .ValidateOnStart();

//Cliente tipado da API de consultas do PNCP com resiliência
builder.Services.AddHttpClient<PncpClient>(client =>
{
    //Barra final é necessária para compor com os caminhos relativos
    client.BaseAddress = new Uri("https://pncp.gov.br/api/consulta/");

    //Agente que está fazendo as requisições
    client.DefaultRequestHeaders.UserAgent.ParseAdd("RadarPncp/1.0 (+https://github.com/scriptedgrain/radar-pncp)");
});

//Serviço de ingestão e consulta: scoped porque depende do DbContext
builder.Services.AddScoped<ProcurementIngestionService>();
builder.Services.AddScoped<ContratacaoQuery>();
builder.Services.AddProblemDetails();

//Worker da ingestão automática (só age se Ingestao:Habilitada = true)
builder.Services.AddHostedService<IngestionWorker>();

//Geração do documento OpenAPI nativo do .NET
builder.Services.AddOpenApi();

//Constroi o webapp
WebApplication app = builder.Build();

//Adiciona as rotas
app.MapGet("/health", () => Results.Ok());
app.MapContratacoes();

//Documentação em todos os ambientes: JSON em /openapi/v1.json e interface do Scalar em /scalar
app.MapOpenApi();
app.MapScalarApiReference();

//Seed para testes, consultas e chamada ao PNCP
if (app.Environment.IsDevelopment())
{
    app.MapDevSeed();
    app.MapDevQueries();
    app.MapDevPncp();
    app.MapDevIngest();
}

//Tudo pronto
app.Run();