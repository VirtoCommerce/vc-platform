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

        public Currency(Language language, string code, string name, string symbol, decimal exchangeRate)
            : this(language, code)
        {
            EnglishName = name;
            Symbol = symbol;
            ExchangeRate = exchangeRate;
            if (NumberFormat == null)
            {
                NumberFormat = NumberFormatInfo.InvariantInfo.Clone() as NumberFormatInfo;
                //TODO: move to currency configuration
                NumberFormat.CurrencyPositivePattern = 3; //n $
                NumberFormat.CurrencyNegativePattern = 8; //-n $
            }
            if(!String.IsNullOrEmpty(symbol))
            {
                NumberFormat.CurrencySymbol = symbol;
            }
        }

        public Currency(Language language, string code)
        {
            Code = code;
            if (!language.IsInvariant)
            {
                var cultureInfo = CultureInfo.GetCultureInfo(language.CultureName);
                if (cultureInfo != null)
                {
                    NumberFormat = cultureInfo.NumberFormat.Clone() as NumberFormatInfo;
                    var region = new RegionInfo(cultureInfo.LCID);
                    Symbol = region.CurrencySymbol;
                    EnglishName = region.CurrencyEnglishName;
                }
            }
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
