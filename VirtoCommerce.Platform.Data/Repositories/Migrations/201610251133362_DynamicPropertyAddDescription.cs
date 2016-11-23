namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicPropertyAddDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlatformDynamicProperty", "Description", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlatformDynamicProperty", "Description");
        }
    }
}
