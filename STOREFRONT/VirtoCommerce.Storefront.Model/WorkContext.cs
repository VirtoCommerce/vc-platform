using System;
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
        public int CurrentPage { get; set; }
        public string ErrorMessage { get; set; }

        #region Catalog Properties

        public Product CurrentProduct { get; set; }

        public Category CurrentCategory { get; set; }

        public Category[] AllCategories { get; set; }

        #endregion

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
