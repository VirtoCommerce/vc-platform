namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OperationProperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderShipment", "CustomerOrderId", "dbo.CustomerOrder");
            CreateTable(
                "dbo.OrderOperation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Number = c.String(nullable: false, maxLength: 64),
                        IsApproved = c.Boolean(nullable: false),
                        Status = c.String(maxLength: 64),
                        Comment = c.String(maxLength: 2048),
                        Currency = c.String(nullable: false, maxLength: 3),
                        TaxIncluded = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledDate = c.DateTime(),
                        CancelReason = c.String(maxLength: 2048),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        ModifiedBy = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);

			Sql("INSERT INTO dbo.OrderOperation  SELECT CO.Id, CO.Number, CO.IsApproved, CO.Status, CO.Comment, CO.Currency, CO.TaxIncluded, CO.Sum, CO.Tax, CO.IsCancelled, CO.CancelledDate, CO.CancelReason, CO.CreatedDate, CO.ModifiedDate, CO.CreatedBy, CO.ModifiedBy  FROM dbo.CustomerOrder AS CO");
			Sql("INSERT INTO dbo.OrderOperation  SELECT CO.Id, CO.Number, CO.IsApproved, CO.Status, CO.Comment, CO.Currency, CO.TaxIncluded, CO.Sum, CO.Tax, CO.IsCancelled, CO.CancelledDate, CO.CancelReason, CO.CreatedDate, CO.ModifiedDate, CO.CreatedBy, CO.ModifiedBy  FROM dbo.OrderShipment AS CO");
			Sql("INSERT INTO dbo.OrderOperation  SELECT CO.Id, CO.Number, CO.IsApproved, CO.Status, CO.Comment, CO.Currency, CO.TaxIncluded, CO.Sum, CO.Tax, CO.IsCancelled, CO.CancelledDate, CO.CancelReason, CO.CreatedDate, CO.ModifiedDate, CO.CreatedBy, CO.ModifiedBy  FROM dbo.OrderPaymentIn AS CO");
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderOperation", t => t.OperationId, cascadeDelete: true)
                .Index(t => t.OperationId);
            
            CreateIndex("dbo.CustomerOrder", "Id");
            CreateIndex("dbo.OrderShipment", "Id");
            CreateIndex("dbo.OrderPaymentIn", "Id");
            AddForeignKey("dbo.CustomerOrder", "Id", "dbo.OrderOperation", "Id");
            AddForeignKey("dbo.OrderShipment", "Id", "dbo.OrderOperation", "Id");
            AddForeignKey("dbo.OrderPaymentIn", "Id", "dbo.OrderOperation", "Id");
            AddForeignKey("dbo.OrderShipment", "CustomerOrderId", "dbo.CustomerOrder", "Id");
            DropColumn("dbo.CustomerOrder", "Number");
            DropColumn("dbo.CustomerOrder", "IsApproved");
            DropColumn("dbo.CustomerOrder", "Status");
            DropColumn("dbo.CustomerOrder", "Comment");
            DropColumn("dbo.CustomerOrder", "Currency");
            DropColumn("dbo.CustomerOrder", "TaxIncluded");
            DropColumn("dbo.CustomerOrder", "Sum");
            DropColumn("dbo.CustomerOrder", "Tax");
            DropColumn("dbo.CustomerOrder", "IsCancelled");
            DropColumn("dbo.CustomerOrder", "CancelledDate");
            DropColumn("dbo.CustomerOrder", "CancelReason");
            DropColumn("dbo.CustomerOrder", "CreatedDate");
            DropColumn("dbo.CustomerOrder", "ModifiedDate");
            DropColumn("dbo.CustomerOrder", "CreatedBy");
            DropColumn("dbo.CustomerOrder", "ModifiedBy");
            DropColumn("dbo.OrderPaymentIn", "Number");
            DropColumn("dbo.OrderPaymentIn", "IsApproved");
            DropColumn("dbo.OrderPaymentIn", "Status");
            DropColumn("dbo.OrderPaymentIn", "Comment");
            DropColumn("dbo.OrderPaymentIn", "Currency");
            DropColumn("dbo.OrderPaymentIn", "TaxIncluded");
            DropColumn("dbo.OrderPaymentIn", "Sum");
            DropColumn("dbo.OrderPaymentIn", "Tax");
            DropColumn("dbo.OrderPaymentIn", "IsCancelled");
            DropColumn("dbo.OrderPaymentIn", "CancelledDate");
            DropColumn("dbo.OrderPaymentIn", "CancelReason");
            DropColumn("dbo.OrderPaymentIn", "CreatedDate");
            DropColumn("dbo.OrderPaymentIn", "ModifiedDate");
            DropColumn("dbo.OrderPaymentIn", "CreatedBy");
            DropColumn("dbo.OrderPaymentIn", "ModifiedBy");
            DropColumn("dbo.OrderShipment", "Number");
            DropColumn("dbo.OrderShipment", "IsApproved");
            DropColumn("dbo.OrderShipment", "Status");
            DropColumn("dbo.OrderShipment", "Comment");
            DropColumn("dbo.OrderShipment", "Currency");
            DropColumn("dbo.OrderShipment", "TaxIncluded");
            DropColumn("dbo.OrderShipment", "Sum");
            DropColumn("dbo.OrderShipment", "Tax");
            DropColumn("dbo.OrderShipment", "IsCancelled");
            DropColumn("dbo.OrderShipment", "CancelledDate");
            DropColumn("dbo.OrderShipment", "CancelReason");
            DropColumn("dbo.OrderShipment", "CreatedDate");
            DropColumn("dbo.OrderShipment", "ModifiedDate");
            DropColumn("dbo.OrderShipment", "CreatedBy");
            DropColumn("dbo.OrderShipment", "ModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderShipment", "ModifiedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderShipment", "CreatedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderShipment", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.OrderShipment", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderShipment", "CancelReason", c => c.String(maxLength: 2048));
            AddColumn("dbo.OrderShipment", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.OrderShipment", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderShipment", "Tax", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.OrderShipment", "Sum", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.OrderShipment", "TaxIncluded", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderShipment", "Currency", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.OrderShipment", "Comment", c => c.String(maxLength: 2048));
            AddColumn("dbo.OrderShipment", "Status", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderShipment", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderShipment", "Number", c => c.String(nullable: false, maxLength: 64));
            AddColumn("dbo.OrderPaymentIn", "ModifiedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderPaymentIn", "CreatedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderPaymentIn", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.OrderPaymentIn", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderPaymentIn", "CancelReason", c => c.String(maxLength: 2048));
            AddColumn("dbo.OrderPaymentIn", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.OrderPaymentIn", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderPaymentIn", "Tax", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.OrderPaymentIn", "Sum", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.OrderPaymentIn", "TaxIncluded", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderPaymentIn", "Currency", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.OrderPaymentIn", "Comment", c => c.String(maxLength: 2048));
            AddColumn("dbo.OrderPaymentIn", "Status", c => c.String(maxLength: 64));
            AddColumn("dbo.OrderPaymentIn", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderPaymentIn", "Number", c => c.String(nullable: false, maxLength: 64));
            AddColumn("dbo.CustomerOrder", "ModifiedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.CustomerOrder", "CreatedBy", c => c.String(maxLength: 64));
            AddColumn("dbo.CustomerOrder", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.CustomerOrder", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerOrder", "CancelReason", c => c.String(maxLength: 2048));
            AddColumn("dbo.CustomerOrder", "CancelledDate", c => c.DateTime());
            AddColumn("dbo.CustomerOrder", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerOrder", "Tax", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.CustomerOrder", "Sum", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.CustomerOrder", "TaxIncluded", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerOrder", "Currency", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.CustomerOrder", "Comment", c => c.String(maxLength: 2048));
            AddColumn("dbo.CustomerOrder", "Status", c => c.String(maxLength: 64));
            AddColumn("dbo.CustomerOrder", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerOrder", "Number", c => c.String(nullable: false, maxLength: 64));
            DropForeignKey("dbo.OrderShipment", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.OrderPaymentIn", "Id", "dbo.OrderOperation");
            DropForeignKey("dbo.OrderShipment", "Id", "dbo.OrderOperation");
            DropForeignKey("dbo.CustomerOrder", "Id", "dbo.OrderOperation");
            DropForeignKey("dbo.OrderOperationProperty", "OperationId", "dbo.OrderOperation");
            DropIndex("dbo.OrderPaymentIn", new[] { "Id" });
            DropIndex("dbo.OrderShipment", new[] { "Id" });
            DropIndex("dbo.CustomerOrder", new[] { "Id" });
            DropIndex("dbo.OrderOperationProperty", new[] { "OperationId" });
            DropTable("dbo.OrderOperationProperty");
            DropTable("dbo.OrderOperation");
            AddForeignKey("dbo.OrderShipment", "CustomerOrderId", "dbo.CustomerOrder", "Id", cascadeDelete: true);
        }
    }
}
