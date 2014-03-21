using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.Marketing.EFMarketingRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Marketing\Migrations\Promotion";
            ContextKey = "VCF.Marketing.Promotion";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Marketing.EFMarketingRepository context)
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
