namespace VirtoCommerce.CartModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContryCodeNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.cart_Address", "CountryCode", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.cart_Address", "CountryCode", c => c.String(nullable: false, maxLength: 3));
        }
    }
}
