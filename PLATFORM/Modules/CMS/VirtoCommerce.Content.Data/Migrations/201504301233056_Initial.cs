namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ContentType = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        Name = c.String(),
                        Path = c.String(),
                        ByteContent = c.Binary(),
                        FileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContentTheme",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ThemePath = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContentMenuLinkList",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        StoreId = c.String(nullable: false),
                        Language = c.String(nullable: false),
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
                        Title = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        Type = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        MenuLinkListId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContentMenuLinkList", t => t.MenuLinkListId, cascadeDelete: true)
                .Index(t => t.MenuLinkListId);
            
            CreateTable(
                "dbo.ContentPage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        ByteContent = c.Binary(),
                        ContentType = c.String(nullable: false),
                        Path = c.String(nullable: false),
                        Language = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentMenuLink", "MenuLinkListId", "dbo.ContentMenuLinkList");
            DropIndex("dbo.ContentMenuLink", new[] { "MenuLinkListId" });
            DropTable("dbo.ContentPage");
            DropTable("dbo.ContentMenuLink");
            DropTable("dbo.ContentMenuLinkList");
            DropTable("dbo.ContentTheme");
            DropTable("dbo.ContentItem");
        }
    }
}
