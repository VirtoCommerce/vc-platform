namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Content
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicContentFolders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DynamicContentFolder",
                c => new
                    {
                        DynamicContentFolderId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        ImageUrl = c.String(maxLength: 2048),
                        ParentFolderId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DynamicContentFolderId)
                .ForeignKey("dbo.DynamicContentFolder", t => t.ParentFolderId)
                .Index(t => t.ParentFolderId);
            
            AddColumn("dbo.DynamicContentItem", "FolderId", c => c.String(maxLength: 128));
            AddColumn("dbo.DynamicContentItem", "ImageUrl", c => c.String(maxLength: 2048));
            AddColumn("dbo.DynamicContentPlace", "ImageUrl", c => c.String(maxLength: 2048));
            AddColumn("dbo.DynamicContentPlace", "FolderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DynamicContentItem", "FolderId");
            CreateIndex("dbo.DynamicContentPlace", "FolderId");
            AddForeignKey("dbo.DynamicContentPlace", "FolderId", "dbo.DynamicContentFolder", "DynamicContentFolderId");
            AddForeignKey("dbo.DynamicContentItem", "FolderId", "dbo.DynamicContentFolder", "DynamicContentFolderId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DynamicContentItem", "FolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.DynamicContentFolder", "ParentFolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.DynamicContentPlace", "FolderId", "dbo.DynamicContentFolder");
            DropIndex("dbo.DynamicContentItemProperty", new[] { "DynamicContentItemId" });
            DropIndex("dbo.DynamicContentPlace", new[] { "FolderId" });
            DropIndex("dbo.DynamicContentFolder", new[] { "ParentFolderId" });
            DropIndex("dbo.DynamicContentItem", new[] { "FolderId" });
            DropColumn("dbo.DynamicContentPlace", "FolderId");
            DropColumn("dbo.DynamicContentPlace", "ImageUrl");
            DropColumn("dbo.DynamicContentItem", "ImageUrl");
            DropColumn("dbo.DynamicContentItem", "FolderId");
            DropTable("dbo.DynamicContentFolder");
        }
    }
}
