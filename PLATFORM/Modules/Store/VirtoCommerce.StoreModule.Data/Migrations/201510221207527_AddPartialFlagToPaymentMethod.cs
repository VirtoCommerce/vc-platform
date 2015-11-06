namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPartialFlagToPaymentMethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StorePaymentMethod", "IsAvailableForPartial", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StorePaymentMethod", "IsAvailableForPartial");
        }
    }
}
