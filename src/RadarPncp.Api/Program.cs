using Microsoft.EntityFrameworkCore;
using Polly;
using RadarPncp.Api.Data;
using RadarPncp.Api.Dev;
using RadarPncp.Api.Pncp;

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

//Cliente tipado da API de consultas do PNCP com resiliência
builder.Services.AddHttpClient<PncpClient>(client =>
{
    //Barra final é necessária para compor com os caminhos relativos
    client.BaseAddress = new Uri("https://pncp.gov.br/api/consulta/");

    //O timeout do HttpClient envolve todas as tentativas: quem controla é o TotalRequestTimeout
    client.Timeout = Timeout.InfiniteTimeSpan;
}).AddStandardResilienceHandler(options =>
{
    //5 novas tentativas com espera exponencial: ~5s, 10s, 20s, 40s, 80s
    options.Retry.MaxRetryAttempts = 5;
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.Delay = TimeSpan.FromSeconds(5);

    //Se a API mandar Retry-After (ex.: 429), usa o tempo pedido por ela
    options.Retry.ShouldRetryAfterHeader = true;

    //Limite de cada tentativa individual
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(30);

    //Limite total: 6 tentativas x 30s + ~155s de espera, com folga
    options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(10);

    //Validação exige janela de amostragem de pelo menos 2x o timeout por tentativa
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);
});

//Constroi o webapp
WebApplication app = builder.Build();

//Adiciona as rotas
app.MapGet("/health", () => Results.Ok());

//Seed para testes, consultas e chamada ao PNCP
if (app.Environment.IsDevelopment())
{
    app.MapDevSeed();
    app.MapDevQueries();
    app.MapDevPncp();
}

//Tudo pronto
app.Run();