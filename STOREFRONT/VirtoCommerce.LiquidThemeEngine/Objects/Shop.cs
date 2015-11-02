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
        private readonly string _currency;
        private readonly string _language;
        private readonly IStorefrontUrlBuilder _urlBuilder;
        public Shop(Store store, IStorefrontUrlBuilder urlBuilder, string currency, string language)
        {
            _store = store;
            _currency = currency;
            _language = language;
            _urlBuilder = urlBuilder;
        }

        /// <summary>
        /// Returns the shop's currency in three-letter format (ex: USD).
        /// </summary>
        public string Currency
        {
            get
            {
                return _currency;
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
                if (_currency.Equals("GBP", StringComparison.OrdinalIgnoreCase)
                   || _currency.Equals("USD", StringComparison.OrdinalIgnoreCase))
                {
                    return  _currency.GetCurrencySymbol() + "{{ amount }}";
                }
                else
                {
                   return  "{{ amount }} " + _currency.GetCurrencySymbol();
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
                return String.IsNullOrEmpty(_store.Url) ? _urlBuilder.ToAbsolute("~/", _store.Id, _language) : _store.Url;
            }
        }

        /// <summary>
        /// All shop currencies
        /// </summary>
        public string[] Currencies
        {
            get
            {
                return _store.Currencies.ToArray();
            }
        }

        /// <summary>
        /// All shop languages
        /// </summary>
        public string[] Languages
        {
            get
            {
                return _store.Languages.ToArray();
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
    }
}
