namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartLineItem", "PriceId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartLineItem", "PriceId");
        }
    }
}
