namespace VirtoCommerce.Foundation.Data.Customers.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class PropertyValues : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.ContactPropertyValue", "Alias", c => c.String(maxLength: 64));
			AddColumn("dbo.ContactPropertyValue", "KeyValue", c => c.String(maxLength: 128));
			AddColumn("dbo.ContactPropertyValue", "Locale", c => c.String(maxLength: 64));
			AddColumn("dbo.CasePropertyValue", "Alias", c => c.String(maxLength: 64));
			AddColumn("dbo.CasePropertyValue", "KeyValue", c => c.String(maxLength: 128));
			AddColumn("dbo.CasePropertyValue", "Locale", c => c.String(maxLength: 64));
			AlterColumn("dbo.ContactPropertyValue", "Name", c => c.String(maxLength: 64));
			AlterColumn("dbo.CasePropertyValue", "Name", c => c.String(maxLength: 64));
		}

		public override void Down()
		{
			AlterColumn("dbo.CasePropertyValue", "Name", c => c.String(maxLength: 128));
			AlterColumn("dbo.ContactPropertyValue", "Name", c => c.String(maxLength: 128));
			DropColumn("dbo.CasePropertyValue", "Locale");
			DropColumn("dbo.CasePropertyValue", "KeyValue");
			DropColumn("dbo.CasePropertyValue", "Alias");
			DropColumn("dbo.ContactPropertyValue", "Locale");
			DropColumn("dbo.ContactPropertyValue", "KeyValue");
			DropColumn("dbo.ContactPropertyValue", "Alias");
		}
	}
}
