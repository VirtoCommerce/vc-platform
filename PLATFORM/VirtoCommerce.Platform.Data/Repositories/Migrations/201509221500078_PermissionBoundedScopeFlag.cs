namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermissionBoundedScopeFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformRolePermission", "ScopeBounded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformRolePermission", "ScopeBounded");
        }
    }
}
