using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class CurrentPrincipal
	{
		public static string GetCurrentUserName()
		{
			var userName = Thread.CurrentPrincipal == null ? "unknown" : Thread.CurrentPrincipal.Identity.Name;
			if (String.IsNullOrEmpty(userName))
				userName = "unknown";
			return userName;
		}
	}
}
