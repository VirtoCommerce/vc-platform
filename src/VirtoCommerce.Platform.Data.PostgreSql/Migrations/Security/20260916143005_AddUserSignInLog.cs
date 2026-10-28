using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations.Security
{
    /// <inheritdoc />
    public partial class AddUserSignInLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSignInLog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Succeeded = table.Column<bool>(type: "boolean", nullable: false),
                    FailureReason = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SignInType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OperatorUserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OperatorUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Host = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ClientId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SessionId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    StoreId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    StoreName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    MemberId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OrganizationId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OrganizationName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSignInLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSignInLog_CreatedDate",
                table: "UserSignInLog",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserSignInLog_IpAddress_CreatedDate",
                table: "UserSignInLog",
                columns: new[] { "IpAddress", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSignInLog_SessionId",
                table: "UserSignInLog",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSignInLog_UserId_CreatedDate",
                table: "UserSignInLog",
                columns: new[] { "UserId", "CreatedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSignInLog");
        }
    }
}
