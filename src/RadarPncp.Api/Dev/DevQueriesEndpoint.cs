namespace RadarPncp.Api.Dev;

using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data;

/// <summary>
/// Endpoint para estudar consultas EF Core: devolve o SQL gerado e o resultado
/// </summary>
public static class DevQueriesEndpoint
{
    /// <summary>
    /// Mapeia GET /dev/consultas/{n}
    /// </summary>
    public static void MapDevQueries(this WebApplication app)
    {
        app.MapGet("/dev/consultas/{n:int}", async (int n, RadarDbContext db) =>
        {
            IQueryable<object> query;

            //Troque o return de cada caso por: query = ...; break;
            switch (n)
            {
                case 1:
                    query = from procurement in db.Procurements
                            where procurement.GovernmentUnit.StateCode == "RS"
                            select procurement;
                    break;
                case 2:
                    query = from procurement in db.Procurements
                            where EF.Functions.ILike(procurement.Object, "%Pavimentação%")
                            select procurement;
                    break;
                case 3:
                    query = from procurement in db.Procurements
                            select new
                            {
                                procurement.Object,
                                procurement.EstimatedTotal,
                                procurement.GovernmentUnit.StateCode
                            };
                    break;
                case 4:
                    query = db.Procurements.OrderByDescending(p => p.PublishDate)
                                           .Skip(2)
                                           .Take(2);
                    break;
                case 5:
                    query = from procurement in db.Procurements
                            group procurement by procurement.GovernmentUnit.StateCode
                            into procurementsByState
                            select new
                            {
                                Uf = procurementsByState.Key,
                                Quantidade = procurementsByState.Count(),
                                Total = procurementsByState.Sum(p => p.EstimatedTotal)
                            };
                    break;
                default:
                    return Results.NotFound();
            }

            return Results.Ok(new { sql = query.ToQueryString(), dados = await query.ToListAsync() });
        });
    }
}