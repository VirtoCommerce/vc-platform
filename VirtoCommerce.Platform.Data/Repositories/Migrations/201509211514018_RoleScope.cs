namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleScope : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformRoleScope",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        Scope = c.String(nullable: false, maxLength: 1024),
                        RoleId = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            DropColumn("dbo.PlatformRoleAssignment", "OrganizationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlatformRoleAssignment", "OrganizationId", c => c.String(maxLength: 64));
            DropForeignKey("dbo.PlatformRoleScope", "RoleId", "dbo.PlatformRole");
            DropIndex("dbo.PlatformRoleScope", new[] { "RoleId" });
            DropTable("dbo.PlatformRoleScope");
        }
    }
}
