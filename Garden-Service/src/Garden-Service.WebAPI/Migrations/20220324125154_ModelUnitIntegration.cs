using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    public partial class ModelUnitIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitId",
                table: "Plants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnitId",
                table: "Gardens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Gardens");
        }
    }
}
