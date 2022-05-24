using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Security.Migrations
{
    public partial class AddServerCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerCertificate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PublicCertBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateKeyCertBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateKeyCertPassword = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerCertificate", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerCertificate");
        }
    }
}
