using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Core.Utils
{
	public static class EnumUtility
	{
		public static T SafeParse<T>(string value, T defaultValue)
			where T : struct
		{
			T result;

			if (!Enum.TryParse(value, out result))
				result = defaultValue;

			return result;
		}
	}
}
