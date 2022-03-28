using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    public partial class DbMig05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFromPlanting",
                table: "Plants");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlantedAtDateTime",
                table: "Plants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlantedAtDateTime",
                table: "Plants");

            migrationBuilder.AddColumn<int>(
                name: "TimeFromPlanting",
                table: "Plants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
