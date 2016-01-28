namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PlatformAccount", "UserName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlatformAccount", new[] { "UserName" });
        }
    }
}
