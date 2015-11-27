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
     
        /// <summary>
        /// Returns the shop's currency in three-letter format (ex: USD).
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Returns the number of collections in a shop.
        /// </summary>
        public string CollectionsCount { get; set; }

        /// <summary>
        /// Returns the description of the shop.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns the primary domain of the shop.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Returns the shop's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Returns a string that is used by Shopify to format money without showing the currency.
        /// </summary>
        public string MoneyFormat { get; set; }

        /// <summary>
        /// Returns a string that is used by Shopify to format money while also displaying the currency.
        /// </summary>
        public string MoneyWithCurrencyFormat { get; set; }

        /// <summary>
        /// Returns the shop's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Returns the full URL of a shop.
        /// </summary>
        public string Url { get; set; }

        public string SimplifiedUrl { get; set; }

        /// <summary>
        /// All shop currencies
        /// </summary>
        public string[] Currencies { get; set; }

        /// <summary>
        /// All shop languages
        /// </summary>
        public Language[] Languages { get; set; }

        /// <summary>
        /// Shop catalog
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Shop metafields
        /// </summary>
        public MetaFieldNamespacesCollection Metafields { get; set; }
     

        public bool CustomerAccountsEnabled { get; set; }
        public bool CustomerAccountsOptional { get; set; }
    }
}
