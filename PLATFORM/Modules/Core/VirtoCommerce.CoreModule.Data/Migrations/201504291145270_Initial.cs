namespace VirtoCommerce.CoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FulfillmentCenter",
                c => new
                    {
                        FulfillmentCenterId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                        MaxReleasesPerPickBatch = c.Int(nullable: false),
                        PickDelay = c.Int(nullable: false),
                        DaytimePhoneNumber = c.String(nullable: false, maxLength: 32),
                        Line1 = c.String(nullable: false, maxLength: 128),
                        Line2 = c.String(maxLength: 128),
                        City = c.String(nullable: false, maxLength: 128),
                        StateProvince = c.String(maxLength: 128),
                        CountryCode = c.String(nullable: false, maxLength: 64),
                        CountryName = c.String(nullable: false, maxLength: 128),
                        PostalCode = c.String(nullable: false, maxLength: 32),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FulfillmentCenterId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FulfillmentCenter");
        }
    }
}
