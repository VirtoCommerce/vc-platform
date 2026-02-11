using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VirtoCommerce.Platform.Data.PostgreSql.Extensions;

public static class MigrationBuilderExtension
{
    /// <summary>
    /// Creates case-insensitive collation if not exists in database.
    /// </summary>
    /// <param name="migrationBuilder">Migration Builder</param>
    /// <returns>OperationBuilder</returns>
    /// <remarks>
    /// Please read https://www.npgsql.org/efcore/misc/collations-and-case-sensitivity.html
    /// </remarks>
    public static OperationBuilder<SqlOperation> CreateCaseInsensitiveCollationIfNotExists(this MigrationBuilder migrationBuilder)
    {
        return migrationBuilder.Sql(string.Format(@"
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_catalog.pg_collation WHERE collname = '{0}') THEN
        CREATE COLLATION {0} (provider = icu, locale = 'und-u-ks-level2', deterministic = false);
    END IF;
END $$;
",
CollationNames.CaseInsensitive));
    }
}
