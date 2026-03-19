using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations.Security
{
    /// <inheritdoc />
    public partial class UpdateOpenIddictModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "OpenIddictTokens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "OpenIddictTokens",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "OpenIddictTokens");
        }
    }
}
