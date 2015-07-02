namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxTypeToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartShipment", "TaxType", c => c.String(maxLength: 64));
            AddColumn("dbo.CartLineItem", "TaxType", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartLineItem", "TaxType");
            DropColumn("dbo.CartShipment", "TaxType");
        }
    }
}
