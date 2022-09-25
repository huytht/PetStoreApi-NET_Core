using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreApi.Migrations
{
    public partial class UpdateOrderItemSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "OrderItem",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderItem",
                newName: "Amount");
        }
    }
}
