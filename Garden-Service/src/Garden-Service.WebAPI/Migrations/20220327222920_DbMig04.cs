using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    public partial class DbMig04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysToMature",
                table: "PlantData");

            migrationBuilder.RenameColumn(
                name: "RipePercentage",
                table: "Plants",
                newName: "DaysToMature");

            migrationBuilder.AddColumn<double>(
                name: "GrowthPercentage",
                table: "Plants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrowthPercentage",
                table: "Plants");

            migrationBuilder.RenameColumn(
                name: "DaysToMature",
                table: "Plants",
                newName: "RipePercentage");

            migrationBuilder.AddColumn<int>(
                name: "DaysToMature",
                table: "PlantData",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
