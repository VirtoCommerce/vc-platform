namespace VirtoCommerce.Foundation.Data.Marketing
{
	public class SqlDynamicContentSampleDatabaseInitializer : SqlDynamicContentDatabaseInitializer
	{
		protected override void Seed(EFDynamicContentRepository context)
		{
			base.Seed(context);
			FillDynamicContentScripts(context);
		}

		private void FillDynamicContentScripts(EFDynamicContentRepository context)
		{
			RunCommand(context, "DynamicContentPlace.sql", "Marketing");
			RunCommand(context, "DynamicContentItem.sql", "Marketing");
			RunCommand(context, "DynamicContentItemProperty.sql", "Marketing");
			RunCommand(context, "DynamicContentPublishingGroup_Sample.sql", "Marketing");
			RunCommand(context, "PublishingGroupContentItem.sql", "Marketing");
			RunCommand(context, "PublishingGroupContentPlace.sql", "Marketing");
			//ExecuteCommand(Path.Combine(GetFrameworkDirectory(), "aspnet_regsql.exe"), string.Format("-C \"{0}\" -ed -et -t DynamicContentItemProperty", context.Database.Connection.ConnectionString));
		}
	}
}
