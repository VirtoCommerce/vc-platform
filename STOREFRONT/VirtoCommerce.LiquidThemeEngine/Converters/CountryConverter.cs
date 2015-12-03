using System.Globalization;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CountryConverter
    {
        public static string ToOptionTag(this Country country)
        {
            var regions = "[]";

            if (country.Regions != null)
            {
                regions = JsonConvert.SerializeObject(country.Regions.Select(r => r.Name));
            }

            return string.Format(CultureInfo.InvariantCulture, "<option value=\"{0}\" data-provinces=\"{1}\">{0}</option>",
                HttpUtility.HtmlAttributeEncode(country.Name),
                HttpUtility.HtmlAttributeEncode(regions)
                );
        }
    }
}
