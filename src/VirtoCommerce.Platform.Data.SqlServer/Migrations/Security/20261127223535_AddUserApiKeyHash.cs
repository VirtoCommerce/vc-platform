using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Security
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
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiKeyHint",
                table: "UserApiKey",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserApiKey_ApiKeyHash",
                table: "UserApiKey",
                column: "ApiKeyHash",
                unique: true,
                filter: "[ApiKeyHash] IS NOT NULL");

            // Hash the keys saved before key hashing, as Sha256ApiKeyHasher does: SHA-256 of the UTF-8 bytes in
            // lowercase hex, and a hint of the first and last four characters for keys of 16 characters or more, and
            // clear their plaintext, so no usable key is left in the database. Back up before upgrading: a downgrade
            // finds no plaintext keys. A project that registers its own IApiKeyHasher has to reissue its keys.
            // HASHBYTES on nvarchar would hash UTF-16, so the key is cast to varchar with a UTF-8 collation (SQL Server
            // 2019 and later), which gives its UTF-8 bytes; varchar(512) holds 128 characters of up to 4 bytes each.
            migrationBuilder.Sql(@"
UPDATE [UserApiKey]
SET [ApiKeyHash] = LOWER(CONVERT(varchar(64), HASHBYTES('SHA2_256', CAST([ApiKey] COLLATE Latin1_General_100_BIN2_UTF8 AS varchar(512))), 2)),
    [ApiKeyHint] = CASE WHEN LEN([ApiKey] + N'x') - 1 >= 16 THEN LEFT([ApiKey], 4) + N'…' + RIGHT([ApiKey], 4) ELSE N'…' END,
    [ApiKey] = NULL
WHERE [ApiKey] IS NOT NULL AND [ApiKeyHash] IS NULL;");
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
