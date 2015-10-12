namespace VirtoCommerce.QuoteModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManualShippingTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteRequest", "ManualShippingTotal", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteRequest", "ManualShippingTotal");
        }
    }
}
