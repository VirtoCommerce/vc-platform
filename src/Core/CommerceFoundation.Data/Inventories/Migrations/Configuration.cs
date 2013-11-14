namespace VirtoCommerce.Foundation.Data.Inventories.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VirtoCommerce.Foundation.Data.Infrastructure;

    public sealed class Configuration : DbMigrationsConfigurationBase<VirtoCommerce.Foundation.Data.Inventories.EFInventoryRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Inventories\Migrations";
            ContextKey = "VCF.Inventory";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Inventories.EFInventoryRepository context)
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
