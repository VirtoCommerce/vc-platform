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
			ExecuteSqlScriptFile(context, "DynamicContentPlace.sql", "Marketing");
			ExecuteSqlScriptFile(context, "DynamicContentItem.sql", "Marketing");
			ExecuteSqlScriptFile(context, "DynamicContentItemProperty.sql", "Marketing");
			ExecuteSqlScriptFile(context, "DynamicContentPublishingGroup_Sample.sql", "Marketing");
			ExecuteSqlScriptFile(context, "PublishingGroupContentItem.sql", "Marketing");
			ExecuteSqlScriptFile(context, "PublishingGroupContentPlace.sql", "Marketing");
			//ExecuteCommand(Path.Combine(GetFrameworkDirectory(), "aspnet_regsql.exe"), string.Format("-C \"{0}\" -ed -et -t DynamicContentItemProperty", context.Database.Connection.ConnectionString));
		}
	}
}
