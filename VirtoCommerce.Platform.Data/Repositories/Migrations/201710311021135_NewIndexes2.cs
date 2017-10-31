namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewIndexes2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PlatformOperationLog", new[] { "ObjectType", "ObjectId" });
            CreateIndex("dbo.PlatformSetting", new[] { "ObjectType", "ObjectId" });
            CreateIndex("dbo.PlatformDynamicPropertyObjectValue", new[] { "ObjectType", "ObjectId" });
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlatformDynamicPropertyObjectValue", new[] { "ObjectType", "ObjectId" });
            DropIndex("dbo.PlatformSetting", new[] { "ObjectType", "ObjectId" });
            DropIndex("dbo.PlatformOperationLog", new[] { "ObjectType", "ObjectId" });
        }
    }
}
