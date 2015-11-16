namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyShemaChanges : DbMigration
    {
        public override void Up()
        {
            //Drop Catalog and VirtualCatalog tables rename CatalogBase to Catalog
            DropForeignKey("dbo.CategoryItemRelation", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.CategoryRelation", "TargetCatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.Item", "CatalogId", "dbo.CatalogBase");
            DropForeignKey("dbo.CatalogLanguage", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.Catalog", "PropertySetId", "dbo.PropertySet");
            DropForeignKey("dbo.Catalog", "Id", "dbo.CatalogBase");
            DropForeignKey("dbo.Property", "CatalogId", "dbo.Catalog");
            DropForeignKey("dbo.CatalogPropertyValue", "CatalogId", "dbo.Catalog");

            DropIndex("dbo.Catalog", new[] { "PropertySetId" });
            DropIndex("dbo.Property", new[] { "CatalogId" });
            DropIndex("dbo.CategoryItemRelation", new[] { "CatalogId" });
            DropIndex("dbo.CategoryRelation", new[] { "TargetCatalogId" });
            DropIndex("dbo.Item", new[] { "CatalogId" });
            DropIndex("dbo.CatalogLanguage", new[] { "CatalogId" });

            RenameTable(name: "dbo.Catalog", newName: "TmpCatalog");
            RenameTable(name: "dbo.CatalogBase", newName: "Catalog");

            AddColumn("dbo.Catalog", "Virtual", c => c.Boolean(nullable: false));
            Sql(@"UPDATE dbo.Catalog SET Virtual = 1 FROM 
                  VirtualCatalog  WHERE dbo.Catalog.Id = VirtualCatalog.Id");
          

            AddForeignKey("dbo.CategoryItemRelation", "CatalogId", "dbo.Catalog", "Id");
            AddForeignKey("dbo.CategoryRelation", "TargetCatalogId", "dbo.Catalog", "Id");
            AddForeignKey("dbo.Item", "CatalogId", "dbo.Catalog", "Id");
            AddForeignKey("dbo.CatalogLanguage", "CatalogId", "dbo.Catalog", "Id");
            AddForeignKey("dbo.Property", "CatalogId", "dbo.Catalog", "Id");

            CreateIndex("dbo.CategoryItemRelation", "CatalogId");
            CreateIndex("dbo.CategoryRelation", "TargetCatalogId");
            CreateIndex("dbo.Item", "CatalogId");
            CreateIndex("dbo.CatalogLanguage", "CatalogId");
            CreateIndex("dbo.Property", "CatalogId");

            //Create PropertyDictionaryValue and copy all data from PropertyValue
            CreateTable(
              "dbo.PropertyDictionaryValue",
              c => new
              {
                  Id = c.String(nullable: false, maxLength: 128),
                  Alias = c.String(maxLength: 64),
                  Name = c.String(maxLength: 64),
                  Value = c.String(maxLength: 512),
                  Locale = c.String(maxLength: 64),
                  PropertyId = c.String(nullable: false, maxLength: 128),
              })
              .PrimaryKey(t => t.Id)
              .ForeignKey("dbo.Property", t => t.PropertyId, cascadeDelete: true)
              .Index(t => t.PropertyId);
            Sql(@"INSERT INTO dbo.PropertyDictionaryValue (Id, Alias, Name, Value, Locale, PropertyId) SELECT Id, Alias, Alias, ShortTextValue, Locale, PropertyId FROM dbo.PropertyValue");

            //Change PropertyValue and copy all data from ItemPropertyValues, CategoryPropertyValues, CatalogPropertyValues
            Sql("DELETE FROM  dbo.PropertyValue");
            DropForeignKey("dbo.PropertyValue", "PropertyId", "dbo.Property");
            DropIndex("dbo.PropertyValue", new[] { "PropertyId" });
            DropColumn("dbo.PropertyValue", "PropertyId");

            DropForeignKey("dbo.PropertyValue", "ParentPropertyValueId", "dbo.PropertyValue");
            DropIndex("dbo.PropertyValue", new[] { "ParentPropertyValueId" });
            DropColumn("dbo.PropertyValue", "ParentPropertyValueId");

            AddColumn("dbo.PropertyValue", "ItemId", c => c.String(maxLength: 128));
            AddColumn("dbo.PropertyValue", "CatalogId", c => c.String(maxLength: 128));
            AddColumn("dbo.PropertyValue", "CategoryId", c => c.String(maxLength: 128));
            CreateIndex("dbo.PropertyValue", "ItemId");
            CreateIndex("dbo.PropertyValue", "CatalogId");
            CreateIndex("dbo.PropertyValue", "CategoryId");
            AddForeignKey("dbo.PropertyValue", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.PropertyValue", "CategoryId", "dbo.Category", "Id");
            AddForeignKey("dbo.PropertyValue", "CatalogId", "dbo.Catalog", "Id");
            Sql(@"INSERT INTO dbo.PropertyValue ([Id], [Alias], [Name], [KeyValue], [ValueType], [ShortTextValue], [LongTextValue],
                                                 [DecimalValue], [IntegerValue], [BooleanValue], [DateTimeValue], [Locale],
                                                 [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [CatalogId]) 
                                                 SELECT [Id], [Alias], [Name], [KeyValue], [ValueType], [ShortTextValue], [LongTextValue],
                                                 [DecimalValue], [IntegerValue], [BooleanValue], [DateTimeValue], [Locale],
                                                 [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [CatalogId] FROM dbo.CatalogPropertyValue");
            Sql(@"INSERT INTO dbo.PropertyValue ([Id], [Alias], [Name], [KeyValue], [ValueType], [ShortTextValue], [LongTextValue],
                                                 [DecimalValue], [IntegerValue], [BooleanValue], [DateTimeValue], [Locale],
                                                 [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [CategoryId]) 
                                                 SELECT [Id], [Alias], [Name], [KeyValue], [ValueType], [ShortTextValue], [LongTextValue],
                                                 [DecimalValue], [IntegerValue], [BooleanValue], [DateTimeValue], [Locale],
                                                 [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [CategoryId] FROM dbo.CategoryPropertyValue");
            Sql(@"INSERT INTO dbo.PropertyValue ([Id], [Alias], [Name], [KeyValue], [ValueType], [ShortTextValue], [LongTextValue],
                                                 [DecimalValue], [IntegerValue], [BooleanValue], [DateTimeValue], [Locale],
                                                 [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [ItemId])
                                                 SELECT [Id], [Alias], [Name], [KeyValue], [ValueType], [ShortTextValue], [LongTextValue],
                                                 [DecimalValue], [IntegerValue], [BooleanValue], [DateTimeValue], [Locale],
                                                 [CreatedDate], [ModifiedDate], [CreatedBy], [ModifiedBy], [ItemId] FROM dbo.ItemPropertyValue");
       

            //Change Property
            DropForeignKey("dbo.Property", "ParentPropertyId", "dbo.Property");
            DropIndex("dbo.Property", new[] { "ParentPropertyId" });
            DropColumn("dbo.Property", "ParentPropertyId");

            DropColumn("dbo.Property", "Discriminator");
            AddColumn("dbo.Property", "CategoryId", c => c.String(maxLength: 128));
            AddForeignKey("dbo.Property", "CategoryId", "dbo.Category", "Id");
            CreateIndex("dbo.Property", "CategoryId");

            Sql(@"UPDATE dbo.Property SET CatalogId = null");

            Sql(@"UPDATE dbo.Property SET CatalogId = C.Id FROM 
                  PropertySetProperty as PSP 
                  INNER JOIN PropertySet as PS ON PS.Id = PSP.PropertySetId
                  INNER JOIN TmpCatalog as C ON PS.Id = C.PropertySetId
                  WHERE PSP.PropertyId = dbo.Property.Id");
            Sql(@"UPDATE dbo.Property SET CategoryId = C.Id FROM 
                  PropertySetProperty as PSP 
                  INNER JOIN PropertySet as PS ON PS.Id = PSP.PropertySetId
                  INNER JOIN Category as C ON PS.Id = C.PropertySetId
                  WHERE PSP.PropertyId = dbo.Property.Id");




            //Delete PropertySet and PropertySetProperty tables
            DropForeignKey("dbo.Category", "PropertySetId", "dbo.PropertySet");
            DropIndex("dbo.Category", new[] { "PropertySetId" });
            DropColumn("dbo.Category", "PropertySetId");

            DropForeignKey("dbo.Item", "PropertySetId", "dbo.PropertySet");
            DropIndex("dbo.Item", new[] { "PropertySetId" });
            DropColumn("dbo.Item", "PropertySetId");

            DropColumn("dbo.Category", "Discriminator");
            DropColumn("dbo.CatalogLanguage", "Discriminator");
            DropColumn("dbo.PropertyAttribute", "Discriminator");


            DropTable("dbo.CatalogPropertyValue");
            DropTable("dbo.CategoryPropertyValue");
            DropTable("dbo.ItemPropertyValue");
            DropTable("dbo.PropertySetProperty");
            DropTable("dbo.PropertySet");
            DropTable("dbo.TmpCatalog");
            DropTable("dbo.VirtualCatalog");
        }
        
        public override void Down()
        {
           
        }
    }
}
