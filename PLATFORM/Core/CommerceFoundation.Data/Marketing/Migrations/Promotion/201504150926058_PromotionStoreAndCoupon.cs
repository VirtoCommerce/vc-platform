namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromotionStoreAndCoupon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotion", "CouponCode", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotion", "CouponCode");
        }
    }
}
