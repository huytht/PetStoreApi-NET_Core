using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreApi.Migrations
{
    public partial class UpdateDateAppUserProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "AppUserProduct",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "dateadd(hour, 7, getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComputedColumnSql: "dateadd(hour, 7, getutcdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "AppUserProduct",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "dateadd(hour, 7, getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "dateadd(hour, 7, getutcdate())");
        }
    }
}
