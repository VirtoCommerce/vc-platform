namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublishingGroupCascadeDeletion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentItemId", "dbo.DynamicContentItem");
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPlaceId", "dbo.DynamicContentPlace");
            AddForeignKey("dbo.PublishingGroupContentItem", "DynamicContentItemId", "dbo.DynamicContentItem", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPlaceId", "dbo.DynamicContentPlace", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPlaceId", "dbo.DynamicContentPlace");
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentItemId", "dbo.DynamicContentItem");
            AddForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPlaceId", "dbo.DynamicContentPlace", "Id");
            AddForeignKey("dbo.PublishingGroupContentItem", "DynamicContentItemId", "dbo.DynamicContentItem", "Id");
        }
    }
}
