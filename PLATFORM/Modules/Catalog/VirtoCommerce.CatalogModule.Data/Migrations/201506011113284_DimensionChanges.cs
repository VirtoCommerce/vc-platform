namespace VirtoCommerce.CatalogModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DimensionChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "ManufacturerPartNumber", c => c.String(maxLength: 128));
            AddColumn("dbo.Item", "Gtin", c => c.String(maxLength: 64));
            AddColumn("dbo.Item", "ProductType", c => c.String(maxLength: 64));
            AddColumn("dbo.Item", "WeightUnit", c => c.String(maxLength: 32));
            AddColumn("dbo.Item", "MeasureUnit", c => c.String(maxLength: 32));
            AddColumn("dbo.Item", "Height", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Item", "Length", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Item", "Width", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Item", "EnableReview", c => c.Boolean());
            AddColumn("dbo.Item", "MaxNumberOfDownload", c => c.Int());
            AddColumn("dbo.Item", "DownloadExpiration", c => c.DateTime());
            AddColumn("dbo.Item", "DownloadType", c => c.String(maxLength: 64));
            AddColumn("dbo.Item", "HasUserAgreement", c => c.Boolean());
            AddColumn("dbo.Item", "ShippingType", c => c.String(maxLength: 64));
            AddColumn("dbo.Item", "TaxType", c => c.String(maxLength: 64));
            AddColumn("dbo.Item", "Vendor", c => c.String(maxLength: 128));
            AlterColumn("dbo.Item", "Weight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Item", "Vendor");
            DropColumn("dbo.Item", "TaxType");
            DropColumn("dbo.Item", "ShippingType");
            DropColumn("dbo.Item", "HasUserAgreement");
            DropColumn("dbo.Item", "DownloadType");
            DropColumn("dbo.Item", "DownloadExpiration");
            DropColumn("dbo.Item", "MaxNumberOfDownload");
            DropColumn("dbo.Item", "EnableReview");
            DropColumn("dbo.Item", "Width");
            DropColumn("dbo.Item", "Length");
            DropColumn("dbo.Item", "Height");
            DropColumn("dbo.Item", "MeasureUnit");
            DropColumn("dbo.Item", "WeightUnit");
            DropColumn("dbo.Item", "ProductType");
            DropColumn("dbo.Item", "Gtin");
            DropColumn("dbo.Item", "ManufacturerPartNumber");
        }
    }
}
