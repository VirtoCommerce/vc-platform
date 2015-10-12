namespace VirtoCommerce.QuoteModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManualTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuoteRequest", "ManualSubTotal", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.QuoteRequest", "ManualRelDiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuoteRequest", "ManualRelDiscountAmount");
            DropColumn("dbo.QuoteRequest", "ManualSubTotal");
        }
    }
}
