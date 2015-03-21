namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineItemWeightAndParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LineItem", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LineItem", "ParentLineItemId", c => c.String(maxLength: 128));
            CreateIndex("dbo.LineItem", "ParentLineItemId");
            AddForeignKey("dbo.LineItem", "ParentLineItemId", "dbo.LineItem", "LineItemId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LineItem", "ParentLineItemId", "dbo.LineItem");
            DropIndex("dbo.LineItem", new[] { "ParentLineItemId" });
            DropColumn("dbo.LineItem", "ParentLineItemId");
            DropColumn("dbo.LineItem", "Weight");
        }
    }
}
