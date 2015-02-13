using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Converters
{
	public static class ThemeItemConverter
	{
		public static ThemeItem ContentItem2ThemeItem(ContentItem item)
		{
			var retVal = new ThemeItem();

			retVal.ThemeName = item.Path.Replace("/", string.Empty);
			retVal.ThemePath = item.Path;

			return retVal;
		}
	}
}
