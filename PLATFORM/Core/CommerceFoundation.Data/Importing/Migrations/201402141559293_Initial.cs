using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Importing.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MappingItem",
                c => new
                    {
                        MappingItemId = c.String(nullable: false, maxLength: 128),
                        EntityColumnName = c.String(maxLength: 128),
                        CsvColumnName = c.String(maxLength: 256),
                        IsSystemProperty = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        CustomValue = c.String(),
                        StringFormat = c.String(),
                        DisplayName = c.String(maxLength: 256),
                        Locale = c.String(maxLength: 128),
                        ImportJobId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MappingItemId)
                .ForeignKey("dbo.ImportJob", t => t.ImportJobId, cascadeDelete: true)
                .Index(t => t.ImportJobId);
            
            CreateTable(
                "dbo.ImportJob",
                c => new
                    {
                        ImportJobId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        CatalogId = c.String(maxLength: 128),
                        TemplatePath = c.String(maxLength: 256),
                        MaxErrorsCount = c.Int(nullable: false),
                        ImportStep = c.Int(nullable: false),
                        ImportCount = c.Int(nullable: false),
                        StartIndex = c.Int(nullable: false),
                        ColumnDelimiter = c.String(maxLength: 8),
                        EntityImporter = c.String(nullable: false, maxLength: 64),
                        PropertySetId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ImportJobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MappingItem", "ImportJobId", "dbo.ImportJob");
            DropIndex("dbo.MappingItem", new[] { "ImportJobId" });
            DropTable("dbo.ImportJob");
            DropTable("dbo.MappingItem");
        }
    }
}
