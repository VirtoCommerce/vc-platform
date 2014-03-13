using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Configuration
{
    public static class NavigationNames
    {
        public const string MenuName = "ConfigurationMenu";
        public const string HomeName = "ConfigurationHome";
		public const string ModuleName = "Configuration";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
    }
}
