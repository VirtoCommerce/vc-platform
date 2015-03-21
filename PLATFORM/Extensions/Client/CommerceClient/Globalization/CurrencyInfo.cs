using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VirtoCommerce.Client.Globalization
{
    /// <summary>
    /// Class CurrencyInfo.
    /// </summary>
    public class CurrencyInfo
    {
        #region Fields
        /// <summary>
        /// The _currencies
        /// </summary>
        static List<CurrencyInfo> _currencies;
        /// <summary>
        /// The _lock
        /// </summary>
        static object _lock = new object();
        #endregion

        #region Methods
        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns>IEnumerable{CurrencyInfo}.</returns>
        public static IEnumerable<CurrencyInfo> GetCurrencies()
        {
            if (_currencies == null)
            {
                lock (_lock)
                {
                    if (_currencies == null)
                    {
                        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

                        var list = new List<CurrencyInfo>();
                        //loop through all the cultures found
                        foreach (var culture in cultures)
                        {
                            //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                            //to the RegionInfo constructor to gain access to the information for that culture
                            try
                            {
                                var region = new RegionInfo(culture.LCID);

                                if (list.Any(i => i.EnglishName == region.CurrencyEnglishName) == false)
                                {
                                    list.Add(new CurrencyInfo
                                    {
                                        EnglishName = region.CurrencyEnglishName,
                                        NativeName = region.CurrencyNativeName,
                                        Symbol = region.CurrencySymbol,
                                        ISOSymbol = region.ISOCurrencySymbol
                                    });
                                }
                            }
                            catch
                            {
                                //next 
                            }
                        }

                        _currencies = list;
                    }
                }
            }

            return _currencies;
        }

        /// <summary>
        /// Gets the currency info.
        /// </summary>
        /// <param name="englishName">Name of the english.</param>
        /// <returns>CurrencyInfo.</returns>
        public static CurrencyInfo GetCurrencyInfo(string englishName)
        {
            return GetCurrencies()
                .Where(i => i.EnglishName == englishName)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the currency info by ISO symbol.
        /// </summary>
        /// <param name="isoSymbol">The iso symbol.</param>
        /// <returns>CurrencyInfo.</returns>
        public static CurrencyInfo GetCurrencyInfoByISOSymbol(string isoSymbol)
        {
            return GetCurrencies()
              .Where(i => i.ISOSymbol == isoSymbol)
              .FirstOrDefault();
        }

        /// <summary>
        /// Gets or sets the name of the english.
        /// </summary>
        /// <value>The name of the english.</value>
        public string EnglishName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the native.
        /// </summary>
        /// <value>The name of the native.</value>
        public string NativeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ISO symbol.
        /// </summary>
        /// <value>The ISO symbol.</value>
        public string ISOSymbol
        {
            get;
            set;
        }
        #endregion
    }
}
