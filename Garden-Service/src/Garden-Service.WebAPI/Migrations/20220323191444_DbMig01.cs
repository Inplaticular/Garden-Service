using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    public partial class DbMig01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gardens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gardens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantData",
                columns: table => new
                {
                    BotanicalName = table.Column<string>(type: "text", nullable: false),
                    FriendlyName = table.Column<string>(type: "text", nullable: false),
                    DaysToMature = table.Column<int>(type: "integer", nullable: false),
                    GrowthPerDay = table.Column<double>(type: "double precision", nullable: false),
                    AvgFruitWeight = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantData", x => x.BotanicalName);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    GardenId = table.Column<string>(type: "text", nullable: false),
                    PlantDataId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plants_Gardens_GardenId",
                        column: x => x.GardenId,
                        principalTable: "Gardens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plants_PlantData_PlantDataId",
                        column: x => x.PlantDataId,
                        principalTable: "PlantData",
                        principalColumn: "BotanicalName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plants_GardenId",
                table: "Plants",
                column: "GardenId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantDataId",
                table: "Plants",
                column: "PlantDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Gardens");

            migrationBuilder.DropTable(
                name: "PlantData");
        }
    }
}
