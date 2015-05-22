namespace VirtoCommerce.StoreModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStoreSetting : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StoreSetting", "StoreId", "dbo.Store");
            DropIndex("dbo.StoreSetting", new[] { "StoreId" });
            DropTable("dbo.StoreSetting");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StoreSetting",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Locale = c.String(maxLength: 64),
                        ValueType = c.String(nullable: false, maxLength: 64),
                        StoreId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.StoreSetting", "StoreId");
            AddForeignKey("dbo.StoreSetting", "StoreId", "dbo.Store", "Id", cascadeDelete: true);
        }
    }
}
