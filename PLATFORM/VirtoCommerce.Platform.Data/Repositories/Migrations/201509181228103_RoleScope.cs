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
                        Scope = c.String(nullable: false, maxLength: 128),
                        Type = c.String(nullable: false, maxLength: 128),
                        RoleAssignmentId = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformRoleAssignment", t => t.RoleAssignmentId, cascadeDelete: true)
                .Index(t => t.RoleAssignmentId);
            
            DropColumn("dbo.PlatformRoleAssignment", "OrganizationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlatformRoleAssignment", "OrganizationId", c => c.String(maxLength: 64));
            DropForeignKey("dbo.PlatformRoleScope", "RoleAssignmentId", "dbo.PlatformRoleAssignment");
            DropIndex("dbo.PlatformRoleScope", new[] { "RoleAssignmentId" });
            DropTable("dbo.PlatformRoleScope");
        }
    }
}
