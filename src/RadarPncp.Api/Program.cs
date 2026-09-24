using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data;
using RadarPncp.Api.Dev;

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

//Constroi o webapp
WebApplication app = builder.Build();

//Aidiciona as rotas
app.MapGet("/health", () => Results.Ok());

//Seed para testes e consultas
if (app.Environment.IsDevelopment())
{
    app.MapDevSeed();
    app.MapDevQueries();
}

//Tudo pronto
app.Run();