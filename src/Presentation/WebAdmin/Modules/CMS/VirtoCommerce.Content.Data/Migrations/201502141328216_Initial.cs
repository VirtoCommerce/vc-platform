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
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        Name = c.String(),
                        ParentContentItemId = c.String(maxLength: 128),
                        Path = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContentItem", t => t.ParentContentItemId)
                .Index(t => t.ParentContentItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentItem", "ParentContentItemId", "dbo.ContentItem");
            DropIndex("dbo.ContentItem", new[] { "ParentContentItemId" });
            DropTable("dbo.ContentItem");
        }
    }
}
