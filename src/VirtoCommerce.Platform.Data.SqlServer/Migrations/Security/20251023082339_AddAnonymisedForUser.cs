using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Security
{
    /// <inheritdoc />
    public partial class AddAnonymisedForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymised",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymised",
                table: "AspNetUsers");
        }
    }
}
