namespace VirtoCommerce.Content.Pages.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentPage", "Language", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentPage", "Language");
        }
    }
}
