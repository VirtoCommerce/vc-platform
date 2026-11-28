using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations.Security
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
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiKeyHint",
                table: "UserApiKey",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

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
UPDATE ""UserApiKey""
SET ""ApiKeyHash"" = encode(sha256(convert_to(""ApiKey"", 'UTF8')), 'hex'),
    ""ApiKeyHint"" = CASE WHEN char_length(""ApiKey"") >= 16 THEN left(""ApiKey"", 4) || '…' || right(""ApiKey"", 4) ELSE '…' END,
    ""ApiKey"" = NULL
WHERE ""ApiKey"" IS NOT NULL AND ""ApiKeyHash"" IS NULL;");
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
