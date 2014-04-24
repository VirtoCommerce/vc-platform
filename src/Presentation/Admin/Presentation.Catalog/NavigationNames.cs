using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Catalog
{
	public static class NavigationNames
	{
		public const string MenuName = "CatalogMenu",
							HomeName = "CatalogHome",
							MenuNamePriceList = "PriceListMenu",
							HomeNamePriceList = "PriceListHome",
							HomeNamePriceListAssignment = "HomeNamePriceListAssignment",
							MenuNamePriceListAssignment = "MenuNamePriceListAssignment",
							HomeNameCatalogImportJob = "CatalogImportJobHome",
							HomeNamePricelistImportJob = "PricelistImportJobHome",
							HomeNameReviews = "ReviewsHome",
							ModuleName = "Catalog";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
