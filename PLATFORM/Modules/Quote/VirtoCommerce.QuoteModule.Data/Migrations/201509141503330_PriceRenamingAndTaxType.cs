namespace VirtoCommerce.QuoteModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceRenamingAndTaxType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteItem", "ListPrice", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.QuoteItem", "SalePrice", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.QuoteItem", "TaxType", c => c.String(maxLength: 64));
            DropColumn("dbo.QuoteItem", "BasePrice");
            DropColumn("dbo.QuoteItem", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuoteItem", "Price", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.QuoteItem", "BasePrice", c => c.Decimal(nullable: false, storeType: "money"));
            DropColumn("dbo.QuoteItem", "TaxType");
            DropColumn("dbo.QuoteItem", "SalePrice");
            DropColumn("dbo.QuoteItem", "ListPrice");
        }
    }
}
