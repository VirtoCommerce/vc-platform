using VirtoCommerce.Client.Globalization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;

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

        internal static void PublishStatusUpdate(string status)
        {
            EventSystem.Publish(new GenericEvent<string> { Message = status });
        }
    }
}
