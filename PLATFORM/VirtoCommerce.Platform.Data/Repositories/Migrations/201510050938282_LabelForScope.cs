namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabelForScope : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformPermissionScope", "Type", c => c.String(maxLength: 255));
            AddColumn("dbo.PlatformPermissionScope", "Label", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformPermissionScope", "Label");
            DropColumn("dbo.PlatformPermissionScope", "Type");
        }
    }
}
