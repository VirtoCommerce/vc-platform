namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethod : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StorePaymentGateway", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store");
            DropIndex("dbo.StorePaymentGateway", new[] { "StoreId" });
            DropIndex("dbo.StoreShippingMethod", new[] { "StoreId" });
            CreateTable(
                "dbo.StorePaymentMethod",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Description = c.String(),
                        LogoUrl = c.String(maxLength: 2048),
                        IsActive = c.Boolean(nullable: false),
                        StoreId = c.String(maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .Index(t => t.StoreId);
            
            AlterColumn("dbo.StoreShippingMethod", "StoreId", c => c.String(maxLength: 128));
            CreateIndex("dbo.StoreShippingMethod", "StoreId");
            AddForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store", "Id");
            DropTable("dbo.StorePaymentGateway");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StorePaymentGateway",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PaymentGateway = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StorePaymentMethod", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreShippingMethod", new[] { "StoreId" });
            DropIndex("dbo.StorePaymentMethod", new[] { "StoreId" });
            AlterColumn("dbo.StoreShippingMethod", "StoreId", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.StorePaymentMethod");
            CreateIndex("dbo.StoreShippingMethod", "StoreId");
            CreateIndex("dbo.StorePaymentGateway", "StoreId");
            AddForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StorePaymentGateway", "StoreId", "dbo.Store", "Id", cascadeDelete: true);
        }
    }
}
