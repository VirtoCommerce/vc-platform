namespace VirtoCommerce.Content.Menu.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddListPropertyLanguageRemoveLinkProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentMenuLinkList", "Language", c => c.String(nullable: false));
            AlterColumn("dbo.ContentMenuLink", "Type", c => c.String());
            DropColumn("dbo.ContentMenuLink", "Language");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContentMenuLink", "Language", c => c.String(nullable: false));
            AlterColumn("dbo.ContentMenuLink", "Type", c => c.String(nullable: false));
            DropColumn("dbo.ContentMenuLinkList", "Language");
        }
    }
}
