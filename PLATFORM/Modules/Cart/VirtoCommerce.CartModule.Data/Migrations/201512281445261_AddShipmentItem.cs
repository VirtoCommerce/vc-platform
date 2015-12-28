namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentItem : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CartLineItem", new[] { "ShipmentId" });
            DropForeignKey("dbo.CartLineItem", "ShipmentId", "dbo.CartShipment");
            CreateTable(
                "dbo.CartShipmentItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BarCode = c.String(maxLength: 128),
                        Quantity = c.Int(nullable: false),
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartLineItem", t => t.LineItemId, cascadeDelete: true)
                .ForeignKey("dbo.CartShipment", t => t.ShipmentId, cascadeDelete: false)
                .Index(t => t.LineItemId)
                .Index(t => t.ShipmentId);
            
            DropColumn("dbo.CartLineItem", "ShipmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartLineItem", "ShipmentId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.CartShipmentItem", "LineItemId", "dbo.CartLineItem");
            DropIndex("dbo.CartShipmentItem", new[] { "ShipmentId" });
            DropIndex("dbo.CartShipmentItem", new[] { "LineItemId" });
            DropTable("dbo.CartShipmentItem");
            CreateIndex("dbo.CartLineItem", "ShipmentId");
        }
    }
}
