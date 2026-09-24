using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadarPncp.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAbbreviationNameDaUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "abbreviation_name",
                table: "government_units");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "abbreviation_name",
                table: "government_units",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
