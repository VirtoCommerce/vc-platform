namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderLineItem", "PriceId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLineItem", "PriceId");
        }
    }
}
