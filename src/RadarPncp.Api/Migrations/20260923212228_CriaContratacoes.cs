using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RadarPncp.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriaContratacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GovernmentEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaxId = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GovernmentUnits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GovernmentEntityId = table.Column<long>(type: "bigint", nullable: false),
                    AbbreviationName = table.Column<string>(type: "text", nullable: false),
                    PncpUnitCode = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StateCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovernmentUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovernmentUnits_GovernmentEntities_GovernmentEntityId",
                        column: x => x.GovernmentEntityId,
                        principalTable: "GovernmentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Procurements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PncpControlNumber = table.Column<string>(type: "text", nullable: false),
                    GovernmentUnitId = table.Column<long>(type: "bigint", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Sequential = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    ProcessNumber = table.Column<string>(type: "text", nullable: false),
                    Object = table.Column<string>(type: "text", nullable: false),
                    ModalityId = table.Column<int>(type: "integer", nullable: false),
                    ModalityName = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    StatusName = table.Column<string>(type: "text", nullable: false),
                    EstimatedTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    HomologatedTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsPriceRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    HasParliamentaryAmendment = table.Column<bool>(type: "boolean", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProposalOpeningDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProposalClosureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Legislation = table.Column<string>(type: "text", nullable: true),
                    DisputeType = table.Column<string>(type: "text", nullable: true),
                    SolicitationName = table.Column<string>(type: "text", nullable: true),
                    Publisher = table.Column<string>(type: "text", nullable: true),
                    IngestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procurements_GovernmentUnits_GovernmentUnitId",
                        column: x => x.GovernmentUnitId,
                        principalTable: "GovernmentUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentEntities_TaxId",
                table: "GovernmentEntities",
                column: "TaxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GovernmentUnits_GovernmentEntityId_PncpUnitCode",
                table: "GovernmentUnits",
                columns: new[] { "GovernmentEntityId", "PncpUnitCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procurements_GovernmentUnitId",
                table: "Procurements",
                column: "GovernmentUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Procurements_PncpControlNumber",
                table: "Procurements",
                column: "PncpControlNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Procurements");

            migrationBuilder.DropTable(
                name: "GovernmentUnits");

            migrationBuilder.DropTable(
                name: "GovernmentEntities");
        }
    }
}
