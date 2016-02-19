using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents the shop object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/shop
    /// </remarks>
    [DataContract]
    public class Shop : Drop
    {
        /// <summary>
        /// Shop id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Returns the shop's currency in three-letter format (ex: USD).
        /// </summary>
        [DataMember]
        public string Currency { get; set; }

        /// <summary>
        /// Returns the number of collections in a shop.
        /// </summary>
        [DataMember]
        public string CollectionsCount { get; set; }

        /// <summary>
        /// Returns the description of the shop.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Returns the primary domain of the shop.
        /// </summary>
        [DataMember]
        public string Domain { get; set; }

        /// <summary>
        /// Returns the shop's email address.
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Returns a string that is used by Shopify to format money without showing the currency.
        /// </summary>
        [DataMember]
        public string MoneyFormat { get; set; }

        /// <summary>
        /// Returns a string that is used by Shopify to format money while also displaying the currency.
        /// </summary>
        [DataMember]
        public string MoneyWithCurrencyFormat { get; set; }

        /// <summary>
        /// Returns the shop's name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Returns the full URL of a shop.
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// All shop currencies
        /// </summary>
        [DataMember]
        public string[] Currencies { get; set; }

        /// <summary>
        /// All shop languages
        /// </summary>
        public Language[] Languages { get; set; }

        /// <summary>
        /// Shop catalog
        /// </summary>
        [DataMember]
        public string Catalog { get; set; }

        [DataMember]
        public bool QuotesEnabled { get; set; }

        /// <summary>
        /// Shop metafields
        /// </summary>
        public MetaFieldNamespacesCollection Metafields { get; set; }

        public bool CustomerAccountsEnabled { get; set; }

        public bool CustomerAccountsOptional { get; set; }

        public string Status { get; set; }
    }
}