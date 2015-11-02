using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DotLiquid;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects
    /// </summary>
    public class ShopifyThemeWorkContext : WorkContext, ILiquidizable
    {
        private readonly IStorefrontUrlBuilder _urlBuilder;
        public ShopifyThemeWorkContext(IStorefrontUrlBuilder urlBuilder)
        {
            _urlBuilder = urlBuilder;
        }
        #region Aliases for shopify theme compliant
      
        /// <summary>
        /// Merchants can specify a page_description.
        /// </summary>
        public string PageDescription
        {
            get
            {
                return CurrentPageSeo != null ? CurrentPageSeo.MetaDescription : String.Empty;
            }
        }
        /// <summary>
        /// The liquid object page_title returns the title of the current page.
        /// </summary>
        public string PageTitle
        {
            get
            {
                return CurrentPageSeo != null ? CurrentPageSeo.Title : String.Empty;
            }
        }
        /// <summary>
        /// The liquid object shop returns information about your shop
        /// </summary>
        public Shop Shop
        {
            get
            {
                return new Shop(CurrentStore, _urlBuilder, CurrentCurrency, CurrentLanguage);
            }
        }
        #endregion

        #region ILiquidizable members
        public object ToLiquid()
        {
            var retVal = new Dictionary<string, object>();
            foreach (var propertyInfo in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                retVal.Add(propertyInfo.Name.Decamelize(), propertyInfo.GetValue(this));
            }

            return retVal;
        }
        #endregion
    }
}
