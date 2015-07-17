namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentItem : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OrderLineItem", new[] { "ShipmentId" });
			DropForeignKey("dbo.OrderLineItem", "FK_dbo.OrderLineItem_dbo.OrderShipment_ShipmentId");
            CreateTable(
                "dbo.OrderShipmentItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BarCode = c.String(maxLength: 128),
                        Quantity = c.Int(nullable: false),
                        LineItemId = c.String(nullable: false, maxLength: 128),
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        ShipmentPackageId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderLineItem", t => t.LineItemId, cascadeDelete: true)
                .ForeignKey("dbo.OrderShipmentPackage", t => t.ShipmentPackageId, cascadeDelete: true)
                .Index(t => t.LineItemId)
                .Index(t => t.ShipmentId)
                .Index(t => t.ShipmentPackageId);
            
            CreateTable(
                "dbo.OrderShipmentPackage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BarCode = c.String(maxLength: 128),
                        PackageType = c.String(maxLength: 64),
                        WeightUnit = c.String(maxLength: 32),
                        Weight = c.Decimal(precision: 18, scale: 2),
                        MeasureUnit = c.String(maxLength: 32),
                        Height = c.Decimal(precision: 18, scale: 2),
                        Length = c.Decimal(precision: 18, scale: 2),
                        Width = c.Decimal(precision: 18, scale: 2),
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderShipment", t => t.ShipmentId)
                .Index(t => t.ShipmentId);
            
            AddColumn("dbo.OrderShipment", "ShipmentMethodOption", c => c.String(maxLength: 64));
            AlterColumn("dbo.OrderLineItem", "CategoryId", c => c.String(maxLength: 64));
            DropColumn("dbo.OrderLineItem", "ShipmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderLineItem", "ShipmentId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.OrderShipmentItem", "ShipmentPackageId", "dbo.OrderShipmentPackage");
            DropForeignKey("dbo.OrderShipmentPackage", "ShipmentId", "dbo.OrderShipment");
            DropForeignKey("dbo.OrderShipmentItem", "LineItemId", "dbo.OrderLineItem");
            DropIndex("dbo.OrderShipmentPackage", new[] { "ShipmentId" });
            DropIndex("dbo.OrderShipmentItem", new[] { "ShipmentPackageId" });
            DropIndex("dbo.OrderShipmentItem", new[] { "ShipmentId" });
            DropIndex("dbo.OrderShipmentItem", new[] { "LineItemId" });
            AlterColumn("dbo.OrderLineItem", "CategoryId", c => c.String(nullable: false, maxLength: 64));
            DropColumn("dbo.OrderShipment", "ShipmentMethodOption");
            DropTable("dbo.OrderShipmentPackage");
            DropTable("dbo.OrderShipmentItem");
            CreateIndex("dbo.OrderLineItem", "ShipmentId");
        }
    }
}
