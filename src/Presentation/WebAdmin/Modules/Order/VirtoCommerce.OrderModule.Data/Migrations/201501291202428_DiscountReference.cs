namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscountReference : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.order_Discount", "Id", "dbo.order_CustomerOrder");
            DropForeignKey("dbo.order_Discount", "Id", "dbo.order_Shipment");
            DropForeignKey("dbo.order_Discount", "Id", "dbo.order_LineItem");
            DropIndex("dbo.order_Discount", new[] { "Id" });
            AddColumn("dbo.order_CustomerOrder", "Discount_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.order_Shipment", "Discount_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.order_LineItem", "Discount_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.order_CustomerOrder", "Discount_Id");
            CreateIndex("dbo.order_Shipment", "Discount_Id");
            CreateIndex("dbo.order_LineItem", "Discount_Id");
            AddForeignKey("dbo.order_CustomerOrder", "Discount_Id", "dbo.order_Discount", "Id");
            AddForeignKey("dbo.order_Shipment", "Discount_Id", "dbo.order_Discount", "Id");
            AddForeignKey("dbo.order_LineItem", "Discount_Id", "dbo.order_Discount", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.order_LineItem", "Discount_Id", "dbo.order_Discount");
            DropForeignKey("dbo.order_Shipment", "Discount_Id", "dbo.order_Discount");
            DropForeignKey("dbo.order_CustomerOrder", "Discount_Id", "dbo.order_Discount");
            DropIndex("dbo.order_LineItem", new[] { "Discount_Id" });
            DropIndex("dbo.order_Shipment", new[] { "Discount_Id" });
            DropIndex("dbo.order_CustomerOrder", new[] { "Discount_Id" });
            DropColumn("dbo.order_LineItem", "Discount_Id");
            DropColumn("dbo.order_Shipment", "Discount_Id");
            DropColumn("dbo.order_CustomerOrder", "Discount_Id");
            CreateIndex("dbo.order_Discount", "Id");
            AddForeignKey("dbo.order_Discount", "Id", "dbo.order_LineItem", "Id");
            AddForeignKey("dbo.order_Discount", "Id", "dbo.order_Shipment", "Id");
            AddForeignKey("dbo.order_Discount", "Id", "dbo.order_CustomerOrder", "Id");
        }
    }
}
