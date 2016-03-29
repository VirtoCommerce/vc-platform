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
                                            .Where(c => !c.IsNeutralCulture && c.LCID != 127)
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

        /// <summary>
        /// Return all localized strings for specified language also always returns strings with invariant language
        /// </summary>
        /// <param name="localizedStrings"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static IEnumerable<LocalizedString> GetLocalizedStringsForLanguage(this IEnumerable<LocalizedString> localizedStrings, Language language)
        {
            if(localizedStrings == null)
            {
                throw new ArgumentNullException("localizedStrings");
            }
            if(language == null)
            {
                throw new ArgumentNullException("language");
            }
            var retVal = localizedStrings.Where(x => x.Language == language || x.Language.IsInvariant).ToArray();
            return retVal;
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
