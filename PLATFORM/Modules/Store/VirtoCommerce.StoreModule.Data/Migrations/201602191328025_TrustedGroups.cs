namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrustedGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreTrustedGroup",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        GroupName = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            DropColumn("dbo.Store", "Discriminator");
            DropColumn("dbo.StoreCurrency", "Discriminator");
            DropColumn("dbo.StoreLanguage", "Discriminator");
            DropColumn("dbo.StorePaymentMethod", "Discriminator");
            DropColumn("dbo.StoreShippingMethod", "Discriminator");
            DropColumn("dbo.StoreTaxProvider", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoreTaxProvider", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.StoreShippingMethod", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.StorePaymentMethod", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.StoreLanguage", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.StoreCurrency", "Discriminator", c => c.String(maxLength: 128));
            AddColumn("dbo.Store", "Discriminator", c => c.String(maxLength: 128));
            DropForeignKey("dbo.StoreTrustedGroup", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreTrustedGroup", new[] { "StoreId" });
            DropTable("dbo.StoreTrustedGroup");
        }
    }
}
