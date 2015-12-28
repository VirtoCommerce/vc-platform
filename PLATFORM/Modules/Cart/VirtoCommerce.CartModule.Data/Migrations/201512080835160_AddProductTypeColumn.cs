namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductTypeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartLineItem", "ProductType", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartLineItem", "ProductType");
        }
    }
}
