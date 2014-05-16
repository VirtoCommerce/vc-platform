using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Order
{
	public static class NavigationNames
	{
		public const string MenuName = "OrderMenu";

		public const string HomeName = "OrderHome",
							PaymentsSettingsHomeName = "PaymentsSettingsHome",
							ShippingSettingsHomeName = "ShippingSettingsHome",
							TaxesSettingsHomeName = "TaxesSettingsHome";

		public const string ModuleName = "Order";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
		public static string Localize(this string source, string key, string category)
		{
			return LocalizeExtension.Localize(source, key, category);
		}
	}
}
