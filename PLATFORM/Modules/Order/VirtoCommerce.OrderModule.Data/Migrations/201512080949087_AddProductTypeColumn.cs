namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductTypeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderLineItem", "Sku", c => c.String(nullable: false, maxLength: 64));
            AddColumn("dbo.OrderLineItem", "ProductType", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLineItem", "ProductType");
            DropColumn("dbo.OrderLineItem", "Sku");
        }
    }
}
