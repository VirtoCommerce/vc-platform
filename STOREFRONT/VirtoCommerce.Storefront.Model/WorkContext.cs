using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Order;
using VirtoCommerce.Storefront.Model.Pricing;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Main working context contains all data which could be used in presentation logic
    /// </summary>
    public class WorkContext : IDisposable
    {
        public WorkContext()
        {
            CurrentPricelists = new List<Pricelist>();
            CurrentLinkLists = new List<MenuLinkList>();
            ExternalLoginProviders = new List<LoginProvider>();
            ApplicationSettings = new Dictionary<string, object>();
        }
        /// <summary>
        /// Current request url example: http:/host/app/store/en-us/search?page=2
        /// </summary>
        public Uri RequestUrl { get; set; }

        public Login Login { get; set; }
        /// <summary>
        /// Current customer
        /// </summary>
        public CustomerInfo CurrentCustomer { get; set; }

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
                    _seoInfo = CurrentStore.CurrentSeoInfo;
                }
                return _seoInfo;
            }
            set
            {
                _seoInfo = value;
            }
        }
        /// <summary>
        /// Represent current account orders search criteria taken from request url
        /// </summary>
        public OrderSearchCriteria CurrentOrderSearchCriteria { get; set; }

        /// <summary>
        /// Current store
        /// </summary>
        public Store CurrentStore { get; set; }

        /// <summary>
        /// Gets or sets the current shopping cart
        /// </summary>
        public ShoppingCart CurrentCart { get; set; }

        /// <summary>
        /// Represent current quotes search criteria taken from request url
        /// </summary>
        public QuoteSearchCriteria CurrentQuoteSearchCriteria { get; set; }

        public QuoteRequest CurrentQuoteRequest { get; set; }

        /// <summary>
        /// Gets or sets the HTML code for payment method prepared form
        /// </summary>
        public string PaymentFormHtml { get; set; }

        /// <summary>
        /// Gets or sets the collection of site navigation menu link lists
        /// </summary>
        public ICollection<MenuLinkList> CurrentLinkLists { get; set; }

        /// <summary>
        /// List of all supported stores
        /// </summary>
        public Store[] AllStores { get; set; }

        /// <summary>
        /// List of all active system currencies
        /// </summary>
        public IEnumerable<Currency> AllCurrencies { get; set; }

        public string ErrorMessage { get; set; }
        /// <summary>
        /// List of active pricelists
        /// </summary>
        public ICollection<Pricelist> CurrentPricelists { get; set; }

        #region Catalog Properties
        /// <summary>
        /// Represent current product
        /// </summary>
        public Product CurrentProduct { get; set; }
        /// <summary>
        /// Represent current category
        /// </summary>
        public Category CurrentCategory { get; set; }
        /// <summary>
        /// Represent all store catalog categories filtered by current search criteria CurrentCatalogSearchCriteria (loaded on first access by lazy loading)
        /// </summary>
        public IMutablePagedList<Category> Categories { get; set; }
        /// <summary>
        /// Represent products filtered by current search criteria CurrentCatalogSearchCriteria (loaded on first access by lazy loading)
        /// </summary>
        public IMutablePagedList<Product> Products { get; set; }
        /// <summary>
        /// Represent bucket, aggregated data based on a search query resulted by current search criteria CurrentCatalogSearchCriteria (example  color 33, gr
        /// </summary>
        public IMutablePagedList<Aggregation> Aggregations { get; set; }

        /// <summary>
        /// Current search catalog criterias
        /// </summary>
        public CatalogSearchCriteria CurrentCatalogSearchCriteria { get; set; }

        #endregion

        #region Static Content Properties
        public ContentPage CurrentPage { get; set; }

        public BlogSearchCriteria CurrentBlogSearchCritera { get; set; }

        public Blog CurrentBlog { get; set; }

        public BlogArticle CurrentBlogArticle { get; set; }
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

        public Country[] AllCountries { get; set; }

        public CustomerOrder CurrentOrder { get; set; }


        public ContactUsForm ContactUsForm { get; set; }

        public StorefrontNotification StorefrontNotification { get; set; }

        /// <summary>
        /// All static content items (Pages, blog articles etc) for current store and theme
        /// </summary>
        public IMutablePagedList<ContentItem> Pages { get; set; }

        /// <summary>
        ///  All blogs with articles for current store and theme
        /// </summary>
        public IMutablePagedList<Blog> Blogs { get; set; }

        /// <summary>
        /// Gets or sets the collection of external login providers
        /// </summary>
        public ICollection<LoginProvider> ExternalLoginProviders { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of application settings
        /// </summary>
        public IDictionary<string, object> ApplicationSettings { get; set; }

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
