namespace VirtoCommerce.QuoteModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkuForQuoteItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteItem", "Sku", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteItem", "Sku");
        }
    }
}
