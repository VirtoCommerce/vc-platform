namespace VirtoCommerce.Foundation.Data.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AccountIdToString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleAssignment", "AccountId", "dbo.Account");
            DropIndex("dbo.RoleAssignment", new[] { "AccountId" });
            DropPrimaryKey("dbo.Account");
            AlterColumn("dbo.RoleAssignment", "AccountId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Account", "AccountIdTmp", c => c.String(nullable: true, maxLength: 128));

            Sql(@"UPDATE dbo.Account 
                SET AccountIdTmp = AccountId
                FROM dbo.Account ");

            DropColumn("dbo.Account", "AccountId");
            RenameColumn("dbo.Account", "AccountIdTmp", "AccountId");
            AlterColumn("dbo.Account", "AccountId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Account", "AccountId");
            CreateIndex("dbo.RoleAssignment", "AccountId");
            AddForeignKey("dbo.RoleAssignment", "AccountId", "dbo.Account", "AccountId", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.RoleAssignment", "AccountId", "dbo.Account");
            DropIndex("dbo.RoleAssignment", new[] { "AccountId" });
            DropPrimaryKey("dbo.Account");

            AlterColumn("dbo.Account", "AccountId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.RoleAssignment", "AccountId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Account", "AccountId");
            CreateIndex("dbo.RoleAssignment", "AccountId");
            AddForeignKey("dbo.RoleAssignment", "AccountId", "dbo.Account", "AccountId", cascadeDelete: true);
        }
    }
}
