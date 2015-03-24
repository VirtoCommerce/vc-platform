namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressRegion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cart_Address", "RegionId", c => c.String(maxLength: 128));
            AddColumn("dbo.cart_Address", "RegionName", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.cart_Address", "RegionName");
            DropColumn("dbo.cart_Address", "RegionId");
        }
    }
}
