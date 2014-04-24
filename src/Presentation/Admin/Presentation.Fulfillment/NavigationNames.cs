using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Fulfillment
{
	public static class NavigationNames
	{
		public const string MenuName = "FulfillmentMenu",
			HomeName = "FulfillmentHome",
            StoresSettingsHomeName = "StoresSettingsHome",
			PicklistMenuName="PicklistMenu",
			PicklistHomeName="PicklistHome",
			RmaMenuName = "RmaMenu",
			RmaHomeName = "RmaHome";

		public const string ModuleName = "Fulfillment";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
