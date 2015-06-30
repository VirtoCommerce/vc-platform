namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemAsset", "ItemId", "dbo.Item");
            DropIndex("dbo.ItemAsset", new[] { "ItemId" });
            CreateTable(
                "dbo.CatalogImage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Url = c.String(nullable: false, maxLength: 2083),
                        Name = c.String(maxLength: 1024),
                        LanguageCode = c.String(maxLength: 5),
                        Group = c.String(maxLength: 64),
                        SortOrder = c.Int(nullable: false),
                        ItemId = c.String(maxLength: 128),
                        CategoryId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.CategoryBase", t => t.CategoryId)
                .Index(t => t.ItemId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.CatalogAsset",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Url = c.String(nullable: false, maxLength: 2083),
                        Name = c.String(maxLength: 1024),
                        MimeType = c.String(maxLength: 128),
                        Size = c.Int(nullable: false),
                        LanguageCode = c.String(maxLength: 5),
                        ItemId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .Index(t => t.ItemId);

			Sql("INSERT INTO dbo.CatalogImage  ([Id], [Url],  [Group], [SortOrder], [ItemId], [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy]) SELECT ia.Id, ia.AssetId, ia.GroupName, ia.SortOrder, ia.ItemId, ia.CreatedDate, ia.ModifiedDate, ia.CreatedBy, ia.ModifiedBy FROM dbo.ItemAsset AS ia");
            DropTable("dbo.ItemAsset");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ItemAsset",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AssetId = c.String(nullable: false, maxLength: 2083),
                        AssetType = c.String(nullable: false, maxLength: 64),
                        GroupName = c.String(nullable: false, maxLength: 64),
                        SortOrder = c.Int(nullable: false),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CatalogImage", "CategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.CatalogImage", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CatalogAsset", "ItemId", "dbo.Item");
            DropIndex("dbo.CatalogAsset", new[] { "ItemId" });
            DropIndex("dbo.CatalogImage", new[] { "CategoryId" });
            DropIndex("dbo.CatalogImage", new[] { "ItemId" });
            DropTable("dbo.CatalogAsset");
            DropTable("dbo.CatalogImage");
            CreateIndex("dbo.ItemAsset", "ItemId");
            AddForeignKey("dbo.ItemAsset", "ItemId", "dbo.Item", "Id", cascadeDelete: true);
        }
    }
}
