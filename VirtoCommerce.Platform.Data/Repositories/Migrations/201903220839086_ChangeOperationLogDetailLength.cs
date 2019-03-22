namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeOperationLogDetailLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlatformOperationLog", "Detail", c => c.String(maxLength: 2048));
        }

        public override void Down()
        {
            AlterColumn("dbo.PlatformOperationLog", "Detail", c => c.String(maxLength: 1024));
        }
    }
}
