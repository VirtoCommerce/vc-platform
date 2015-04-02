namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Promotion
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SerializedRewards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotion", "RewardsSerialized", c => c.String());
        }
        
        public override void Down()
        {
            DropIndex("dbo.PromotionUsage", new[] { "PromotionId" });
     
        }
    }
}
