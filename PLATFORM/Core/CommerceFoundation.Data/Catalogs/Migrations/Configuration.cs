using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.Catalogs.EFCatalogRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Catalogs\Migrations";
            ContextKey = "VCF.Catalogs";
        }

        protected override void Seed(EFCatalogRepository context)
        {
            //  This method will be called after migrating to the latest version.
            CreateDefaultPackages(context);
            CreateDefaultPriceLists(context);
        }

        private void CreateDefaultPriceLists(EFCatalogRepository context)
        {
            context.AddOrUpdate(new Pricelist() { Currency = "USD", PricelistId = "DefaultUSD", Name = "MSRP USD Price List", Description = "MSRP Price List in USD" });
            context.AddOrUpdate(new Pricelist() { Currency = "USD", PricelistId = "SaleUSD", Name = "Sale USD Price List", Description = "Sale Price List in USD" });
            context.AddOrUpdate(new Pricelist() { Currency = "EUR", PricelistId = "DefaultEUR", Name = "Sale EUR Price List", Description = "Sale Price List in EUR" });
            context.UnitOfWork.Commit();
        }

        private void CreateDefaultPackages(EFCatalogRepository context)
        {
            context.AddOrUpdate(CreatePackaging("1", "Small box", "For catalogs, file folders, videotapes and CDs", 12.25m, 10.12m, 1.5m, LengthUnitOfMeasure.Inches));
            context.AddOrUpdate(CreatePackaging("2", "Medium box", "For binders, books and heavy documents", 13.25m, 11.5m, 2.3m, LengthUnitOfMeasure.Inches));
            context.AddOrUpdate(CreatePackaging("3", "Large box", "For side-by-side paper stacks, small parts and reports", 17.88m, 12.12m, 3m, LengthUnitOfMeasure.Inches));
            context.AddOrUpdate(CreatePackaging("4", "10kg box", "Weight limit: 22 lbs. (to qualify for the flat rate, weight cannot exceed 22 lbs.", 15.88m, 12.95m, 10.30m, LengthUnitOfMeasure.Inches));
            context.AddOrUpdate(CreatePackaging("5", "25kg box", "Weight limit: 56 lbs. (to qualify for the flat rate, weight cannot exceed 56 lbs.", 21.5m, 16.5m, 13.2m, LengthUnitOfMeasure.Inches));
            context.UnitOfWork.Commit();
        }

        private Packaging CreatePackaging(string id, string name, string description, decimal width, decimal height, decimal depth, LengthUnitOfMeasure measure)
        {
            var package = new Packaging
                {
                    Name = name,
                    Description = description,
                    Width = width,
                    Height = height,
                    Depth = depth,
                    PackageId = id
                };
            return package;
        }
    }
}
