namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationCcBccFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformNotification", "Сс", c => c.String(maxLength: 1024));
            AddColumn("dbo.PlatformNotification", "Bcс", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformNotification", "Bcс");
            DropColumn("dbo.PlatformNotification", "Сс");
        }
    }
}
