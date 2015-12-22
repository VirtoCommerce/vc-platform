using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public static class LocalizationExtensions
    {
        private static readonly  RegionInfo[] _cachedRegionInfos;
        static LocalizationExtensions()
        {
            _cachedRegionInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                            .Where(c => !c.IsNeutralCulture)
                .Select(x => 
                {
                    try
                    {
                        return new RegionInfo(x.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(x=> x != null).ToArray();
        }

        public static string GetCurrencySymbol(this string ISOCurrencySymbol)
        {
            var symbol = _cachedRegionInfos.Where(x => x != null && String.Equals(x.ISOCurrencySymbol, ISOCurrencySymbol, StringComparison.InvariantCultureIgnoreCase))
                                       .Select(ri => ri.CurrencySymbol)
                                       .FirstOrDefault();
            return symbol;
        }
    }
}
