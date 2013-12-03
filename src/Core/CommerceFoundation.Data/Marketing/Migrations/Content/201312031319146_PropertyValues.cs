namespace VirtoCommerce.Foundation.Data.Marketing.Migrations.Content
{
	using System.Data.Entity.Migrations;

	public partial class PropertyValues : DbMigration
	{
		public override void Up()
		{
			AlterColumn("dbo.DynamicContentItemProperty", "Name", c => c.String(maxLength: 64));
		}

		public override void Down()
		{
			AlterColumn("dbo.DynamicContentItemProperty", "Name", c => c.String(maxLength: 128));
		}
	}
}
