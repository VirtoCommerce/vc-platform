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
                return new Shop(CurrentStore, _urlBuilder, this);
            }
        }

        /// <summary>
        /// Current single form value  created in DotLiquidThemedView with ModelState errors
        /// The form object is used within the form tag. It contains attributes of its parent form.
        /// </summary>
        public Objects.Form Form
        {
            get; set;
        }

        public Product Product
        {
            get
            {
                return new Product(CurrentProduct, _urlBuilder, this);
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
