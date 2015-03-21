namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsageProps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromotionUsage", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.PromotionUsage", "UsageDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromotionUsage", "UsageDate");
            DropColumn("dbo.PromotionUsage", "Status");
        }
    }
}
