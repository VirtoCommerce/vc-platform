namespace VirtoCommerce.Platform.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpDecimalPrecisionToFive : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlatformSettingValue", "DecimalValue", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("dbo.PlatformDynamicPropertyObjectValue", "DecimalValue", c => c.Decimal(precision: 18, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlatformDynamicPropertyObjectValue", "DecimalValue", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PlatformSettingValue", "DecimalValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
