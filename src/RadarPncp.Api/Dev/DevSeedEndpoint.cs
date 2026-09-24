namespace RadarPncp.Api.Dev;

using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data;
using RadarPncp.Api.Data.Entities;

/// <summary>
/// Endpoint que popula o banco com contratações reais do PNCP para desenvolvimento
/// </summary>
public static class DevSeedEndpoint
{
    /// <summary>
    /// Mapeia POST /dev/seed
    /// </summary>
    public static void MapDevSeed(this WebApplication app)
    {
        app.MapPost("/dev/seed", async (RadarDbContext db) =>
        {
            int inserted = 0;

            foreach (Procurement procurement in BuildSeed())
            {
                //Compra já gravada: não grava de novo
                if (await db.Procurements.AnyAsync(p => p.PncpControlNumber == procurement.PncpControlNumber))
                    continue;

                //Reaproveita órgão e unidade que já existirem no banco
                GovernmentEntity? entity = await db.GovernmentEntities
                    .FirstOrDefaultAsync(e => e.TaxId == procurement.GovernmentUnit.GovernmentEntity.TaxId);

                if (entity is not null)
                {
                    procurement.GovernmentUnit.GovernmentEntity = entity;

                    GovernmentUnit? unit = await db.GovernmentUnits
                        .FirstOrDefaultAsync(u => u.GovernmentEntityId == entity.Id && u.PncpUnitCode == procurement.GovernmentUnit.PncpUnitCode);

                    if (unit is not null)
                        procurement.GovernmentUnit = unit;
                }

                db.Procurements.Add(procurement);
                inserted++;
            }

            await db.SaveChangesAsync();

            return Results.Ok(new { inserted });
        });
    }

    /// <summary>
    /// Converte uma data do PNCP (horário de Brasília, sem fuso) para UTC
    /// </summary>
    private static DateTime Brt(string value) =>
        DateTimeOffset.Parse($"{value}-03:00", CultureInfo.InvariantCulture).UtcDateTime;

    /// <summary>
    /// Monta as contratações do seed: 3 de SC e 3 de RS
    /// </summary>
    private static List<Procurement> BuildSeed()
    {
        DateTime now = DateTime.UtcNow;

        return
        [
            new Procurement
            {
                PncpControlNumber = "01599409000139-1-000024/2026",
                Sequential = 24,
                Year = 2026,
                Number = "PMCA 053/26",
                ProcessNumber = "PMCA 053/26",
                Object = "Contratação de pessoa jurídica para fornecimento de pedra brita, galerias e concreto usinado para melhoria na infraestrutura da malha viária do município de Capão Alto (Convênio 18756/2025).",
                ModalityId = 6,
                ModalityName = "Pregão - Eletrônico",
                StatusId = 1,
                StatusName = "Divulgada no PNCP",
                EstimatedTotal = 596960m,
                HomologatedTotal = null,
                IsPriceRegistration = true,
                HasParliamentaryAmendment = false,
                PublishDate = Brt("2026-09-15T00:00:23"),
                UpdateDate = Brt("2026-09-15T00:01:55"),
                ProposalOpeningDate = Brt("2026-09-14T13:00:01"),
                ProposalClosureDate = Brt("2026-09-28T13:30:01"),
                Legislation = "Lei 14.133/2021, Art. 28, I",
                DisputeType = "Aberto-Fechado",
                SolicitationId = 1,
                SolicitationName = "Edital",
                Publisher = "Pública Tecnologia Ltda.",
                IngestedAt = now,
                GovernmentUnit = new GovernmentUnit
                {
                    PncpUnitCode = "1",
                    Name = "PREFEITURA MUNICIPAL DE CAPÃO ALTO - SC",
                    StateCode = "SC",
                    GovernmentEntity = new GovernmentEntity { TaxId = "01599409000139", Name = "MUNICIPIO DE CAPAO ALTO" }
                }
            },
            new Procurement
            {
                PncpControlNumber = "82827148000169-1-000066/2026",
                Sequential = 66,
                Year = 2026,
                Number = "44PR2026",
                ProcessNumber = "44PR2026",
                Object = "CONTRATAÇÃO DE EMPRESA ESPECIALIZADA PARA A EXECUÇÃO DOS SERVIÇOS DE MÃO DE OBRA DESTINADOS À SUBSTITUIÇÃO DA COBERTURA DA CRECHE MUNICIPAL DE PINHEIRO PRETO/SC.",
                ModalityId = 6,
                ModalityName = "Pregão - Eletrônico",
                StatusId = 1,
                StatusName = "Divulgada no PNCP",
                EstimatedTotal = 20762.41m,
                HomologatedTotal = null,
                IsPriceRegistration = false,
                HasParliamentaryAmendment = false,
                PublishDate = Brt("2026-09-15T00:00:24"),
                UpdateDate = Brt("2026-09-15T00:00:40"),
                ProposalOpeningDate = Brt("2026-09-14T17:00:01"),
                ProposalClosureDate = Brt("2026-09-30T08:15:01"),
                Legislation = "Lei 14.133/2021, Art. 28, I",
                DisputeType = "Aberto",
                SolicitationId = 1,
                SolicitationName = "Edital",
                Publisher = "Pública Tecnologia Ltda.",
                IngestedAt = now,
                GovernmentUnit = new GovernmentUnit
                {
                    PncpUnitCode = "2",
                    Name = "Município de Pinheiro Preto",
                    StateCode = "SC",
                    GovernmentEntity = new GovernmentEntity { TaxId = "82827148000169", Name = "MUNICIPIO DE PINHEIRO PRETO" }
                }
            },
            new Procurement
            {
                PncpControlNumber = "15408168000108-1-000006/2026",
                Sequential = 6,
                Year = 2026,
                Number = "FMS 006/26",
                ProcessNumber = "FMS 006/26",
                Object = "Serviço de manutenção preventiva e corretiva, com reposição de peças, em equipamentos odontológicos, conforme tabela, condições e exigências descritas no termo de referência.",
                ModalityId = 6,
                ModalityName = "Pregão - Eletrônico",
                StatusId = 1,
                StatusName = "Divulgada no PNCP",
                EstimatedTotal = 69041m,
                HomologatedTotal = null,
                IsPriceRegistration = false,
                HasParliamentaryAmendment = false,
                PublishDate = Brt("2026-09-15T00:00:35"),
                UpdateDate = Brt("2026-09-15T00:01:12"),
                ProposalOpeningDate = Brt("2026-09-14T13:00:01"),
                ProposalClosureDate = Brt("2026-09-28T13:30:01"),
                Legislation = "Lei 14.133/2021, Art. 28, I",
                DisputeType = "Aberto-Fechado",
                SolicitationId = 1,
                SolicitationName = "Edital",
                Publisher = "Pública Tecnologia Ltda.",
                IngestedAt = now,
                GovernmentUnit = new GovernmentUnit
                {
                    PncpUnitCode = "2",
                    Name = "Fundo Municipal de Saúde de Capão Alto",
                    StateCode = "SC",
                    GovernmentEntity = new GovernmentEntity { TaxId = "15408168000108", Name = "FUNDO MUNICIPAL DE SAUDE DE CAPAO ALTO" }
                }
            },
            new Procurement
            {
                PncpControlNumber = "13183513000127-1-000169/2026",
                Sequential = 169,
                Year = 2026,
                Number = "0154",
                ProcessNumber = "327584",
                Object = "CONTRATAÇÃO DE EMPRESA ESPECIALIZADA NA PRESTAÇÃO DE SERVIÇOS PARA REALIZAÇÃO DE EXAMES DE DIAGNÓSTICOS POR IMAGEM PARA O HOSPITAL MUNICIPAL GETÚLIO VARGAS, EM SAPUCAIA DO SUL",
                ModalityId = 6,
                ModalityName = "Pregão - Eletrônico",
                StatusId = 1,
                StatusName = "Divulgada no PNCP",
                EstimatedTotal = 0m,
                HomologatedTotal = null,
                IsPriceRegistration = false,
                HasParliamentaryAmendment = false,
                PublishDate = Brt("2026-09-15T00:02:11"),
                UpdateDate = Brt("2026-09-15T00:02:20"),
                ProposalOpeningDate = Brt("2026-09-15T08:00:00"),
                ProposalClosureDate = Brt("2026-09-29T08:50:00"),
                Legislation = "Lei 14.133/2021, Art. 28, I",
                DisputeType = "Aberto",
                SolicitationId = 1,
                SolicitationName = "Edital",
                Publisher = "PROCERGS - CENTRO DE TECNOLOGIA DA INFORMACAO E COMUNICACAO DO ESTADO DO RIO GRANDE DO SUL S.A.",
                IngestedAt = now,
                GovernmentUnit = new GovernmentUnit
                {
                    PncpUnitCode = "127122",
                    Name = "FUNDAÇÃO DE SAÚDE SAPUCAIA DO SUL",
                    StateCode = "RS",
                    GovernmentEntity = new GovernmentEntity { TaxId = "13183513000127", Name = "FUNDACAO DE SAUDE SAPUCAIA DO SUL" }
                }
            },
            new Procurement
            {
                PncpControlNumber = "88124961000159-1-000106/2026",
                Sequential = 106,
                Year = 2026,
                Number = "0055",
                ProcessNumber = "4210",
                Object = "AQUISIÇÃO DE IMPLEMENTOS AGRÍCOLAS",
                ModalityId = 6,
                ModalityName = "Pregão - Eletrônico",
                StatusId = 1,
                StatusName = "Divulgada no PNCP",
                EstimatedTotal = 195375.42m,
                HomologatedTotal = null,
                IsPriceRegistration = false,
                HasParliamentaryAmendment = false,
                PublishDate = Brt("2026-09-15T00:02:32"),
                UpdateDate = Brt("2026-09-15T00:02:46"),
                ProposalOpeningDate = Brt("2026-09-15T08:00:00"),
                ProposalClosureDate = Brt("2026-09-29T09:00:00"),
                Legislation = "Lei 14.133/2021, Art. 28, I",
                DisputeType = "Aberto",
                SolicitationId = 1,
                SolicitationName = "Edital",
                Publisher = "PROCERGS - CENTRO DE TECNOLOGIA DA INFORMACAO E COMUNICACAO DO ESTADO DO RIO GRANDE DO SUL S.A.",
                IngestedAt = now,
                GovernmentUnit = new GovernmentUnit
                {
                    PncpUnitCode = "154476",
                    Name = "PREFEITURA MUNICIPAL DE SANT´ANA DOLIVRAMENTO",
                    StateCode = "RS",
                    GovernmentEntity = new GovernmentEntity { TaxId = "88124961000159", Name = "MUNICIPIO DE SANTANA DO LIVRAMENTO" }
                }
            },
            new Procurement
            {
                PncpControlNumber = "87613071000148-1-000385/2026",
                Sequential = 385,
                Year = 2026,
                Number = "0064",
                ProcessNumber = "284/2026",
                Object = "REGISTRO DE PREÇO DE MATERIAS DE EPIs PARA SECRETARIA DE MEIO AMBIENTE E DESENVOLVIMENTO URBANO.",
                ModalityId = 6,
                ModalityName = "Pregão - Eletrônico",
                StatusId = 1,
                StatusName = "Divulgada no PNCP",
                EstimatedTotal = 182286.02m,
                HomologatedTotal = null,
                IsPriceRegistration = true,
                HasParliamentaryAmendment = false,
                PublishDate = Brt("2026-09-15T00:02:58"),
                UpdateDate = Brt("2026-09-15T09:01:35"),
                ProposalOpeningDate = Brt("2026-09-15T08:00:00"),
                ProposalClosureDate = Brt("2026-09-25T08:00:00"),
                Legislation = "Lei 14.133/2021, Art. 28, I",
                DisputeType = "Aberto",
                SolicitationId = 1,
                SolicitationName = "Edital",
                Publisher = "PROCERGS - CENTRO DE TECNOLOGIA DA INFORMACAO E COMUNICACAO DO ESTADO DO RIO GRANDE DO SUL S.A.",
                IngestedAt = now,
                GovernmentUnit = new GovernmentUnit
                {
                    PncpUnitCode = "109819",
                    Name = "PREFEITURA MUNICIPAL DE SANTO ÂNGELO",
                    StateCode = "RS",
                    GovernmentEntity = new GovernmentEntity { TaxId = "87613071000148", Name = "MUNICIPIO DE SANTO ANGELO" }
                }
            }
        ];
    }
}
