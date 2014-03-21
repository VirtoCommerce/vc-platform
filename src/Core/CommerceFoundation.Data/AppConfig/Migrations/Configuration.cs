using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.AppConfig.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.AppConfig.EFAppConfigRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"AppConfig\Migrations";
            ContextKey = "VCF.AppConfig";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.AppConfig.EFAppConfigRepository context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
