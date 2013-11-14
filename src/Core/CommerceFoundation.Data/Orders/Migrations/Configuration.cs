namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VirtoCommerce.Foundation.Data.Infrastructure;

    public sealed class Configuration : DbMigrationsConfigurationBase<VirtoCommerce.Foundation.Data.Orders.EFOrderRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Orders\Migrations";
            ContextKey = "VCF.Orders";
        }
    }
}
