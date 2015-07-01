namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationAddLanguagesAndObjectType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformNotification", "ObjectId", c => c.String(maxLength: 128));
            AddColumn("dbo.PlatformNotification", "ObjectTypeId", c => c.String(maxLength: 128));
            AddColumn("dbo.PlatformNotification", "Language", c => c.String(maxLength: 10));
            AddColumn("dbo.PlatformNotificationTemplate", "ObjectTypeId", c => c.String(maxLength: 128));
            AddColumn("dbo.PlatformNotificationTemplate", "Language", c => c.String(maxLength: 10));
            AddColumn("dbo.PlatformNotificationTemplate", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformNotificationTemplate", "IsDefault");
            DropColumn("dbo.PlatformNotificationTemplate", "Language");
            DropColumn("dbo.PlatformNotificationTemplate", "ObjectTypeId");
            DropColumn("dbo.PlatformNotification", "Language");
            DropColumn("dbo.PlatformNotification", "ObjectTypeId");
            DropColumn("dbo.PlatformNotification", "ObjectId");
        }
    }
}
