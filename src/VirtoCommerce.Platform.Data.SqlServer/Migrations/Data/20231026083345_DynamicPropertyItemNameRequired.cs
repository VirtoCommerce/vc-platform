using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Data
{
    public partial class DynamicPropertyItemNameRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE PlatformDynamicPropertyDictionaryItem SET Name = Id WHERE Name IS NULL or LEN(Name) = 0");

            migrationBuilder.DropIndex(
                name: "IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name",
                table: "PlatformDynamicPropertyDictionaryItem");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlatformDynamicPropertyDictionaryItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name",
                table: "PlatformDynamicPropertyDictionaryItem",
                columns: new[] { "PropertyId", "Name" },
                unique: true,
                filter: "[PropertyId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name",
                table: "PlatformDynamicPropertyDictionaryItem");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlatformDynamicPropertyDictionaryItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name",
                table: "PlatformDynamicPropertyDictionaryItem",
                columns: new[] { "PropertyId", "Name" },
                unique: true,
                filter: "[PropertyId] IS NOT NULL AND [Name] IS NOT NULL");
        }
    }
}
