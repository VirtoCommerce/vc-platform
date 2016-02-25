using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace VirtoCommerce.Storefront.Model.Common
{
    /// <summary>
    /// Represent currency information in storefront. Contains some extra informations as exchnage rate, symbol, formating. 
    /// </summary>
    public class Currency
    {
        protected Currency()
        {
        }

        public Currency(Language language, string code, string name, string symbol, decimal exchangeRate)
            : this(language, code)
        {
            ExchangeRate = exchangeRate;
      
            if (!String.IsNullOrEmpty(name))
            {
                EnglishName = name;
            }
            if (!String.IsNullOrEmpty(symbol))
            {
                Symbol = symbol;
                NumberFormat.CurrencySymbol = symbol;
            }
        }

        public Currency(Language language, string code)
        {
            Code = code;
            ExchangeRate = 1;
            if (!language.IsInvariant)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(language.CultureName);
                NumberFormat = cultureInfo.NumberFormat.Clone() as NumberFormatInfo;
                var region = new RegionInfo(cultureInfo.LCID);
                EnglishName = region.CurrencyEnglishName;
                Symbol = CurrencySymbolFromCodeISO(code) ?? "N/A";
                NumberFormat.CurrencySymbol = Symbol;
            }
            else
            {
                NumberFormat = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
            }
        }

        /// <summary>
        /// Currency code may be used ISO 4217
        /// </summary>
        public string Code { get; set; }
        [IgnoreDataMember]
        public NumberFormatInfo NumberFormat { get; set; }
        public string Symbol { get; set; }
        public string EnglishName { get; set; }
        /// <summary>
        /// Exchnage rate with primary currency
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dwhawy9k%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
        /// </summary>
        public string CustomFormatting { get; set; }


        private static string CurrencySymbolFromCodeISO(string isoCode)
        {
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo ri = new RegionInfo(ci.LCID);
                if (ri.ISOCurrencySymbol == isoCode)
                    return ri.CurrencySymbol;
            }
            return null;
        }

        public static bool operator ==(Currency left, Currency right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Currency left, Currency right)
        {
            return !(left == right);
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
            var code = obj as string;
            if (other != null)
            {
                return String.Equals(Code, other.Code, StringComparison.InvariantCultureIgnoreCase);
            }
            if(code != null)
            {
                return String.Equals(Code, code, StringComparison.InvariantCultureIgnoreCase);
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
