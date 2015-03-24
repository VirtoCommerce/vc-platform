namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressRegion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.order_Address", "RegionId", c => c.String(maxLength: 128));
            AddColumn("dbo.order_Address", "RegionName", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.order_Address", "RegionName");
            DropColumn("dbo.order_Address", "RegionId");
        }
    }
}
