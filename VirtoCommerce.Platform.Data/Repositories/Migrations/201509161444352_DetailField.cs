namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DetailField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformOperationLog", "Detail", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformOperationLog", "Detail");
        }
    }
}
