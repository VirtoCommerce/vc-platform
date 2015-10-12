namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountEntityEnumPropChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformAccount", "IsAdministrator", c => c.Boolean(nullable: false));
            AddColumn("dbo.PlatformAccount", "UserType", c => c.String(maxLength: 128));
            AlterColumn("dbo.PlatformAccount", "AccountState", c => c.String(maxLength: 128));
            Sql("UPDATE PlatformAccount SET PlatformAccount.UserType = 'Administrator' WHERE RegisterType = 2");
            Sql("UPDATE PlatformAccount SET PlatformAccount.UserType = 'Customer' WHERE RegisterType = 0");
            Sql("UPDATE PlatformAccount SET PlatformAccount.UserType = 'Manager' WHERE RegisterType = 3");
            Sql("UPDATE PlatformAccount SET PlatformAccount.AccountState = 'Approved'");
            Sql("UPDATE PlatformAccount SET PlatformAccount.IsAdministrator = 1 WHERE RegisterType = 2");
            DropColumn("dbo.PlatformAccount", "RegisterType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlatformAccount", "RegisterType", c => c.Int(nullable: false));
            AlterColumn("dbo.PlatformAccount", "AccountState", c => c.Int(nullable: false));
            DropColumn("dbo.PlatformAccount", "UserType");
            DropColumn("dbo.PlatformAccount", "IsAdministrator");
        }
    }
}
