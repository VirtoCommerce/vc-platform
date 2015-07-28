namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOrderProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderOperationProperty", "OperationId", "dbo.OrderOperation");
            DropIndex("dbo.OrderOperationProperty", new[] { "OperationId" });
            DropTable("dbo.OrderOperationProperty");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderOperationProperty",
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
                        OperationId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.OrderOperationProperty", "OperationId");
            AddForeignKey("dbo.OrderOperationProperty", "OperationId", "dbo.OrderOperation", "Id", cascadeDelete: true);
        }
    }
}
