namespace VirtoCommerce.Foundation.Data.Importing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
                        CsvColumnName = c.String(maxLength: 128),
                        SystemProperty = c.Boolean(nullable: false),
                        CustomValue = c.String(maxLength: 128),
                        Locale = c.String(maxLength: 128),
                        ColumnMappingId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MappingItemId)
                .ForeignKey("dbo.ColumnMapping", t => t.ColumnMappingId, cascadeDelete: true)
                .Index(t => t.ColumnMappingId);
            
            CreateTable(
                "dbo.ColumnMapping",
                c => new
                    {
                        ColumnMappingId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        EntityImporter = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ColumnMappingId);
            
            CreateTable(
                "dbo.ImportJob",
                c => new
                    {
                        ImportJobId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        EntityImporter = c.String(nullable: false, maxLength: 64),
                        ContainerId = c.String(maxLength: 128),
                        TemplateId = c.String(maxLength: 256),
                        MaxErrorsCount = c.Int(nullable: false),
                        ColumnDelimiter = c.String(maxLength: 8),
                        ImportAction = c.Int(nullable: false),
                        ColumnMappingId = c.String(maxLength: 128),
                        PropertySetId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ImportJobId)
                .ForeignKey("dbo.ColumnMapping", t => t.ColumnMappingId)
                .Index(t => t.ColumnMappingId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ImportJob", new[] { "ColumnMappingId" });
            DropIndex("dbo.MappingItem", new[] { "ColumnMappingId" });
            DropForeignKey("dbo.ImportJob", "ColumnMappingId", "dbo.ColumnMapping");
            DropForeignKey("dbo.MappingItem", "ColumnMappingId", "dbo.ColumnMapping");
            DropTable("dbo.ImportJob");
            DropTable("dbo.ColumnMapping");
            DropTable("dbo.MappingItem");
        }
    }
}
