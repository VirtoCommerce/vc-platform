using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.MySql.Migrations.Data
{
    /// <inheritdoc />
    public partial class MakeSettingIdNotNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM PlatformSettingValue WHERE SettingId IS NULL;");

            migrationBuilder.UpdateData(
                table: "PlatformSettingValue",
                keyColumn: "SettingId",
                keyValue: null,
                column: "SettingId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "PlatformSettingValue",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "PlatformSettingValue",
                type: "varchar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
