namespace VirtoCommerce.Content.Menu.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentMenuLinkList",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        StoreId = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContentMenuLink",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Link = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        MenuLinkListId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContentMenuLinkList", t => t.MenuLinkListId)
                .Index(t => t.MenuLinkListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentMenuLink", "MenuLinkListId", "dbo.ContentMenuLinkList");
            DropIndex("dbo.ContentMenuLink", new[] { "MenuLinkListId" });
            DropTable("dbo.ContentMenuLink");
            DropTable("dbo.ContentMenuLinkList");
        }
    }
}
