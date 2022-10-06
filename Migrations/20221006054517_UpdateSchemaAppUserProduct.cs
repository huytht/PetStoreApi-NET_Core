using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreApi.Migrations
{
    public partial class UpdateSchemaAppUserProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "AppUserProduct",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getutcdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "AppUserProduct");
        }
    }
}
