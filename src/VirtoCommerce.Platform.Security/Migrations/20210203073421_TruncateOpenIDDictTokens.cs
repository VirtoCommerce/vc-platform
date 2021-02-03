using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.Security.Migrations
{
    public partial class TruncateOpenIDDictTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This removes all OpenIDDict tokens and authorizations
            // Truncate to avoid timeouts in the first run of "PruneExpiredTokensJob".
            migrationBuilder.Sql(@"
                ALTER TABLE [OpenIddictTokens] DROP CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId]

                TRUNCATE TABLE OpenIddictTokens

                TRUNCATE TABLE OpenIddictAuthorizations

                ALTER TABLE [OpenIddictTokens]  WITH CHECK ADD  CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId] FOREIGN KEY([AuthorizationId])
                REFERENCES [OpenIddictAuthorizations] ([Id])

                ALTER TABLE [OpenIddictTokens] CHECK CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId]
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
