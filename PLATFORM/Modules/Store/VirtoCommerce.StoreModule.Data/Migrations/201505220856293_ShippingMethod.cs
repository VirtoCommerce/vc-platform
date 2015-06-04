namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingMethod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreShippingMethod",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 128),
                        Priority = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Description = c.String(),
                        LogoUrl = c.String(maxLength: 2048),
                        IsActive = c.Boolean(nullable: false),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreShippingMethod", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreShippingMethod", new[] { "StoreId" });
            DropTable("dbo.StoreShippingMethod");
        }
    }
}
