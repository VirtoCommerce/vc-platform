namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DimensionChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartAddress", "ShoppingCartId", "dbo.Cart");
            DropForeignKey("dbo.CartLineItem", "ShoppingCartId", "dbo.Cart");
            DropForeignKey("dbo.CartPayment", "ShoppingCartId", "dbo.Cart");
            AddColumn("dbo.CartLineItem", "Weight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLineItem", "MeasureUnit", c => c.String(maxLength: 32));
            AddColumn("dbo.CartLineItem", "Height", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLineItem", "Length", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLineItem", "Width", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CartLineItem", "WeightUnit", c => c.String(maxLength: 32));
            AddForeignKey("dbo.CartAddress", "ShoppingCartId", "dbo.Cart", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CartLineItem", "ShoppingCartId", "dbo.Cart", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CartPayment", "ShoppingCartId", "dbo.Cart", "Id", cascadeDelete: true);
            DropColumn("dbo.CartLineItem", "WeightValue");
            DropColumn("dbo.CartLineItem", "DimensionUnit");
            DropColumn("dbo.CartLineItem", "DimensionHeight");
            DropColumn("dbo.CartLineItem", "DimensionLength");
            DropColumn("dbo.CartLineItem", "DimensionWidth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartLineItem", "DimensionWidth", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLineItem", "DimensionLength", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLineItem", "DimensionHeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CartLineItem", "DimensionUnit", c => c.String(maxLength: 16));
            AddColumn("dbo.CartLineItem", "WeightValue", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.CartPayment", "ShoppingCartId", "dbo.Cart");
            DropForeignKey("dbo.CartLineItem", "ShoppingCartId", "dbo.Cart");
            DropForeignKey("dbo.CartAddress", "ShoppingCartId", "dbo.Cart");
            AlterColumn("dbo.CartLineItem", "WeightUnit", c => c.String(maxLength: 16));
            DropColumn("dbo.CartLineItem", "Width");
            DropColumn("dbo.CartLineItem", "Length");
            DropColumn("dbo.CartLineItem", "Height");
            DropColumn("dbo.CartLineItem", "MeasureUnit");
            DropColumn("dbo.CartLineItem", "Weight");
            AddForeignKey("dbo.CartPayment", "ShoppingCartId", "dbo.Cart", "Id");
            AddForeignKey("dbo.CartLineItem", "ShoppingCartId", "dbo.Cart", "Id");
            AddForeignKey("dbo.CartAddress", "ShoppingCartId", "dbo.Cart", "Id");
        }
    }
}
