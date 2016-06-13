namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingDescriptionIncSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlatformSetting", "Description", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlatformSetting", "Description", c => c.String(maxLength: 256));
        }
    }
}
