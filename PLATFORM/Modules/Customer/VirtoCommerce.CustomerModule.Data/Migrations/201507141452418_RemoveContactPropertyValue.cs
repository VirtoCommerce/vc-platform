namespace VirtoCommerce.CustomerModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveContactPropertyValue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactPropertyValue", "ContactId", "dbo.Contact");
            DropIndex("dbo.ContactPropertyValue", new[] { "ContactId" });
            DropTable("dbo.ContactPropertyValue");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ContactPropertyValue",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 128),
                        ValueType = c.Int(nullable: false),
                        ShortTextValue = c.String(maxLength: 512),
                        LongTextValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IntegerValue = c.Int(nullable: false),
                        BooleanValue = c.Boolean(nullable: false),
                        DateTimeValue = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        ContactId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ContactPropertyValue", "ContactId");
            AddForeignKey("dbo.ContactPropertyValue", "ContactId", "dbo.Contact", "Id");
        }
    }
}
