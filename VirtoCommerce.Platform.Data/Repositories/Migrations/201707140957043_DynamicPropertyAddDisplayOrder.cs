namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicPropertyAddDisplayOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformDynamicProperty", "DisplayOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformDynamicProperty", "DisplayOrder");
        }
    }
}
