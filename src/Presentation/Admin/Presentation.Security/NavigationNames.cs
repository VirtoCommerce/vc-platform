using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Security
{
	public static class NavigationNames
	{
		public const string LoginName = "Login";
        public const string MenuName = "SecurityMenu";
        public const string HomeName = "SecurityHome";
		public const string ModuleName = "Security";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
    }
}
