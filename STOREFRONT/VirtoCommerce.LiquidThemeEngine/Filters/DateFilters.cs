using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    public class DateFilters
    {
        public static string Date(object input, string format)
        {
            if (!string.IsNullOrEmpty(format) && !format.Contains("%")) // special formats that can be defined in settings
            {
                var key = string.Concat("date_formats.", format);
                var newFormat = TranslationFilter.T(key);
                if (newFormat != key)
                    format = newFormat;

                if (format == "long")
                {
                    format = "%d %b %Y %X";
                }
            }

            return StandardFilters.Date(input, format);
        }
    }
}
