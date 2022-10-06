using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreApi.Migrations
{
    public partial class UpdateTypeDateAppUserProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "AppUserProduct",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "dateadd(hour, 7, getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComputedColumnSql: "getutcdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "AppUserProduct",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComputedColumnSql: "dateadd(hour, 7, getutcdate())");
        }
    }
}
