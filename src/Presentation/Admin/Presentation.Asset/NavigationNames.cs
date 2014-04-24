using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Asset
{
	public static class NavigationNames
	{
		public const string ModuleName = "Asset";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
