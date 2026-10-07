using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadarPncp.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLimiteCodigoUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "pncp_unit_code",
                table: "government_units",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "pncp_unit_code",
                table: "government_units",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
