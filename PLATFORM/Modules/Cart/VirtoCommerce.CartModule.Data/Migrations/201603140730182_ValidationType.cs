namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cart", "ValidationType", c => c.String(maxLength: 64));
            AddColumn("dbo.CartLineItem", "ValidationType", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartLineItem", "ValidationType");
            DropColumn("dbo.Cart", "ValidationType");
        }
    }
}
