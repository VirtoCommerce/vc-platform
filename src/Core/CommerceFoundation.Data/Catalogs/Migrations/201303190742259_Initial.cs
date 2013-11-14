namespace VirtoCommerce.Foundation.Data.Catalogs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryBase",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 64),
                        IsActive = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        ParentCategoryId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryBase", t => t.ParentCategoryId)
                .Index(t => t.CatalogId)
                .Index(t => t.ParentCategoryId);
            
            CreateTable(
                "dbo.PropertySet",
                c => new
                    {
                        PropertySetId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        TargetType = c.String(nullable: false, maxLength: 64),
                        CatalogId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PropertySetId)
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.CatalogBase",
                c => new
                    {
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        DefaultLanguage = c.String(nullable: false, maxLength: 64),
                        OwnerId = c.String(maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.CatalogId);
            
            CreateTable(
                "dbo.CatalogLanguage",
                c => new
                    {
                        CatalogLanguageId = c.String(nullable: false, maxLength: 128),
                        Language = c.String(maxLength: 64),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CatalogLanguageId)
                .ForeignKey("dbo.Catalog", t => t.CatalogId, cascadeDelete: true)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.PropertySetProperty",
                c => new
                    {
                        PropertySetPropertyId = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        PropertyId = c.String(nullable: false, maxLength: 128),
                        PropertySetId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PropertySetPropertyId)
                .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId, cascadeDelete: true)
                .Index(t => t.PropertyId)
                .Index(t => t.PropertySetId);
            
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        PropertyId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PropertyId)
                .ForeignKey("dbo.Property", t => t.ParentPropertyId)
                .ForeignKey("dbo.Catalog", t => t.CatalogId)
                .Index(t => t.ParentPropertyId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.PropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
                .ForeignKey("dbo.PropertyValue", t => t.ParentPropertyValueId)
                .Index(t => t.PropertyId)
                .Index(t => t.ParentPropertyValueId);
            
            CreateTable(
                "dbo.PropertyAttribute",
                c => new
                    {
                        PropertyAttributeId = c.String(nullable: false, maxLength: 128),
                        PropertyAttributeName = c.String(nullable: false, maxLength: 128),
                        PropertyAttributeValue = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        PropertyId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PropertyAttributeId)
                .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
            CreateTable(
                "dbo.CategoryPropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.CategoryItemRelation",
                c => new
                    {
                        CategoryItemRelationId = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CategoryItemRelationId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryBase", t => t.CategoryId)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.CategoryId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.ItemAsset",
                c => new
                    {
                        ItemAssetId = c.String(nullable: false, maxLength: 128),
                        AssetId = c.String(nullable: false, maxLength: 128),
                        AssetType = c.String(nullable: false, maxLength: 64),
                        GroupName = c.String(nullable: false, maxLength: 64),
                        SortOrder = c.Int(nullable: false),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ItemAssetId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.AssociationGroup",
                c => new
                    {
                        AssociationGroupId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Priority = c.Int(nullable: false),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AssociationGroupId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Association",
                c => new
                    {
                        AssociationId = c.String(nullable: false, maxLength: 128),
                        AssociationType = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        AssociationGroupId = c.String(nullable: false, maxLength: 128),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AssociationId)
                .ForeignKey("dbo.AssociationGroup", t => t.AssociationGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .Index(t => t.AssociationGroupId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.EditorialReview",
                c => new
                    {
                        EditorialReviewId = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Source = c.String(maxLength: 128),
                        Content = c.String(),
                        ReviewState = c.Int(nullable: false),
                        Comments = c.String(),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EditorialReviewId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemPropertyValue",
                c => new
                    {
                        PropertyValueId = c.String(nullable: false, maxLength: 128),
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.PropertyValueId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ItemId = c.String(nullable: false, maxLength: 128),
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
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId)
                .Index(t => t.PropertySetId)
				//.Index(t => new { t.Code, t.CatalogId }, true)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.ItemRelation",
                c => new
                    {
                        ItemRelationId = c.String(nullable: false, maxLength: 128),
                        RelationTypeId = c.String(maxLength: 64),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GroupName = c.String(nullable: false, maxLength: 64),
                        Priority = c.Int(nullable: false),
                        ChildItemId = c.String(nullable: false, maxLength: 128),
                        ParentItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ItemRelationId)
                .ForeignKey("dbo.Item", t => t.ChildItemId)
                .ForeignKey("dbo.Item", t => t.ParentItemId, cascadeDelete: true)
                .Index(t => t.ChildItemId)
                .Index(t => t.ParentItemId);
            
            CreateTable(
                "dbo.Price",
                c => new
                    {
                        PriceId = c.String(nullable: false, maxLength: 128),
                        Sale = c.Decimal(precision: 18, scale: 2),
                        List = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        ItemId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PriceId)
                .ForeignKey("dbo.Pricelist", t => t.PricelistId, cascadeDelete: true)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.PricelistId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Pricelist",
                c => new
                    {
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Currency = c.String(nullable: false, maxLength: 64),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PricelistId);
            
            CreateTable(
                "dbo.PricelistAssignment",
                c => new
                    {
                        PricelistAssignmentId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Priority = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        ConditionExpression = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PricelistAssignmentId)
                .ForeignKey("dbo.Pricelist", t => t.PricelistId, cascadeDelete: true)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId, cascadeDelete: true)
                .Index(t => t.PricelistId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.Packaging",
                c => new
                    {
                        PackageId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Description = c.String(nullable: false, maxLength: 512),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Depth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LengthMeasure = c.Int(nullable: false),
                        LastModified = c.DateTime(),
                        Created = c.DateTime(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PackageId);

	        CreateTable(
		        "dbo.TaxCategory",
		        c => new
			        {
				        TaxCategoryId = c.String(nullable: false, maxLength: 64),
				        Name = c.String(maxLength: 128),
				        LastModified = c.DateTime(),
				        Created = c.DateTime(),
				        Discriminator = c.String(maxLength: 128),
			        })
				//.Index(t => t.Name, true)
		        .PrimaryKey(t => t.TaxCategoryId);
				
				
            
            CreateTable(
                "dbo.VirtualCatalog",
                c => new
                    {
                        CatalogId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CatalogId)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.Catalog",
                c => new
                    {
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        WeightMeasure = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CatalogId)
                .ForeignKey("dbo.CatalogBase", t => t.CatalogId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        PropertySetId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.CategoryBase", t => t.CategoryId)
                .ForeignKey("dbo.PropertySet", t => t.PropertySetId)
                .Index(t => t.CategoryId)
                .Index(t => t.PropertySetId);
            
            CreateTable(
                "dbo.LinkedCategory",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        LinkedCatalogId = c.String(nullable: false, maxLength: 128),
                        LinkedCategoryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.CategoryBase", t => t.CategoryId)
                .ForeignKey("dbo.CatalogBase", t => t.LinkedCatalogId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryBase", t => t.LinkedCategoryId)
                .Index(t => t.CategoryId)
                .Index(t => t.LinkedCatalogId)
                .Index(t => t.LinkedCategoryId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.LinkedCategory", new[] { "LinkedCategoryId" });
            DropIndex("dbo.LinkedCategory", new[] { "LinkedCatalogId" });
            DropIndex("dbo.LinkedCategory", new[] { "CategoryId" });
            DropIndex("dbo.Category", new[] { "PropertySetId" });
            DropIndex("dbo.Category", new[] { "CategoryId" });
            DropIndex("dbo.Catalog", new[] { "CatalogId" });
            DropIndex("dbo.VirtualCatalog", new[] { "CatalogId" });
            DropIndex("dbo.PricelistAssignment", new[] { "CatalogId" });
            DropIndex("dbo.PricelistAssignment", new[] { "PricelistId" });
            DropIndex("dbo.Price", new[] { "ItemId" });
            DropIndex("dbo.Price", new[] { "PricelistId" });
            DropIndex("dbo.ItemRelation", new[] { "ParentItemId" });
            DropIndex("dbo.ItemRelation", new[] { "ChildItemId" });
            DropIndex("dbo.Item", new[] { "CatalogId" });
            DropIndex("dbo.Item", new[] { "PropertySetId" });
            DropIndex("dbo.ItemPropertyValue", new[] { "ItemId" });
            DropIndex("dbo.EditorialReview", new[] { "ItemId" });
            DropIndex("dbo.Association", new[] { "ItemId" });
            DropIndex("dbo.Association", new[] { "AssociationGroupId" });
            DropIndex("dbo.AssociationGroup", new[] { "ItemId" });
            DropIndex("dbo.ItemAsset", new[] { "ItemId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "CatalogId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "CategoryId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "ItemId" });
            DropIndex("dbo.CategoryPropertyValue", new[] { "CategoryId" });
            DropIndex("dbo.PropertyAttribute", new[] { "PropertyId" });
            DropIndex("dbo.PropertyValue", new[] { "ParentPropertyValueId" });
            DropIndex("dbo.PropertyValue", new[] { "PropertyId" });
            DropIndex("dbo.Property", new[] { "CatalogId" });
            DropIndex("dbo.Property", new[] { "ParentPropertyId" });
            DropIndex("dbo.PropertySetProperty", new[] { "PropertySetId" });
            DropIndex("dbo.PropertySetProperty", new[] { "PropertyId" });
            DropIndex("dbo.CatalogLanguage", new[] { "CatalogId" });
            DropIndex("dbo.PropertySet", new[] { "CatalogId" });
            DropIndex("dbo.CategoryBase", new[] { "ParentCategoryId" });
            DropIndex("dbo.CategoryBase", new[] { "CatalogId" });
            DropForeignKey("dbo.LinkedCategory", "LinkedCategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.LinkedCategory", "LinkedCatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.LinkedCategory", "CategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Category", "CategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.Catalog", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.VirtualCatalog", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.PricelistAssignment", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.PricelistAssignment", "PricelistId", "dbo.Pricelist");
            DropForeignKey("dbo.Price", "ItemId", "dbo.Item");
            DropForeignKey("dbo.Price", "PricelistId", "dbo.Pricelist");
            DropForeignKey("dbo.ItemRelation", "ParentItemId", "dbo.Item");
            DropForeignKey("dbo.ItemRelation", "ChildItemId", "dbo.Item");
            DropForeignKey("dbo.Item", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.ItemPropertyValue", "ItemId", "dbo.Item");
            DropForeignKey("dbo.EditorialReview", "ItemId", "dbo.Item");
            DropForeignKey("dbo.Association", "ItemId", "dbo.Item");
            DropForeignKey("dbo.Association", "AssociationGroupId", "dbo.AssociationGroup");
            DropForeignKey("dbo.AssociationGroup", "ItemId", "dbo.Item");
            DropForeignKey("dbo.ItemAsset", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CategoryItemRelation", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.CategoryItemRelation", "CategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.CategoryItemRelation", "ItemId", "dbo.Item");
            DropForeignKey("dbo.CategoryPropertyValue", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.PropertyAttribute", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.PropertyValue", "ParentPropertyValueId", "dbo.PropertyValue");
            DropForeignKey("dbo.PropertyValue", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.Property", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.Property", "ParentPropertyId", "dbo.Property");
            DropForeignKey("dbo.PropertySetProperty", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.PropertySetProperty", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.CatalogLanguage", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.PropertySet", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.CategoryBase", "ParentCategoryId", "dbo.CategoryBase");
            DropForeignKey("dbo.CategoryBase", "CatalogId", "dbo.CatalogBase");
            DropTable("dbo.LinkedCategory");
            DropTable("dbo.Category");
            DropTable("dbo.Catalog");
            DropTable("dbo.VirtualCatalog");
            DropTable("dbo.TaxCategory");
            DropTable("dbo.Packaging");
            DropTable("dbo.PricelistAssignment");
            DropTable("dbo.Pricelist");
            DropTable("dbo.Price");
            DropTable("dbo.ItemRelation");
            DropTable("dbo.Item");
            DropTable("dbo.ItemPropertyValue");
            DropTable("dbo.EditorialReview");
            DropTable("dbo.Association");
            DropTable("dbo.AssociationGroup");
            DropTable("dbo.ItemAsset");
            DropTable("dbo.CategoryItemRelation");
            DropTable("dbo.CategoryPropertyValue");
            DropTable("dbo.PropertyAttribute");
            DropTable("dbo.PropertyValue");
            DropTable("dbo.Property");
            DropTable("dbo.PropertySetProperty");
            DropTable("dbo.CatalogLanguage");
            DropTable("dbo.CatalogBase");
            DropTable("dbo.PropertySet");
            DropTable("dbo.CategoryBase");
        }
    }
}
