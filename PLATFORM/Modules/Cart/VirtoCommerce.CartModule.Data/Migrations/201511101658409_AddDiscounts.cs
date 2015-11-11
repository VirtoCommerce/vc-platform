namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartDiscount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(maxLength: 64),
                        PromotionDescription = c.String(maxLength: 1024),
                        Currency = c.String(nullable: false, maxLength: 3),
                        DiscountAmount = c.Decimal(nullable: false, storeType: "money"),
                        CouponCode = c.String(maxLength: 64),
                        ShoppingCartId = c.String(maxLength: 128),
                        ShipmentId = c.String(maxLength: 128),
                        LineItemId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartLineItem", t => t.LineItemId)
                .ForeignKey("dbo.CartShipment", t => t.ShipmentId)
                .ForeignKey("dbo.Cart", t => t.ShoppingCartId)
                .Index(t => t.ShoppingCartId)
                .Index(t => t.ShipmentId)
                .Index(t => t.LineItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartDiscount", "ShoppingCartId", "dbo.Cart");
            DropForeignKey("dbo.CartDiscount", "ShipmentId", "dbo.CartShipment");
            DropForeignKey("dbo.CartDiscount", "LineItemId", "dbo.CartLineItem");
            DropIndex("dbo.CartDiscount", new[] { "LineItemId" });
            DropIndex("dbo.CartDiscount", new[] { "ShipmentId" });
            DropIndex("dbo.CartDiscount", new[] { "ShoppingCartId" });
            DropTable("dbo.CartDiscount");
        }
    }
}
