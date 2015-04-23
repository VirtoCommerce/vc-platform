using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
	public static class StringExtensions
	{
		public static Dictionary<string, string> ToDictionary(this string sourceString, string pairSeparator, string valueSeparator)
		{
			return sourceString.Split(new[] { pairSeparator }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Split(new[] { valueSeparator }, 2, StringSplitOptions.RemoveEmptyEntries))
				.ToDictionary(x => x[0], x => x.Length == 2 ? x[1] : string.Empty, StringComparer.OrdinalIgnoreCase);
		}
	}
}
