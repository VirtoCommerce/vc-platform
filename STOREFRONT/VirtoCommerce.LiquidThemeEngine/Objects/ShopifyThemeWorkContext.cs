using System;
using System.Collections.Generic;
using System.Reflection;
using DotLiquid;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects
    /// </summary>
    public class ShopifyThemeWorkContext : ILiquidizable
    {
        #region Aliases for shopify theme compliance

        /// <summary>
        /// Merchants can specify a page_description.
        /// </summary>
        public string PageDescription { get; set; }

        /// <summary>
        /// The liquid object page_title returns the title of the current page.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// The liquid object shop returns information about your shop
        /// </summary>
        public Shop Shop { get; set; }

        /// <summary>
        /// The liquid object cart returns information about customer shopping cart
        /// </summary>
        public Cart Cart { get; set; }

        /// <summary>
        /// Current single form value  created in DotLiquidThemedView with ModelState errors
        /// The form object is used within the form tag. It contains attributes of its parent form.
        /// </summary>
        public Form Form { get; set; }

        /// <summary>
        /// Contains a collection of all of the links in your shop.
        /// You can access a linklist by calling its handle on linklists
        /// </summary>
        public Linklists Linklists { get; set; }

        /// <summary>
        /// Contains a collection of all pages
        /// </summary>
        public Pages Pages { get; set; }

        /// <summary>
        /// Contains collection of all blogs
        /// </summary>
        public Blogs Blogs { get; set; }

        public Product Product { get; set; }

        public Page Page { get; set; }

        public Blog Blog { get; set; }
        /// <summary>
        /// Current blog article
        /// </summary>
        public Article Article { get; set; }
        /// <summary>
        /// HTML code for payment method prepared form
        /// </summary>
        public string PaymentFormHtml { get; set; }

        /// <summary>
        /// Returns logged in customer or null.
        /// </summary>
        public Customer Customer { get; set; }

        public string CountryOptionTags { get; set; }

        /// <summary>
        /// The collection https://docs.shopify.com/themes/liquid-documentation/objects/collection
        /// </summary>
        public Collection Collection { get; set; }

        public Collections Collections { get; set; }

        public int CurrentPage { get; set; }

        public TagCollection CurrentTags { get; set; }

        public string PoweredByLink { get; set; }

        #region Custom properties
        public Language CurrentLanguage { get; set; }

        public Currency CurrentCurrency { get; set; }

        public Shop[] AllStores { get; set; }

        /// <summary>
        /// Current request url
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// Search result (products, articles, blogs)
        /// </summary>
        public Search Search { get; set; }
        #endregion
        /// <summary>
        /// template returns the name of the template used to render the current page, with the .liquid extension omitted.
        /// </summary>
        public string Template { get; set; }

        public Notification Notification { get; set; }

        public Order Order { get; set; }

        public QuoteRequest QuoteRequest { get; set; }

        public ICollection<LoginProvider> ExternalLoginProviders { get; set; }

        public MetafieldsCollection ApplicationSettings { get; set; }
        #endregion

        #region ILiquidizable members

        public object ToLiquid()
        {
            var retVal = new Dictionary<string, object>();
            foreach (var propertyInfo in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                retVal.Add(propertyInfo.Name.Decamelize(), propertyInfo.GetValue(this));
            }

            return retVal;
        }

        #endregion
    }
}
