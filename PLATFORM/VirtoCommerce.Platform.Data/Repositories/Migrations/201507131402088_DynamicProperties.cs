namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicProperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlatformDynamicProperty",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        ObjectType = c.String(maxLength: 256),
                        Name = c.String(maxLength: 256),
                        ValueType = c.String(nullable: false, maxLength: 64),
                        IsArray = c.Boolean(nullable: false),
                        IsDictionary = c.Boolean(nullable: false),
                        IsMultilingual = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ObjectType, t.Name }, unique: true, name: "IX_PlatformDynamicProperty_ObjectType_Name");
            
            CreateTable(
                "dbo.PlatformDynamicPropertyDictionaryItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        PropertyId = c.String(nullable: false, maxLength: 64),
                        Name = c.String(maxLength: 512),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformDynamicProperty", t => t.PropertyId, cascadeDelete: true)
                .Index(t => new { t.PropertyId, t.Name }, unique: true, name: "IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name");
            
            CreateTable(
                "dbo.PlatformDynamicPropertyDictionaryItemName",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        DictionaryItemId = c.String(nullable: false, maxLength: 64),
                        Locale = c.String(maxLength: 64),
                        Name = c.String(maxLength: 512),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformDynamicPropertyDictionaryItem", t => t.DictionaryItemId, cascadeDelete: true)
                .Index(t => new { t.DictionaryItemId, t.Locale, t.Name }, unique: true, name: "IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name");
            
            CreateTable(
                "dbo.PlatformDynamicPropertyObjectValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        ObjectType = c.String(maxLength: 256),
                        ObjectId = c.String(maxLength: 128),
                        Locale = c.String(maxLength: 64),
                        ValueType = c.String(nullable: false, maxLength: 64),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(precision: 18, scale: 2),
                        IntegerValue = c.Int(),
                        BooleanValue = c.Boolean(),
                        DateTimeValue = c.DateTime(),
                        PropertyId = c.String(nullable: false, maxLength: 64),
                        DictionaryItemId = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformDynamicPropertyDictionaryItem", t => t.DictionaryItemId)
                .ForeignKey("dbo.PlatformDynamicProperty", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId)
                .Index(t => t.DictionaryItemId);
            
            CreateTable(
                "dbo.PlatformDynamicPropertyName",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 64),
                        PropertyId = c.String(nullable: false, maxLength: 64),
                        Locale = c.String(maxLength: 64),
                        Name = c.String(maxLength: 256),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformDynamicProperty", t => t.PropertyId, cascadeDelete: true)
                .Index(t => new { t.PropertyId, t.Locale, t.Name }, unique: true, name: "IX_PlatformDynamicPropertyName_PropertyId_Locale_Name");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlatformDynamicPropertyName", "PropertyId", "dbo.PlatformDynamicProperty");
            DropForeignKey("dbo.PlatformDynamicPropertyDictionaryItem", "PropertyId", "dbo.PlatformDynamicProperty");
            DropForeignKey("dbo.PlatformDynamicPropertyObjectValue", "PropertyId", "dbo.PlatformDynamicProperty");
            DropForeignKey("dbo.PlatformDynamicPropertyObjectValue", "DictionaryItemId", "dbo.PlatformDynamicPropertyDictionaryItem");
            DropForeignKey("dbo.PlatformDynamicPropertyDictionaryItemName", "DictionaryItemId", "dbo.PlatformDynamicPropertyDictionaryItem");
            DropIndex("dbo.PlatformDynamicPropertyName", "IX_PlatformDynamicPropertyName_PropertyId_Locale_Name");
            DropIndex("dbo.PlatformDynamicPropertyObjectValue", new[] { "DictionaryItemId" });
            DropIndex("dbo.PlatformDynamicPropertyObjectValue", new[] { "PropertyId" });
            DropIndex("dbo.PlatformDynamicPropertyDictionaryItemName", "IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name");
            DropIndex("dbo.PlatformDynamicPropertyDictionaryItem", "IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name");
            DropIndex("dbo.PlatformDynamicProperty", "IX_PlatformDynamicProperty_ObjectType_Name");
            DropTable("dbo.PlatformDynamicPropertyName");
            DropTable("dbo.PlatformDynamicPropertyObjectValue");
            DropTable("dbo.PlatformDynamicPropertyDictionaryItemName");
            DropTable("dbo.PlatformDynamicPropertyDictionaryItem");
            DropTable("dbo.PlatformDynamicProperty");
        }
    }
}
