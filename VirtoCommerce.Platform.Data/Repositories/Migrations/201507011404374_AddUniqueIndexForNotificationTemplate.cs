namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIndexForNotificationTemplate : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PlatformNotificationTemplate", new[] { "NotificationTypeId", "ObjectId", "ObjectTypeId", "Language" }, unique: true, name: "ix_template_unique");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlatformNotificationTemplate", "ix_template_unique");
        }
    }
}
