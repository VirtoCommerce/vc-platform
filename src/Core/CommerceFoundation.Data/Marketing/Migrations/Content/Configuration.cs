namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Content
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VirtoCommerce.Foundation.Data.Infrastructure;

    public sealed class Configuration : DbMigrationsConfigurationBase<VirtoCommerce.Foundation.Data.Marketing.EFDynamicContentRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Marketing\Migrations\Content";
            ContextKey = "VCF.Marketing.Content";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Marketing.EFDynamicContentRepository context)
        {
        }
    }
}
