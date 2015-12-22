namespace VirtoCommerce.Content.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePagesAndThemes : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ContentItem");
            DropTable("dbo.ContentTheme");
            DropTable("dbo.ContentPage");
        }
        
        public override void Down()
        {
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
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContentTheme",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ThemePath = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContentItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ContentType = c.String(),
                        Name = c.String(),
                        Path = c.String(),
                        ByteContent = c.Binary(),
                        FileUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
