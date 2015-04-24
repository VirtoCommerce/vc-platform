namespace VirtoCommerce.PricingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.vc_Price",
                c => new
                    {
                        PriceId = c.String(nullable: false, maxLength: 128),
                        Sale = c.Decimal(precision: 18, scale: 2),
                        List = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.String(maxLength: 128),
                        ProductName = c.String(maxLength: 1024),
                        MinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PriceId)
                .ForeignKey("dbo.vc_Pricelist", t => t.PricelistId, cascadeDelete: true)
                .Index(t => t.PricelistId);
            
            CreateTable(
                "dbo.vc_Pricelist",
                c => new
                    {
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 512),
                        Currency = c.String(nullable: false, maxLength: 64),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PricelistId);
            
            CreateTable(
                "dbo.vc_PricelistAssignment",
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
                        CatalogId = c.String(nullable: false, maxLength: 128),
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PricelistAssignmentId)
                .ForeignKey("dbo.vc_Pricelist", t => t.PricelistId, cascadeDelete: true)
                .Index(t => t.PricelistId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.vc_PricelistAssignment", "PricelistId", "dbo.vc_Pricelist");
            DropForeignKey("dbo.vc_Price", "PricelistId", "dbo.vc_Pricelist");
            DropIndex("dbo.vc_PricelistAssignment", new[] { "PricelistId" });
            DropIndex("dbo.vc_Price", new[] { "PricelistId" });
            DropTable("dbo.vc_PricelistAssignment");
            DropTable("dbo.vc_Pricelist");
            DropTable("dbo.vc_Price");
        }
    }
}
