namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreReferencesCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StorePaymentMethod", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store");
            DropIndex("dbo.StorePaymentMethod", new[] { "StoreId" });
            DropIndex("dbo.StoreShippingMethod", new[] { "StoreId" });
            AlterColumn("dbo.StorePaymentMethod", "StoreId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StoreShippingMethod", "StoreId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.StorePaymentMethod", "StoreId");
            CreateIndex("dbo.StoreShippingMethod", "StoreId");
            AddForeignKey("dbo.StorePaymentMethod", "StoreId", "dbo.Store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StorePaymentMethod", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreShippingMethod", new[] { "StoreId" });
            DropIndex("dbo.StorePaymentMethod", new[] { "StoreId" });
            AlterColumn("dbo.StoreShippingMethod", "StoreId", c => c.String(maxLength: 128));
            AlterColumn("dbo.StorePaymentMethod", "StoreId", c => c.String(maxLength: 128));
            CreateIndex("dbo.StoreShippingMethod", "StoreId");
            CreateIndex("dbo.StorePaymentMethod", "StoreId");
            AddForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store", "Id");
            AddForeignKey("dbo.StorePaymentMethod", "StoreId", "dbo.Store", "Id");
        }
    }
}
