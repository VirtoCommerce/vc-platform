using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Security
{
    public partial class UserPasswordChangeRequestDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordChangeRequestDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPasswordChangeRequestDate",
                table: "AspNetUsers");
        }
    }
}
