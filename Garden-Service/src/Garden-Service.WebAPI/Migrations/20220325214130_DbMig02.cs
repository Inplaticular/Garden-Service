using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    public partial class DbMig02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CoordinateLatitude",
                table: "Gardens",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CoordinateLongitude",
                table: "Gardens",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinateLatitude",
                table: "Gardens");

            migrationBuilder.DropColumn(
                name: "CoordinateLongitude",
                table: "Gardens");
        }
    }
}
