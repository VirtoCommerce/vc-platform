namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDisplayNameTemplate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PlatformNotificationTemplate", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlatformNotificationTemplate", "DisplayName", c => c.String(maxLength: 64));
        }
    }
}
