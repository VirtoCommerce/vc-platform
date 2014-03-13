using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Import
{
	public static class NavigationNames
	{
		public const string HomeName = "ImportJobHome",
							MenuName = "ImportJobMenu",
							CatalogHomeName = "CatalogHome",
							CatalogMenu = "CatalogMenu";
		public const string ModuleName = "Import";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
