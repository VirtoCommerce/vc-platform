namespace VirtoCommerce.Content.Menu.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentMenuLink", "Title", c => c.String(nullable: false));
            AddColumn("dbo.ContentMenuLink", "Url", c => c.String(nullable: false));
            AddColumn("dbo.ContentMenuLink", "Type", c => c.String(nullable: false));
            AddColumn("dbo.ContentMenuLink", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContentMenuLink", "Language", c => c.String(nullable: false));
            AddColumn("dbo.ContentMenuLink", "Priority", c => c.Int(nullable: false));
            DropColumn("dbo.ContentMenuLink", "Name");
            DropColumn("dbo.ContentMenuLink", "Link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContentMenuLink", "Link", c => c.String(nullable: false));
            AddColumn("dbo.ContentMenuLink", "Name", c => c.String(nullable: false));
            DropColumn("dbo.ContentMenuLink", "Priority");
            DropColumn("dbo.ContentMenuLink", "Language");
            DropColumn("dbo.ContentMenuLink", "IsActive");
            DropColumn("dbo.ContentMenuLink", "Type");
            DropColumn("dbo.ContentMenuLink", "Url");
            DropColumn("dbo.ContentMenuLink", "Title");
        }
    }
}
