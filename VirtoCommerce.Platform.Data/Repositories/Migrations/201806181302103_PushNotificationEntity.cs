namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PushNotificationEntity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PlatformPushNotifcation", newName: "PlatformPushNotification");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PlatformPushNotification", newName: "PlatformPushNotifcation");
        }
    }
}
