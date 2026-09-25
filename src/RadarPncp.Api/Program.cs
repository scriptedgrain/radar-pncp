using Microsoft.EntityFrameworkCore;
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

//Cliente tipado da API de consultas do PNCP com resiliência padrão
builder.Services.AddHttpClient<PncpClient>(client =>
{
    //Barra final é necessária para compor com os caminhos relativos
    client.BaseAddress = new Uri("https://pncp.gov.br/api/consulta/");
}).AddStandardResilienceHandler();

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