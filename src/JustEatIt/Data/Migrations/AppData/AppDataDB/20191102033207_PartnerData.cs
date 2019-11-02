using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JustEatIt.Data.Migrations.AppData.AppDataDB
{
    public partial class PartnerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    BestBefore = table.Column<DateTime>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Restaurant = table.Column<string>(nullable: true),
<<<<<<< HEAD
                    PartnerId = table.Column<string>(nullable: true),
=======
                    PartnerId = table.Column<string>(nullable: true)
>>>>>>> jovane_r1_i2
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dish_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dish_PartnerId",
                table: "Dish",
                column: "PartnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dish");

            migrationBuilder.DropTable(
                name: "Partner");
        }
    }
}
