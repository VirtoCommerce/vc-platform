namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjectForSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformSetting", "ObjectId", c => c.String(maxLength: 128));
            AddColumn("dbo.PlatformSetting", "ObjectType", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformSetting", "ObjectType");
            DropColumn("dbo.PlatformSetting", "ObjectId");
        }
    }
}
