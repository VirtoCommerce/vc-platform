namespace VirtoCommerce.Foundation.Data.Infrastructure.LogMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VirtoCommerce.Foundation.Data.Infrastructure;

    public sealed class Configuration : DbMigrationsConfigurationBase<OperationLogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Infrastructure\LogMigrations";
            ContextKey = "VCF.Logging";
        }

        protected override void Seed(OperationLogContext context)
        {
        }
    }
}
