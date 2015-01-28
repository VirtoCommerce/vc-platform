using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Security.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        PermissionId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PermissionId);
            
            CreateTable(
                "dbo.RolePermission",
                c => new
                    {
                        RolePermissionId = c.String(nullable: false, maxLength: 64),
                        RoleId = c.String(nullable: false, maxLength: 64),
                        PermissionId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RolePermissionId)
                .ForeignKey("dbo.Permission", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.PermissionId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.RoleAssignment",
                c => new
                    {
                        RoleAssignmentId = c.String(nullable: false, maxLength: 64),
                        OrganizationId = c.String(maxLength: 64),
                        AccountId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleAssignmentId)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        StoreId = c.String(maxLength: 128),
                        MemberId = c.String(maxLength: 64),
                        UserName = c.String(nullable: false, maxLength: 128),
                        RegisterType = c.Int(nullable: false),
                        AccountState = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RolePermission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleAssignment", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleAssignment", "AccountId", "dbo.Account");
            DropForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission");
            DropIndex("dbo.RolePermission", new[] { "RoleId" });
            DropIndex("dbo.RoleAssignment", new[] { "RoleId" });
            DropIndex("dbo.RoleAssignment", new[] { "AccountId" });
            DropIndex("dbo.RolePermission", new[] { "PermissionId" });
            DropTable("dbo.Account");
            DropTable("dbo.RoleAssignment");
            DropTable("dbo.Role");
            DropTable("dbo.RolePermission");
            DropTable("dbo.Permission");
        }
    }
}
