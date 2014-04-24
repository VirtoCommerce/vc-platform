using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.DynamicContent
{
	public static class NavigationNames
	{
		public const string MenuName = "MarketingMenu"; //"DynamicContentMenu";
		public const string HomeName = "MarketingHome", //"DynamicContentHome";
            HomeNameDynamicContent = "DynamicContentHome",
                HomeNameContentPublishing = "ContentPublishingHome";

		public const string ModuleName = "DynamicContent";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
    }
}
