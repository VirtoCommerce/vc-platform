using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Main working context contains all data which could be used in presentation logic
    /// </summary>
    public class WorkContext : IDisposable
    {
        /// <summary>
        /// Current request url example: http:/host/app/store/en-us/search?page=2
        /// </summary>
        public Uri RequestUrl { get; set; }

        public Login Login { get; set; }
        /// <summary>
        /// Current customer
        /// </summary>
        public Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Current language and culture
        /// </summary>
        public Language CurrentLanguage { get; set; }

        /// <summary>
        /// Current currency
        /// </summary>
        public Currency CurrentCurrency { get; set; }

        private SeoInfo _seoInfo;
        public SeoInfo CurrentPageSeo
        {
            get
            {
                if (_seoInfo == null)
                {
                    //TODO: next need detec seo from category or product or cart etc
                    _seoInfo = CurrentStore.SeoInfos.FirstOrDefault();
                }
                return _seoInfo;
            }
            set
            {
                _seoInfo = value;
            }
        }

        /// <summary>
        /// Current store
        /// </summary>
        public Store CurrentStore { get; set; }

        /// <summary>
        /// Current shopping cart
        /// </summary>
        public ShoppingCart CurrentCart { get; set; }

        /// <summary>
        /// List of all supported stores
        /// </summary>
        public Store[] AllStores { get; set; }
        public string ErrorMessage { get; set; }

        #region Catalog Properties
        /// <summary>
        /// Represent current product
        /// </summary>
        public Product CurrentProduct { get; set; }

        public Category CurrentCategory { get; set; }

        /// <summary>
        /// Current search catalog criterias
        /// </summary>
        public CatalogSearchCriteria CurrentCatalogSearchCriteria { get; set; }

        public CatalogSearchResult CurrentCatalogSearchResult { get; set; }
        #endregion

        private DateTime? _utcNow;
        /// <summary>
        /// Represent current storefront datetime in UTC (may be changes to test in future or past events)
        /// </summary>
        public DateTime StorefrontUtcNow
        {
            get
            {
                return _utcNow == null ? DateTime.UtcNow : _utcNow.Value;
            }
            set
            {
                _utcNow = value;
            }
        }

        public ICollection<Country> Countries { get; set; }

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        #endregion
    }
}
