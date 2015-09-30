namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermissionScope : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlatformRoleScope", "RoleId", "dbo.PlatformRole");
            DropIndex("dbo.PlatformRoleScope", new[] { "RoleId" });
            CreateTable(
                "dbo.PlatformPermissionScope",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        Scope = c.String(nullable: false, maxLength: 1024),
                        RolePermissionId = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformRolePermission", t => t.RolePermissionId, cascadeDelete: true)
                .Index(t => t.RolePermissionId);
            
            DropColumn("dbo.PlatformRolePermission", "ScopeBounded");
            DropTable("dbo.PlatformRoleScope");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PlatformRoleScope",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        Scope = c.String(nullable: false, maxLength: 1024),
                        RoleId = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PlatformRolePermission", "ScopeBounded", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.PlatformPermissionScope", "RolePermissionId", "dbo.PlatformRolePermission");
            DropIndex("dbo.PlatformPermissionScope", new[] { "RolePermissionId" });
            DropTable("dbo.PlatformPermissionScope");
            CreateIndex("dbo.PlatformRoleScope", "RoleId");
            AddForeignKey("dbo.PlatformRoleScope", "RoleId", "dbo.PlatformRole", "Id", cascadeDelete: true);
        }
    }
}
