using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Reporting
{
    public static class NavigationNames
    {
        public const string MenuName = "ReportingMenu";
        public const string HomeName = "ReportingHome";

		public const string ModuleName = "Reporting";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
    }
}
