namespace VirtoCommerce.PricingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropDescriminator : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Price", "Discriminator");
            DropColumn("dbo.Pricelist", "Discriminator");
            DropColumn("dbo.PricelistAssignment", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PricelistAssignment", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Pricelist", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Price", "Discriminator", c => c.String(maxLength: 128));
        }
    }
}
