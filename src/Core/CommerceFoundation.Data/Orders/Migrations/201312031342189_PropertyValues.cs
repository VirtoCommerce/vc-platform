namespace VirtoCommerce.Foundation.Data.Orders.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class PropertyValues : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.OrderFormPropertyValue",
				c => new
					{
						PropertyValueId = c.String(nullable: false, maxLength: 128),
						OrderFormId = c.String(maxLength: 128),
						Alias = c.String(maxLength: 64),
						Name = c.String(maxLength: 64),
						KeyValue = c.String(maxLength: 128),
						ValueType = c.Int(nullable: false),
						ShortTextValue = c.String(maxLength: 512),
						LongTextValue = c.String(),
						DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
						IntegerValue = c.Int(nullable: false),
						BooleanValue = c.Boolean(nullable: false),
						DateTimeValue = c.DateTime(),
						Locale = c.String(maxLength: 64),
						LastModified = c.DateTime(),
						Created = c.DateTime(),
					})
				.PrimaryKey(t => t.PropertyValueId)
				.ForeignKey("dbo.OrderForm", t => t.OrderFormId)
				.Index(t => t.OrderFormId);

		}

		public override void Down()
		{
			DropIndex("dbo.OrderFormPropertyValue", new[] { "OrderFormId" });
			DropForeignKey("dbo.OrderFormPropertyValue", "OrderFormId", "dbo.OrderForm");
			DropTable("dbo.OrderFormPropertyValue");
		}
	}
}
