using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine
{
    public class ShopifyThemeWorkContext : WorkContext
    {
        #region Aliases for shopify theme compliant
        /// <summary>
        /// Merchants can specify a page_description.
        /// </summary>
        public string PageDescription
        {
            get
            {
                return CurrentPageSeo.MetaDescription;
            }
        }
        /// <summary>
        /// The liquid object page_title returns the title of the current page.
        /// </summary>
        public string PageTitle
        {
            get
            {
                return CurrentPageSeo.Title;
            }
        }
        /// <summary>
        /// The liquid object shop returns information about your shop
        /// </summary>
        public Store Shop
        {
            get
            {
                return CurrentStore;
            }
        }

        public Store[] Shops
        {
            get
            {
                return AllStores;
            }
        }

        #endregion
    }
}
