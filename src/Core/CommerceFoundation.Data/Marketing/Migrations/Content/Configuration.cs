using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Content
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.Marketing.EFDynamicContentRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Marketing\Migrations\Content";
            ContextKey = "VCF.Marketing.Content";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Marketing.EFDynamicContentRepository context)
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
