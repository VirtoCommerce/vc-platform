using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations.Data
{
    /// <inheritdoc />
    public partial class MakeSettingIdNotNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""PlatformSettingValue"" WHERE ""SettingId"" IS NULL");

            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "PlatformSettingValue",
                type: "character varying(128)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "PlatformSettingValue",
                type: "character varying(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)");
        }
    }
}
