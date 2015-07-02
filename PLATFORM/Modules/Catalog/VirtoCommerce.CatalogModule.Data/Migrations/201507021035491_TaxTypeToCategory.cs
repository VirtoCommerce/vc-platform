namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxTypeToCategory : DbMigration
    {
        public override void Up()
        {
			DropForeignKey("dbo.Category", "Id", "dbo.CategoryBase");
			DropForeignKey("dbo.CategoryPropertyValue", "CategoryId", "dbo.Category");
			DropForeignKey("dbo.CategoryItemRelation", "CategoryId", "dbo.CategoryBase");
			DropForeignKey("dbo.CatalogImage", "CategoryId", "dbo.CategoryBase");
			DropIndex("dbo.CategoryBase", new[] { "CatalogId" });
			DropIndex("dbo.CategoryBase", new[] { "ParentCategoryId" });
			DropIndex("dbo.Category", new[] { "Id" });
			AddColumn("dbo.Category", "Code", c => c.String(nullable: false, maxLength: 64));
			AddColumn("dbo.Category", "IsActive", c => c.Boolean(nullable: false));
			AddColumn("dbo.Category", "Priority", c => c.Int(nullable: false));
			AddColumn("dbo.Category", "TaxType", c => c.String(maxLength: 64));
			AddColumn("dbo.Category", "CatalogId", c => c.String(nullable: false, maxLength: 128));
			AddColumn("dbo.Category", "ParentCategoryId", c => c.String(maxLength: 128));
			AddColumn("dbo.Category", "CreatedDate", c => c.DateTime(nullable: false));
			AddColumn("dbo.Category", "ModifiedDate", c => c.DateTime());
			AddColumn("dbo.Category", "CreatedBy", c => c.String(maxLength: 64));
			AddColumn("dbo.Category", "ModifiedBy", c => c.String(maxLength: 64));
			AddColumn("dbo.Category", "Discriminator", c => c.String(maxLength: 128));
			CreateIndex("dbo.Category", "CatalogId");
			CreateIndex("dbo.Category", "ParentCategoryId");
			AddForeignKey("dbo.CategoryPropertyValue", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
			AddForeignKey("dbo.CategoryItemRelation", "CategoryId", "dbo.Category", "Id", cascadeDelete: false);
			AddForeignKey("dbo.CatalogImage", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
			AddForeignKey("dbo.Category", "ParentCategoryId", "dbo.Category", "Id", cascadeDelete: false);
			DropColumn("dbo.Item", "TaxCategory");

			Sql("UPDATE dbo.Category SET Code = base.Code, IsActive = base.IsActive, Priority = base.Priority, CatalogId = base.CatalogId, ParentCategoryId = base.ParentCategoryId, CreatedDate = base.CreatedDate, ModifiedDate = base.ModifiedDate, CreatedBy = base.CreatedBy, ModifiedBy = base.ModifiedBy  FROM dbo.CategoryBase as base where base.Id = dbo.Category.Id ");
			
			DropTable("dbo.CategoryBase");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CategoryBase",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 64),
                        IsActive = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        ParentCategoryId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Item", "TaxCategory", c => c.String(maxLength: 128));
            DropForeignKey("dbo.CategoryPropertyValue", "CategoryId", "dbo.Category");
            DropIndex("dbo.Category", new[] { "ParentCategoryId" });
            DropIndex("dbo.Category", new[] { "CatalogId" });
            DropColumn("dbo.Category", "ModifiedBy");
            DropColumn("dbo.Category", "CreatedBy");
            DropColumn("dbo.Category", "ModifiedDate");
            DropColumn("dbo.Category", "CreatedDate");
            DropColumn("dbo.Category", "ParentCategoryId");
            DropColumn("dbo.Category", "CatalogId");
            DropColumn("dbo.Category", "TaxType");
            DropColumn("dbo.Category", "Priority");
            DropColumn("dbo.Category", "IsActive");
            DropColumn("dbo.Category", "Code");
            CreateIndex("dbo.Category", "Id");
            CreateIndex("dbo.CategoryBase", "ParentCategoryId");
            CreateIndex("dbo.CategoryBase", "CatalogId");
            AddForeignKey("dbo.CategoryPropertyValue", "CategoryId", "dbo.Category", "Id");
            AddForeignKey("dbo.Category", "Id", "dbo.CategoryBase", "Id");
        }
    }
}
