using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Customers
{
	public static class NavigationNames
	{
		public const string MenuName = "CustomersMenu";
        public const string HomeName = "CustomersHome",
                            HomeNameSearch = "SearchHome",
							ModuleName = "Customers";

		public static string Localize(this string source)
		{
			return source.Localize(null, ModuleName);
		}
	}
}
