using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/shop
    /// </summary>
    public class Shop : Drop
    {
        private readonly Store _store;
        private readonly IStorefrontUrlBuilder _urlBuilder;
        private readonly WorkContext _context;
        public Shop(Store store, IStorefrontUrlBuilder urlBuilder, WorkContext context)
        {
            _store = store;
            _urlBuilder = urlBuilder;
            _context = context;

            CustomerAccountsEnabled = true;
            CustomerAccountsOptional = true;
        }

        /// <summary>
        /// Returns the shop's currency in three-letter format (ex: USD).
        /// </summary>
        public string Currency
        {
            get
            {
                return _context.CurrentCurrency.Code;
            }
        }

        /// <summary>
        /// Returns the number of collections in a shop.
        /// </summary>
        public string CollectionsCount
        {
            get
            {
                return "0";
            }
        }

        /// <summary>
        /// Returns the description of the shop.
        /// </summary>
        public string Description
        {
            get
            {
                return _store.Description;
            }
        }

        /// <summary>
        /// Returns the primary domain of the shop.
        /// </summary>
        public string Domain
        {
            get
            {
                return _store.Url;
            }
        }

        /// <summary>
        /// Returns the shop's email address.
        /// </summary>
        public string Email
        {
            get
            {
                return _store.Email;
            }
        }

        /// <summary>
        /// Returns a string that is used by Shopify to format money without showing the currency.
        /// </summary>
        public string MoneyFormat
        {
            get
            {
                if (Currency.Equals("GBP", StringComparison.OrdinalIgnoreCase)
                   || Currency.Equals("USD", StringComparison.OrdinalIgnoreCase))
                {
                    return _context.CurrentCurrency.Symbol + "{{ amount }}";
                }
                else
                {
                    return "{{ amount }} " + _context.CurrentCurrency.Symbol;
                }
            }
        }

        /// <summary>
        /// Returns a string that is used by Shopify to format money while also displaying the currency.
        /// </summary>
        public string MoneyWithCurrencyFormat
        {
            get
            {
                return MoneyFormat;
            }
        }

        /// <summary>
        /// Returns the shop's name.
        /// </summary>
        public string Name
        {
            get
            {
                return _store.Name;
            }
        }

        /// <summary>
        /// Returns the full URL of a shop.
        /// </summary>
        public string Url
        {
            get
            {
                return string.IsNullOrEmpty(_store.Url) ? _urlBuilder.ToAbsolute(_context, "~/", _store, _context.CurrentLanguage) : _store.Url;
            }
        }

        public string SimplifiedUrl
        {
            get
            {
                return Url;
            }
        }

        /// <summary>
        /// All shop currencies
        /// </summary>
        public string[] Currencies
        {
            get
            {
                return _store.Currencies.Select(x => x.Code).ToArray();
            }
        }

        /// <summary>
        /// All shop languages
        /// </summary>
        public string[] Languages
        {
            get
            {
                return _store.Languages.Select(x => x.CultureName).ToArray();
            }
        }

        /// <summary>
        /// Shop catalog
        /// </summary>
        public string Catalog
        {
            get
            {
                return _store.Catalog;
            }
        }

        /// <summary>
        /// Shop metafields
        /// </summary>
        public MetaFieldNamespacesCollection Metafields
        {
            get
            {
                var fieldsCollection = new MetafieldsCollection("global", _store.DynamicProperties);
                return new MetaFieldNamespacesCollection(new[] { fieldsCollection });

            }
        }

        public bool CustomerAccountsEnabled { get; set; }
        public bool CustomerAccountsOptional { get; set; }
    }
}
