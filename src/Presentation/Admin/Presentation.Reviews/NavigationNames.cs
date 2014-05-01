using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Reviews
{
	public static class NavigationNames
	{
		public const string MenuName = "ReviewsMenu",
			HomeName = "ReviewsHome",
			CatalogHome = "CatalogHome",
			CatalogMenu = "CatalogMenu";

		public const string ModuleName = "Reviews";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
