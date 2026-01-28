using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.Platform.Data.SqlServer.Migrations.Security
{
    /// <inheritdoc />
    public partial class FixPendingModelChangesWarning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Find and drop the primary key constraint dynamically (required for SQL Server when altering PK columns)
            // This handles cases where the constraint name might be different or doesn't exist
            migrationBuilder.Sql(@"
                DECLARE @PKConstraintName NVARCHAR(200)
                SELECT @PKConstraintName = name
                FROM sys.key_constraints
                WHERE type = 'PK' AND parent_object_id = OBJECT_ID('AspNetUserTokens')
                
                IF @PKConstraintName IS NOT NULL
                BEGIN
                    EXEC('ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @PKConstraintName + ']')
                END
            ");

            // Alter the columns
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            // Recreate the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: ["UserId", "LoginProvider", "Name"]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Find and drop the primary key constraint dynamically (required for SQL Server when altering PK columns)
            migrationBuilder.Sql(@"
                DECLARE @PKConstraintName NVARCHAR(200)
                SELECT @PKConstraintName = name
                FROM sys.key_constraints
                WHERE type = 'PK' AND parent_object_id = OBJECT_ID('AspNetUserTokens')
                
                IF @PKConstraintName IS NOT NULL
                BEGIN
                    EXEC('ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @PKConstraintName + ']')
                END
            ");

            // Alter the columns back to original values
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Recreate the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: ["UserId", "LoginProvider", "Name"]);
        }
    }
}
