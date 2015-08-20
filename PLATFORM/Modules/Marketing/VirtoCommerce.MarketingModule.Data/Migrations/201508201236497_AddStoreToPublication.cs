namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoreToPublication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DynamicContentPublishingGroup", "StoreId", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DynamicContentPublishingGroup", "StoreId");
        }
    }
}
