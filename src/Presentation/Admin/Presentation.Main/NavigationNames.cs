using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Main
{
	public static class NavigationNames
	{
		public const string MenuName = "MainMenu";
		public const string HomeName = "MainHome";
		public const string ModuleName = "Main";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
