using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RadarDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Radar") ??
    throw new InvalidOperationException("Connection string 'Radar' não configurada.")));

WebApplication app = builder.Build();

app.MapGet("/health", () => Results.Ok());

app.Run();
