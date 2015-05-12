namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LinkedCategory", "Id", "dbo.CategoryBase");
            DropForeignKey("dbo.LinkedCategory", "LinkedCatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.LinkedCategory", "LinkedCategoryId", "dbo.CategoryBase");
            DropIndex("dbo.LinkedCategory", new[] { "Id" });
            DropIndex("dbo.LinkedCategory", new[] { "LinkedCatalogId" });
            DropIndex("dbo.LinkedCategory", new[] { "LinkedCategoryId" });
            CreateTable(
                "dbo.CategoryRelation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SourceCategoryId = c.String(nullable: false, maxLength: 128),
                        TargetCatalogId = c.String(maxLength: 128),
                        TargetCategoryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.SourceCategoryId)
                .ForeignKey("dbo.CatalogBase", t => t.TargetCatalogId)
                .ForeignKey("dbo.Category", t => t.TargetCategoryId)
                .Index(t => t.SourceCategoryId)
                .Index(t => t.TargetCatalogId)
                .Index(t => t.TargetCategoryId);
            
            DropTable("dbo.LinkedCategory");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LinkedCategory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LinkedCatalogId = c.String(nullable: false, maxLength: 128),
                        LinkedCategoryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CategoryRelation", "TargetCategoryId", "dbo.Category");
            DropForeignKey("dbo.CategoryRelation", "TargetCatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.CategoryRelation", "SourceCategoryId", "dbo.Category");
            DropIndex("dbo.CategoryRelation", new[] { "TargetCategoryId" });
            DropIndex("dbo.CategoryRelation", new[] { "TargetCatalogId" });
            DropIndex("dbo.CategoryRelation", new[] { "SourceCategoryId" });
            DropTable("dbo.CategoryRelation");
            CreateIndex("dbo.LinkedCategory", "LinkedCategoryId");
            CreateIndex("dbo.LinkedCategory", "LinkedCatalogId");
            CreateIndex("dbo.LinkedCategory", "Id");
            AddForeignKey("dbo.LinkedCategory", "LinkedCategoryId", "dbo.CategoryBase", "Id");
            AddForeignKey("dbo.LinkedCategory", "LinkedCatalogId", "dbo.CatalogBase", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LinkedCategory", "Id", "dbo.CategoryBase", "Id");
        }
    }
}
