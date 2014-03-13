using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.AppConfig
{
	public static class NavigationNames
	{
		public const string MenuName = "AppConfigMenu";
		public const string HomeName = "AppConfigHome";
		public const string ModuleName = "AppConfig";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
