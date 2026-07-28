using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.MySql.Migrations.Data
{
    /// <inheritdoc />
    public partial class PlatformSettingNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ObjectType_ObjectId",
                table: "PlatformSetting");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectType_ObjectId_Name",
                table: "PlatformSetting",
                columns: new[] { "ObjectType", "ObjectId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ObjectType_ObjectId_Name",
                table: "PlatformSetting");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectType_ObjectId",
                table: "PlatformSetting",
                columns: new[] { "ObjectType", "ObjectId" });
        }
    }
}
