namespace VirtoCommerce.PricingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Price",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Sale = c.Decimal(precision: 18, scale: 2),
                        List = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.String(maxLength: 128),
                        ProductName = c.String(maxLength: 1024),
                        MinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pricelist", t => t.PricelistId, cascadeDelete: true)
                .Index(t => t.PricelistId);
            
            CreateTable(
                "dbo.Pricelist",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Currency = c.String(nullable: false, maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PricelistAssignment",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Priority = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        ConditionExpression = c.String(),
                        PredicateVisualTreeSerialized = c.String(),
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pricelist", t => t.PricelistId, cascadeDelete: true)
                .Index(t => t.PricelistId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PricelistAssignment", "PricelistId", "dbo.Pricelist");
            DropForeignKey("dbo.Price", "PricelistId", "dbo.Pricelist");
            DropIndex("dbo.PricelistAssignment", new[] { "PricelistId" });
            DropIndex("dbo.Price", new[] { "PricelistId" });
            DropTable("dbo.PricelistAssignment");
            DropTable("dbo.Pricelist");
            DropTable("dbo.Price");
        }
    }
}
