using Microsoft.EntityFrameworkCore.Migrations;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddCaseInsensitiveCollations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCaseInsensitiveCollationIfNotExists();

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlatformDynamicPropertyDictionaryItem",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlatformDynamicProperty",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlatformDynamicPropertyDictionaryItem",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PlatformDynamicProperty",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "case_insensitive");
        }
    }
}
