using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VirtoCommerce.Storefront.Model.Common
{
    /// <summary>
    /// Represent currency information in storefront. Contains some extra informations as exchnage rate, symbol, formating. 
    /// </summary>
    public class Currency
    {
        private Language _language;
        private string _code;

        protected Currency()
        {
        }

        public Currency(Language language, string code, string name, string symbol, decimal exchangeRate)
            : this(language, code)
        {
            ExchangeRate = exchangeRate;

            if (!string.IsNullOrEmpty(name))
            {
                EnglishName = name;
            }
            if (!string.IsNullOrEmpty(symbol))
            {
                Symbol = symbol;
                NumberFormat.CurrencySymbol = symbol;
            }
        }

        public Currency(Language language, string code)
        {
            _language = language;
            _code = code;
            ExchangeRate = 1;
            Initialize();
        }

        /// <summary>
        /// Currency code may be used ISO 4217
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                Initialize();
            }
        }

        public string CultureName
        {
            get { return _language.CultureName; }
            set
            {
                _language = new Language(value);
                Initialize();
            }
        }

        [IgnoreDataMember]
        public NumberFormatInfo NumberFormat { get; private set; }
        public string Symbol { get; set; }
        public string EnglishName { get; set; }
        /// <summary>
        /// Exchnage rate with primary currency
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dwhawy9k%28v=vs.110%29.aspx?f=255&amp;MSPPError=-2147217396
        /// </summary>
        public string CustomFormatting { get; set; }


        private void Initialize()
        {
            if (_language != null)
            {
                if (!_language.IsInvariant)
                {
                    var cultureInfo = CultureInfo.GetCultureInfo(_language.CultureName);
                    NumberFormat = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                    var region = new RegionInfo(cultureInfo.LCID);
                    EnglishName = region.CurrencyEnglishName;

                    if (_code != null)
                    {
                        Symbol = CurrencySymbolFromCodeIso(_code) ?? "N/A";
                        NumberFormat.CurrencySymbol = Symbol;
                    }
                }
                else
                {
                    NumberFormat = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
                }
            }
        }

        private static string CurrencySymbolFromCodeIso(string isoCode)
        {
            foreach (var ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var ri = new RegionInfo(ci.LCID);
                if (ri.ISOCurrencySymbol == isoCode)
                    return ri.CurrencySymbol;
            }
            return null;
        }


        public static bool operator ==(Currency left, Currency right)
        {
            return Equals(left, null)
                ? Equals(right, null)
                : left.Equals(right);
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

            if (ReferenceEquals(this, obj))
                return true;

            var other = obj as Currency;
            var code = obj as string;

            if (other != null)
            {
                return string.Equals(Code, other.Code, StringComparison.InvariantCultureIgnoreCase);
            }

            if (code != null)
            {
                return string.Equals(Code, code, StringComparison.InvariantCultureIgnoreCase);
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
