namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartTaxDetail",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        Name = c.String(maxLength: 1024),
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
            DropForeignKey("dbo.CartTaxDetail", "ShoppingCartId", "dbo.Cart");
            DropForeignKey("dbo.CartTaxDetail", "ShipmentId", "dbo.CartShipment");
            DropForeignKey("dbo.CartTaxDetail", "LineItemId", "dbo.CartLineItem");
            DropIndex("dbo.CartTaxDetail", new[] { "LineItemId" });
            DropIndex("dbo.CartTaxDetail", new[] { "ShipmentId" });
            DropIndex("dbo.CartTaxDetail", new[] { "ShoppingCartId" });
            DropTable("dbo.CartTaxDetail");
        }
    }
}
