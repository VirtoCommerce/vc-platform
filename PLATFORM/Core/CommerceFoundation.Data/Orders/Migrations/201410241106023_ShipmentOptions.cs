namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShipmentOption",
                c => new
                    {
                        ShipmentOptionId = c.String(nullable: false, maxLength: 128),
                        ShipmentId = c.String(nullable: false, maxLength: 128),
                        OptionName = c.String(nullable: false, maxLength: 64),
                        OptionValue = c.String(maxLength: 1024),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShipmentOptionId)
                .ForeignKey("dbo.Shipment", t => t.ShipmentId, cascadeDelete: true)
                .Index(t => t.ShipmentId);
            
            AddColumn("dbo.Shipment", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LineItemOption", "OptionValue", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipmentOption", "ShipmentId", "dbo.Shipment");
            DropIndex("dbo.ShipmentOption", new[] { "ShipmentId" });
            AlterColumn("dbo.LineItemOption", "OptionValue", c => c.String(maxLength: 128));
            DropColumn("dbo.Shipment", "Weight");
            DropTable("dbo.ShipmentOption");
        }
    }
}
