using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddOperationLogObjectIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OperationLog_ObjectId_CreatedDate",
                table: "PlatformOperationLog",
                columns: new[] { "ObjectId", "CreatedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OperationLog_ObjectId_CreatedDate",
                table: "PlatformOperationLog");
        }
    }
}
