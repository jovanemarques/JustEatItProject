using Microsoft.EntityFrameworkCore.Migrations;

namespace JustEatIt.Migrations
{
    public partial class AddPartnerToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartnerId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PartnerId",
                table: "Orders",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Partners_PartnerId",
                table: "Orders",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Partners_PartnerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PartnerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Orders");
        }
    }
}
