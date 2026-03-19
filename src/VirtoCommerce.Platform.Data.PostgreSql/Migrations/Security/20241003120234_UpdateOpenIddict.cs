using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations.Security
{
    /// <inheritdoc />
    public partial class UpdateOpenIddict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "OpenIddictApplications",
                newName: "ClientType");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationType",
                table: "OpenIddictApplications",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonWebKeySet",
                table: "OpenIddictApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "OpenIddictApplications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationType",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "JsonWebKeySet",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "Settings",
                table: "OpenIddictApplications");

            migrationBuilder.RenameColumn(
                name: "ClientType",
                table: "OpenIddictApplications",
                newName: "Type");
        }
    }
}
