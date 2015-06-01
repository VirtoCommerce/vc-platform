namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DimensionChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDiscount", "LineItemId", "dbo.OrderLineItem");
            AddColumn("dbo.OrderShipment", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderShipment", "MeasureUnit", c => c.String(maxLength: 32));
            AddColumn("dbo.OrderShipment", "Height", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderShipment", "Length", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderShipment", "Width", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderLineItem", "WeightUnit", c => c.String(maxLength: 32));
            AddColumn("dbo.OrderLineItem", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderLineItem", "MeasureUnit", c => c.String(maxLength: 32));
            AddColumn("dbo.OrderLineItem", "Height", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderLineItem", "Length", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderLineItem", "Width", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.OrderShipment", "WeightUnit", c => c.String(maxLength: 32));
            AddForeignKey("dbo.OrderDiscount", "LineItemId", "dbo.OrderLineItem", "Id", cascadeDelete: true);
            DropColumn("dbo.OrderShipment", "WeightValue");
            DropColumn("dbo.OrderShipment", "DimensionUnit");
            DropColumn("dbo.OrderShipment", "DimensionHeight");
            DropColumn("dbo.OrderShipment", "DimensionLength");
            DropColumn("dbo.OrderShipment", "DimensionWidth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderShipment", "DimensionWidth", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderShipment", "DimensionLength", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderShipment", "DimensionHeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrderShipment", "DimensionUnit", c => c.String(maxLength: 16));
            AddColumn("dbo.OrderShipment", "WeightValue", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.OrderDiscount", "LineItemId", "dbo.OrderLineItem");
            AlterColumn("dbo.OrderShipment", "WeightUnit", c => c.String(maxLength: 16));
            DropColumn("dbo.OrderLineItem", "Width");
            DropColumn("dbo.OrderLineItem", "Length");
            DropColumn("dbo.OrderLineItem", "Height");
            DropColumn("dbo.OrderLineItem", "MeasureUnit");
            DropColumn("dbo.OrderLineItem", "Weight");
            DropColumn("dbo.OrderLineItem", "WeightUnit");
            DropColumn("dbo.OrderShipment", "Width");
            DropColumn("dbo.OrderShipment", "Length");
            DropColumn("dbo.OrderShipment", "Height");
            DropColumn("dbo.OrderShipment", "MeasureUnit");
            DropColumn("dbo.OrderShipment", "Weight");
            AddForeignKey("dbo.OrderDiscount", "LineItemId", "dbo.OrderLineItem", "Id");
        }
    }
}
