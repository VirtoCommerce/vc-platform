using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Data
{
    public partial class FixIndexName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_ObjectType_ObjectId",
                table: "PlatformOperationLog",
                newName: "IX_OperationLog_ObjectType_ObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_OperationLog_ObjectType_ObjectId",
                table: "PlatformOperationLog",
                newName: "IX_ObjectType_ObjectId");
        }
    }
}
