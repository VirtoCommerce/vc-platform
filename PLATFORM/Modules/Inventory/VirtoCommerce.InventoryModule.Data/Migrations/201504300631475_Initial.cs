namespace VirtoCommerce.InventoryModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FulfillmentCenterId = c.String(nullable: false, maxLength: 128),
                        InStockQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReservedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReorderMinQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreorderQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BackorderQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AllowBackorder = c.Boolean(nullable: false),
                        AllowPreorder = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        PreorderAvailabilityDate = c.DateTime(),
                        BackorderAvailabilityDate = c.DateTime(),
                        Sku = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Inventory");
        }
    }
}
