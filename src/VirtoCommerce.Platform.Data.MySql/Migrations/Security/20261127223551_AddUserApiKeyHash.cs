using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.MySql.Migrations.Security
{
    /// <inheritdoc />
    public partial class AddUserApiKeyHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKeyHash",
                table: "UserApiKey",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ApiKeyHint",
                table: "UserApiKey",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserApiKey_ApiKeyHash",
                table: "UserApiKey",
                column: "ApiKeyHash",
                unique: true);

            // Hash the keys saved before key hashing, as Sha256ApiKeyHasher does: SHA-256 of the UTF-8 bytes in
            // lowercase hex, and a hint of the first and last four characters for keys of 16 characters or more, and
            // clear their plaintext, so no usable key is left in the database. Back up before upgrading: a downgrade
            // finds no plaintext keys. A project that registers its own IApiKeyHasher has to reissue its keys.
            migrationBuilder.Sql(@"
UPDATE `UserApiKey`
SET `ApiKeyHash` = LOWER(SHA2(CONVERT(`ApiKey` USING utf8mb4), 256)),
    `ApiKeyHint` = CASE WHEN CHAR_LENGTH(`ApiKey`) >= 16 THEN CONCAT(LEFT(`ApiKey`, 4), '…', RIGHT(`ApiKey`, 4)) ELSE '…' END,
    `ApiKey` = NULL
WHERE `ApiKey` IS NOT NULL AND `ApiKeyHash` IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserApiKey_ApiKeyHash",
                table: "UserApiKey");

            migrationBuilder.DropColumn(
                name: "ApiKeyHash",
                table: "UserApiKey");

            migrationBuilder.DropColumn(
                name: "ApiKeyHint",
                table: "UserApiKey");
        }
    }
}
