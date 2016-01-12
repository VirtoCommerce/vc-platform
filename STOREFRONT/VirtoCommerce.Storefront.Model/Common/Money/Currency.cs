using System;
using System.Collections.Generic;
using System.Globalization;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class Currency
    {
        public string Code { get; set; }
        public NumberFormatInfo NumberFormat { get; set; }
        public string Symbol { get; set; }
        public string EnglishName { get; set; }
        public decimal ExchangeRate { get; set; }

        public Currency(string code, string name, string symbol, decimal exchangeRate)
            : this(code, exchangeRate)
        {
            EnglishName = name;
            Symbol = symbol;
            if (NumberFormat == null)
            {
                NumberFormat = NumberFormatInfo.InvariantInfo.Clone() as NumberFormatInfo;
                NumberFormat.CurrencySymbol = symbol;
                //TODO: move to currency configuration
                NumberFormat.CurrencyPositivePattern = 3; //n $
                NumberFormat.CurrencyNegativePattern = 8; //-n $
            }
        }

        public Currency(string code, decimal exchangeRate)
        {
            Code = code;
            ExchangeRate = exchangeRate;
            var cultureInfo = CultureInfoFromCurrencyISO(code);
            if (cultureInfo != null)
            {
                NumberFormat = cultureInfo.NumberFormat;
                var region = new RegionInfo(cultureInfo.LCID);
                Symbol = region.CurrencySymbol;
                EnglishName = region.CurrencyEnglishName;
            }
        }

        private static CultureInfo CultureInfoFromCurrencyISO(string isoCode)
        {
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo ri = new RegionInfo(ci.LCID);
                if (ri.ISOCurrencySymbol == isoCode)
                    return ci;
            }
            return null;
        }

        public bool IsHasSameCode(string code)
        {
            return string.Equals(Code, code, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            var other = obj as Currency;

            if (other != null)
            {
                return String.Equals(other.Code, Code, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            return Code.ToUpper().GetHashCode();
        }

    }


}
