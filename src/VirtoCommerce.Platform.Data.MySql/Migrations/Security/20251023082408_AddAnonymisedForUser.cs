using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.MySql.Migrations.Security
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
                type: "tinyint(1)",
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
