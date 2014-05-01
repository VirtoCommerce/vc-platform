using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Marketing
{
	public static class NavigationNames
	{
		public const string MenuName = "MarketingMenu";
		public const string HomeName = "MarketingHome",
            HomeNameDynamicContent = "DynamicContentHome",
                HomeNameContentPublishing = "ContentPublishingHome";

		public const string ModuleName = "Marketing";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
