using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreApi.Migrations
{
    public partial class UpdateDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Breed",
                table: "Product");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Breed",
                table: "Product",
                column: "BreedId",
                principalTable: "Breed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Breed",
                table: "Product");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Breed",
                table: "Product",
                column: "BreedId",
                principalTable: "Breed",
                principalColumn: "Id");
        }
    }
}
