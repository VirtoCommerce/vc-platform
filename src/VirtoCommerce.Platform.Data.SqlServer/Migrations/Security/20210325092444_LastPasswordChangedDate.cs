using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Security
{
    public partial class LastPasswordChangedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordChangedDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPasswordChangedDate",
                table: "AspNetUsers");
        }
    }
}
