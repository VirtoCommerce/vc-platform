using System;
using System.Linq;
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
        public Customer Customer { get; set; }
     
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
     

        public Store CurrentStore { get; set; }

        /// <summary>
        /// List of all supported stores
        /// </summary>
        public Store[] AllStores { get; set; }
        public int CurrentPage { get; set; }
        
        /// <summary>
        /// List of categories
        /// </summary>
        public Category[] AllCategories { get; set; } 

        public Product CurrentProduct { get; set; }

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
