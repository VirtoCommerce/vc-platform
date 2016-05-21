namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ApiAccountName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformApiAccount", "Name", c => c.String(maxLength: 128));
            Sql("UPDATE dbo.PlatformApiAccount SET ApiAccountType = 1 WHERE ApiAccountType = 0");
            Sql("UPDATE dbo.PlatformApiAccount SET Name = 'No Name' WHERE Name IS NULL");
        }

        public override void Down()
        {
            DropColumn("dbo.PlatformApiAccount", "Name");
        }
    }
}
