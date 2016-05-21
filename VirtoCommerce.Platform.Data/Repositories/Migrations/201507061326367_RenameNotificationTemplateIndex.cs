namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameNotificationTemplateIndex : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.PlatformNotificationTemplate", name: "ix_template_unique", newName: "IX_PlatformNotificationTemplate_NotificationTypeId_ObjectTypeId_ObjectId_Language");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PlatformNotificationTemplate", name: "IX_PlatformNotificationTemplate_NotificationTypeId_ObjectTypeId_ObjectId_Language", newName: "ix_template_unique");
        }
    }
}
