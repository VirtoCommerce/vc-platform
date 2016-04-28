namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineItemCategoryOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CartLineItem", "CategoryId", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CartLineItem", "CategoryId", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
