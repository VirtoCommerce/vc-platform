namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxTypeToStoreShippingMethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreShippingMethod", "TaxType", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreShippingMethod", "TaxType");
        }
    }
}
