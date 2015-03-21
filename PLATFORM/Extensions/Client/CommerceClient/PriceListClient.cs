using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Tagging;

namespace VirtoCommerce.Client
{
    public class PriceListClient
    {
        #region Cache Constants
        public const string CatalogCacheKey = "C:C:{0}";
        public const string CategoryCacheKey = "C:CT:{0}:{1}";
        public const string ChildCategoriesCacheKey = "C:CT:{0}:p:{1}";
        //public const string ItemCacheKey = "C:I:{0}:g:{1}";
        public const string ItemsCacheKey = "C:Is:{0}";
        public const string ItemsSearchCacheKey = "C:Is:{0}:{1}";
        public const string ItemsQueryCacheKey = "C:Is:{0}";
        public const string PriceListCacheKey = "C:PL:{0}:{1}";

        public const string PricesCacheKey = "C:P:{0}";
        public const string ItemPricesCacheKey = "C:P:{0}:{1}";
        public const string PricelistAssignmentCacheKey = "C:PLA:{0}";

        public const string PropertiesCacheKey = "C:PR:{0}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        private readonly IPricelistRepository _priceListRepository;
        private readonly ICustomerSessionService _customerSession;
        private readonly IPriceListAssignmentEvaluationContext _priceListEvalContext;
        private readonly IPriceListAssignmentEvaluator _priceListEvaluator;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceListClient" /> class.
        /// </summary>
        /// <param name="priceListRepository">The price list repository.</param>
        /// <param name="customerSession">The customer session.</param>
        /// <param name="priceListEvaluator">The price list evaluator.</param>
        /// <param name="priceListEvalContext">The price list eval context.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public PriceListClient(IPricelistRepository priceListRepository, ICustomerSessionService customerSession, IPriceListAssignmentEvaluator priceListEvaluator, IPriceListAssignmentEvaluationContext priceListEvalContext, ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
            _priceListRepository = priceListRepository;
            _customerSession = customerSession;
            _priceListEvalContext = priceListEvalContext;
            _priceListEvaluator = priceListEvaluator;
            _isEnabled = CatalogConfiguration.Instance.Cache.IsEnabled;
        }

        /// <summary>
        /// Gets the price list stack in the order it should be applied using parameters specified.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public string[] GetPriceListStack(string catalogId, string currency, TagSet tags, bool useCache = true)
        {
            var lists = GetPriceListStackInternal(catalogId, currency, tags);
            return lists == null ? null : lists.Select(y => y.PricelistId).ToArray();
        }

        private IEnumerable<Pricelist> GetPriceListStackInternal(string catalogId, string currency, TagSet tags)
        {
            var evaluateContext = _priceListEvalContext;

            evaluateContext.Currency = currency;
            evaluateContext.CatalogId = catalogId;
            evaluateContext.ContextObject = tags;
	        evaluateContext.CurrentDate = DateTime.Now;

            var evaluator = _priceListEvaluator;

            var lists = evaluator.Evaluate(evaluateContext);

            return lists;
        }

        /// <summary>
        /// Gets the lowest price.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public Price GetLowestPrice(string itemId, decimal quantity, bool useCache = true)
        {
            var session = _customerSession.CustomerSession;
            var prices = GetLowestPrices(session.Pricelists, new[] { itemId }, quantity, useCache);

            if (prices != null && prices.Any())
            {
                return prices[0]; // since prices already ordered, return the top one
            }

            return null;
        }

        /// <summary>
        /// Gets the item prices.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public Price[] GetItemPrices(string itemId, decimal quantity, bool useCache = true)
        {
            var session = _customerSession.CustomerSession;
            return _priceListRepository.GetItemPrices(session.Pricelists, itemId, quantity);
        }

        /// <summary>
        /// Gets the lowest prices.
        /// </summary>
        /// <param name="priceLists">The price lists.</param>
        /// <param name="itemIds">The item ids.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public Price[] GetLowestPrices(string[] priceLists, string[] itemIds, decimal quantity, bool useCache = true)
        {
            // no price lists, no prices
            if (priceLists == null || priceLists.Length == 0)
                return null;

            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.PricelistCachePrefix, string.Format(ItemPricesCacheKey, CacheHelper.CreateCacheKey(priceLists), CacheHelper.CreateCacheKey(itemIds))),
                () => (_priceListRepository.FindLowestPrices(priceLists, itemIds, quantity)),
                CatalogConfiguration.Instance.Cache.PricesTimeout,
                _isEnabled && useCache);
        }

        CacheHelper _cacheHelper;
        /// <summary>
        /// Gets the helper.
        /// </summary>
        /// <value>
        /// The helper.
        /// </value>
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
