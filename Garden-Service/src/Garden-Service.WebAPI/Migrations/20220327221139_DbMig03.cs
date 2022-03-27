using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    public partial class DbMig03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActFruitCount",
                table: "Plants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RipePercentage",
                table: "Plants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeFromPlanting",
                table: "Plants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Yield",
                table: "Plants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActFruitCount",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "RipePercentage",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "TimeFromPlanting",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "Yield",
                table: "Plants");
        }
    }
}
