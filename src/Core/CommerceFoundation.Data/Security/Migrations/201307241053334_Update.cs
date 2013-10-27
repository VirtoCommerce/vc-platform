namespace VirtoCommerce.Foundation.Data.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RolePermission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission");
            DropIndex("dbo.RolePermission", new[] { "RoleId" });
            DropIndex("dbo.RolePermission", new[] { "PermissionId" });
            AlterColumn("dbo.RolePermission", "RoleId", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.RolePermission", "PermissionId", c => c.String(nullable: false, maxLength: 128));
            AddForeignKey("dbo.RolePermission", "RoleId", "dbo.Role", "RoleId", cascadeDelete: true);
            AddForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission", "PermissionId", cascadeDelete: true);
            CreateIndex("dbo.RolePermission", "RoleId");
            CreateIndex("dbo.RolePermission", "PermissionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RolePermission", new[] { "PermissionId" });
            DropIndex("dbo.RolePermission", new[] { "RoleId" });
            DropForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission");
            DropForeignKey("dbo.RolePermission", "RoleId", "dbo.Role");
            AlterColumn("dbo.RolePermission", "PermissionId", c => c.String(maxLength: 128));
            AlterColumn("dbo.RolePermission", "RoleId", c => c.String(maxLength: 64));
            CreateIndex("dbo.RolePermission", "PermissionId");
            CreateIndex("dbo.RolePermission", "RoleId");
            AddForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission", "PermissionId");
            AddForeignKey("dbo.RolePermission", "RoleId", "dbo.Role", "RoleId");
        }
    }
}
