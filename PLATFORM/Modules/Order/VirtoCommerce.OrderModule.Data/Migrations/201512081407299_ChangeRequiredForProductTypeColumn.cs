namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequiredForProductTypeColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderLineItem", "ProductType", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderLineItem", "ProductType", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
