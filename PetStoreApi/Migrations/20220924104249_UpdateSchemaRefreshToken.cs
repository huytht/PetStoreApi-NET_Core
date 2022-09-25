using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreApi.Migrations
{
    public partial class UpdateSchemaRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpireAt",
                table: "RefreshToken",
                newName: "ExpiredAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiredAt",
                table: "RefreshToken",
                newName: "ExpireAt");
        }
    }
}
