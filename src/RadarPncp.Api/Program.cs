using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data;
using RadarPncp.Api.Dev;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RadarDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Radar") ?? throw new InvalidOperationException("Connection string 'Radar' não configurada."))
           .UseSnakeCaseNamingConvention());

WebApplication app = builder.Build();

app.MapGet("/health", () => Results.Ok());

if (app.Environment.IsDevelopment())
    app.MapDevSeed();

app.Run();