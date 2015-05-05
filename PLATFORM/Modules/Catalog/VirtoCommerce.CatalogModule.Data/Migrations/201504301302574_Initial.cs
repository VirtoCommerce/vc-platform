namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CatalogBase",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        DefaultLanguage = c.String(nullable: false, maxLength: 64),
                        OwnerId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryBase", t => t.ParentCategoryId)
                .Index(t => t.CatalogId)
                .Index(t => t.ParentCategoryId);
            
            CreateTable(
                "dbo.CategoryPropertyValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
                        KeyValue = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.PropertySet",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        TargetType = c.String(nullable: false, maxLength: 64),
                        CatalogId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Catalog_Id = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalog", t => t.Catalog_Id)
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .Index(t => t.CatalogId)
                .Index(t => t.Catalog_Id);
            
            CreateTable(
                "dbo.CatalogLanguage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Language = c.String(maxLength: 64),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.CatalogPropertyValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
                        KeyValue = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.PropertySetProperty",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        PropertyId = c.String(nullable: false, maxLength: 128),
                        PropertySetId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId, cascadeDelete: true)
                .Index(t => t.PropertyId)
                .Index(t => t.PropertySetId);
            
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        TargetType = c.String(maxLength: 128),
                        IsKey = c.Boolean(nullable: false),
                        IsSale = c.Boolean(nullable: false),
                        IsEnum = c.Boolean(nullable: false),
                        IsInput = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        IsMultiValue = c.Boolean(nullable: false),
                        IsLocaleDependant = c.Boolean(nullable: false),
                        AllowAlias = c.Boolean(nullable: false),
                        PropertyValueType = c.Int(nullable: false),
                        ParentPropertyId = c.String(maxLength: 128),
                        CatalogId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .ForeignKey("dbo.Property", t => t.ParentPropertyId)
                .Index(t => t.ParentPropertyId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.PropertyAttribute",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PropertyAttributeName = c.String(nullable: false, maxLength: 128),
                        PropertyAttributeValue = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        PropertyId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
            CreateTable(
                "dbo.PropertyValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PropertyId = c.String(nullable: false, maxLength: 128),
                        ParentPropertyValueId = c.String(maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
                        KeyValue = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PropertyValue", t => t.ParentPropertyValueId)
                .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId)
                .Index(t => t.ParentPropertyValueId);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 1024),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsBuyable = c.Boolean(nullable: false),
                        AvailabilityRule = c.Int(nullable: false),
                        MinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrackInventory = c.Boolean(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackageType = c.String(maxLength: 128),
                        TaxCategory = c.String(maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 64),
                        PropertySetId = c.String(maxLength: 128),
                        CatalogId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId)
                .Index(t => t.PropertySetId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.AssociationGroup",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Priority = c.Int(nullable: false),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Association",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AssociationType = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        AssociationGroupId = c.String(nullable: false, maxLength: 128),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.AssociationGroup", t => t.AssociationGroupId, cascadeDelete: true)
                .Index(t => t.AssociationGroupId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.CategoryItemRelation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryBase", t => t.CategoryId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.CategoryId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.EditorialReview",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Source = c.String(maxLength: 128),
                        Content = c.String(),
                        ReviewState = c.Int(nullable: false),
                        Comments = c.String(),
                        Locale = c.String(maxLength: 64),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemAsset",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AssetId = c.String(nullable: false, maxLength: 128),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemPropertyValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
                        KeyValue = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemRelation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RelationTypeId = c.String(maxLength: 64),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GroupName = c.String(nullable: false, maxLength: 64),
                        Priority = c.Int(nullable: false),
                        ChildItemId = c.String(nullable: false, maxLength: 128),
                        ParentItemId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ChildItemId)
                .ForeignKey("dbo.Item", t => t.ParentItemId, cascadeDelete: true)
                .Index(t => t.ChildItemId)
                .Index(t => t.ParentItemId);
            
            CreateTable(
                "dbo.VirtualCatalog",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CatalogBase", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Catalog",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WeightMeasure = c.Int(nullable: false),
                        PropertySetId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CatalogBase", t => t.Id)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId)
                .Index(t => t.Id)
                .Index(t => t.PropertySetId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        PropertySetId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryBase", t => t.Id)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId)
                .Index(t => t.Id)
                .Index(t => t.PropertySetId);
            
            CreateTable(
                "dbo.LinkedCategory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LinkedCatalogId = c.String(nullable: false, maxLength: 128),
                        LinkedCategoryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryBase", t => t.Id)
                .ForeignKey("dbo.CatalogBase", t => t.LinkedCatalogId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryBase", t => t.LinkedCategoryId)
                .Index(t => t.Id)
                .Index(t => t.LinkedCatalogId)
                .Index(t => t.LinkedCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LinkedCategory", "LinkedCategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.LinkedCategory", "LinkedCatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.LinkedCategory", "Id", "dbo.CategoryBase");
            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Category", "Id", "dbo.CategoryBase");
            DropForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Catalog", "Id", "dbo.CatalogBase");
            DropForeignKey("dbo.VirtualCatalog", "Id", "dbo.CatalogBase");
            DropForeignKey("dbo.ItemRelation", "ParentItemId", "dbo.Item");
            DropForeignKey("dbo.ItemRelation", "ChildItemId", "dbo.Item");
            DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.ItemPropertyValue", "ItemId", "dbo.Item");
            DropForeignKey("dbo.ItemAsset", "ItemId", "dbo.Item");
            DropForeignKey("dbo.EditorialReview", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CategoryItemRelation", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CategoryItemRelation", "CategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.CategoryItemRelation", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.Item", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.AssociationGroup", "ItemId", "dbo.Item");
            DropForeignKey("dbo.Association", "AssociationGroupId", "dbo.AssociationGroup");
            DropForeignKey("dbo.Association", "ItemId", "dbo.Item");
            DropForeignKey("dbo.PropertySetProperty", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.PropertySetProperty", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.PropertyValue", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.PropertyValue", "ParentPropertyValueId", "dbo.PropertyValue");
            DropForeignKey("dbo.PropertyAttribute", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.Property", "ParentPropertyId", "dbo.Property");
            DropForeignKey("dbo.Property", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.PropertySet", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.PropertySet", "Catalog_Id", "dbo.Catalog");
            DropForeignKey("dbo.CatalogPropertyValue", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.CatalogLanguage", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.CategoryPropertyValue", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.CategoryBase", "ParentCategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.CategoryBase", "CatalogId", "dbo.CatalogBase");
            DropIndex("dbo.LinkedCategory", new[] { "LinkedCategoryId" });
            DropIndex("dbo.LinkedCategory", new[] { "LinkedCatalogId" });
            DropIndex("dbo.LinkedCategory", new[] { "Id" });
            DropIndex("dbo.Category", new[] { "PropertySetId" });
            DropIndex("dbo.Category", new[] { "Id" });
            DropIndex("dbo.Catalog", new[] { "PropertySetId" });
            DropIndex("dbo.Catalog", new[] { "Id" });
            DropIndex("dbo.VirtualCatalog", new[] { "Id" });
            DropIndex("dbo.ItemRelation", new[] { "ParentItemId" });
            DropIndex("dbo.ItemRelation", new[] { "ChildItemId" });
            DropIndex("dbo.ItemPropertyValue", new[] { "ItemId" });
            DropIndex("dbo.ItemAsset", new[] { "ItemId" });
            DropIndex("dbo.EditorialReview", new[] { "ItemId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "CatalogId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "CategoryId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "ItemId" });
            DropIndex("dbo.Association", new[] { "ItemId" });
            DropIndex("dbo.Association", new[] { "AssociationGroupId" });
            DropIndex("dbo.AssociationGroup", new[] { "ItemId" });
            DropIndex("dbo.Item", new[] { "CatalogId" });
            DropIndex("dbo.Item", new[] { "PropertySetId" });
            DropIndex("dbo.PropertyValue", new[] { "ParentPropertyValueId" });
            DropIndex("dbo.PropertyValue", new[] { "PropertyId" });
            DropIndex("dbo.PropertyAttribute", new[] { "PropertyId" });
            DropIndex("dbo.Property", new[] { "CatalogId" });
            DropIndex("dbo.Property", new[] { "ParentPropertyId" });
            DropIndex("dbo.PropertySetProperty", new[] { "PropertySetId" });
            DropIndex("dbo.PropertySetProperty", new[] { "PropertyId" });
            DropIndex("dbo.CatalogPropertyValue", new[] { "CatalogId" });
            DropIndex("dbo.CatalogLanguage", new[] { "CatalogId" });
            DropIndex("dbo.PropertySet", new[] { "Catalog_Id" });
            DropIndex("dbo.PropertySet", new[] { "CatalogId" });
            DropIndex("dbo.CategoryPropertyValue", new[] { "CategoryId" });
            DropIndex("dbo.CategoryBase", new[] { "ParentCategoryId" });
            DropIndex("dbo.CategoryBase", new[] { "CatalogId" });
            DropTable("dbo.LinkedCategory");
            DropTable("dbo.Category");
            DropTable("dbo.Catalog");
            DropTable("dbo.VirtualCatalog");
            DropTable("dbo.ItemRelation");
            DropTable("dbo.ItemPropertyValue");
            DropTable("dbo.ItemAsset");
            DropTable("dbo.EditorialReview");
            DropTable("dbo.CategoryItemRelation");
            DropTable("dbo.Association");
            DropTable("dbo.AssociationGroup");
            DropTable("dbo.Item");
            DropTable("dbo.PropertyValue");
            DropTable("dbo.PropertyAttribute");
            DropTable("dbo.Property");
            DropTable("dbo.PropertySetProperty");
            DropTable("dbo.CatalogPropertyValue");
            DropTable("dbo.CatalogLanguage");
            DropTable("dbo.PropertySet");
            DropTable("dbo.CategoryPropertyValue");
            DropTable("dbo.CategoryBase");
            DropTable("dbo.CatalogBase");
        }
    }
}
