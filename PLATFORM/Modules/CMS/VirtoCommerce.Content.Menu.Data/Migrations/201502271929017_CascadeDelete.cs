namespace VirtoCommerce.Content.Menu.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContentMenuLink", "MenuLinkListId", "dbo.ContentMenuLinkList");
            AddForeignKey("dbo.ContentMenuLink", "MenuLinkListId", "dbo.ContentMenuLinkList", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentMenuLink", "MenuLinkListId", "dbo.ContentMenuLinkList");
            AddForeignKey("dbo.ContentMenuLink", "MenuLinkListId", "dbo.ContentMenuLinkList", "Id");
        }
    }
}
