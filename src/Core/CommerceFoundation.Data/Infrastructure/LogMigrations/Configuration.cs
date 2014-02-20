using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Infrastructure.LogMigrations
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.Infrastructure.OperationLogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "VCF.OperationLogs";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Infrastructure.OperationLogContext context)
        {
        }
    }
}
