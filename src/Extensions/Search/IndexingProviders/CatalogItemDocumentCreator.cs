using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Search.Index
{
    using VirtoCommerce.Foundation.Catalogs.Services;

    public class CatalogItemDocumentCreator : ISearchIndexDocumentCreator<Partition>
    {

        #region Cache Constants
        public const string PropertiesCacheKey = "C:PR:{0}";
        public const string PriceListsCacheKey = "C:PL:{0}";
        #endregion

        private Price[] _prices;
        private Pricelist[] _priceLists;
        private Property[] _properties;

        /// <summary>
        /// Gets the catalog repository.
        /// </summary>
        /// <value>
        /// The catalog repository.
        /// </value>
        public ICatalogRepository CatalogRepository { get; private set; }

        /// <summary>
        /// Gets the price list repository.
        /// </summary>
        /// <value>
        /// The price list repository.
        /// </value>
        public IPricelistRepository PriceListRepository { get; private set; }

        /// <summary>
        /// Gets the outline builder.
        /// </summary>
        /// <value>
        /// The outline builder.
        /// </value>
        public ICatalogOutlineBuilder OutlineBuilder { get; private set; }

        /// <summary>
        /// Gets the review repository.
        /// </summary>
        /// <value>
        /// The review repository.
        /// </value>
        public IReviewRepository ReviewRepository { get; private set; }

        /// <summary>
        /// Gets the cache repository.
        /// </summary>
        /// <value>
        /// The cache repository.
        /// </value>
        public ICacheRepository CacheRepository { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemDocumentCreator" /> class.
        /// </summary>
        /// <param name="catalogRepository">The catalog repository.</param>
        /// <param name="pricelistRepository">The pricelist repository.</param>
        /// <param name="catalogOutlinebuilder">The catalog outline builder.</param>
        /// <param name="reviewRepository"></param>
        /// <param name="cacheRepository">The cache repository.</param>
        public CatalogItemDocumentCreator(ICatalogRepository catalogRepository, IPricelistRepository pricelistRepository, ICatalogOutlineBuilder catalogOutlinebuilder, IReviewRepository reviewRepository, ICacheRepository cacheRepository)
        {
            ReviewRepository = reviewRepository;
            CatalogRepository = catalogRepository;
            PriceListRepository = pricelistRepository;
            CacheRepository = cacheRepository;
            OutlineBuilder = catalogOutlinebuilder;
        }

        /// <summary>
        /// Creates the document from the partition source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// source.Keys
        /// </exception>
        public IEnumerable<IDocument> CreateDocument(Partition source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Keys == null)
                throw new ArgumentNullException("source.Keys");


            var index = 0;
            Trace.TraceInformation(String.Format("Processing documents starting {0} of {1} - {2}%", source.Start, source.Total, (source.Start * 100 / source.Total)));
            foreach (var item in LoadItems(source.JobId, source.Keys))
            {
                var doc = new ResultDocument();
                IndexItem(ref doc, item);
                yield return doc;
                index++;
            }
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        protected virtual IQueryable<Item> LoadItems(string jobId, string[] items)
        {
            // preload prices
            var prices = (from p in PriceListRepository.Prices where items.Contains(p.ItemId) select p).AsNoTracking();
            _prices = prices.ToArray();

            // preload price lists
            _priceLists = GetPriceLists(jobId);

            // preload properties
            _properties = GetProperties(jobId);

            var query = from i in CatalogRepository.Items where items.Contains(i.ItemId) select i;
            query = query.Expand("CategoryItemRelations.Category.LinkedCategories");
            query = query.Expand(p => p.EditorialReviews);
            query = query.Expand(p => p.ItemPropertyValues);
            return query.AsNoTracking();
        }

        protected virtual void IndexItem(ref ResultDocument doc, Item item)
        {
            doc.Add(new DocumentField("__key", item.ItemId.ToLower(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            //doc.Add(new DocumentField("__loc", "en-us", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__type", item.GetType().Name, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__sort", item.Name, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__hidden", (!item.IsActive).ToString().ToLower(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("code", item.Code, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("name", item.Name, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("startdate", item.StartDate, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("enddate", item.EndDate.HasValue ? item.EndDate : DateTime.MaxValue, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("createddate", item.Created.HasValue ? item.Created : DateTime.MaxValue, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("lastmodifieddate", item.LastModified.HasValue ? item.LastModified : DateTime.MaxValue, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("catalog", item.CatalogId.ToLower(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED, IndexDataType.StringCollection }));
            doc.Add(new DocumentField("__outline", item.CatalogId.ToLower(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED, IndexDataType.StringCollection }));

            // Index categories
            IndexItemCategories(ref doc, item);

            // Index custom properties
            IndexItemCustomProperties(ref doc, item);

            // Index item prices
            IndexItemPrices(ref doc, item);

            //Index item reviews
            IndexReviews(ref doc, item);

            // add to content
            doc.Add(new DocumentField("__content", item.Name, new[] { IndexStore.YES, IndexType.ANALYZED, IndexDataType.StringCollection }));
            doc.Add(new DocumentField("__content", item.Code, new[] { IndexStore.YES, IndexType.ANALYZED, IndexDataType.StringCollection }));
        }

        protected virtual void IndexItemCustomProperties(ref ResultDocument doc, Item item)
        {
            foreach (var val in item.ItemPropertyValues)
            {
                var key = val.Name;

                PropertyValueBase indexVal = val;

                var prop = _properties.FirstOrDefault(p => p.Name == val.Name && p.CatalogId == item.CatalogId);

                //Handle dictionary value
                if (prop != null && prop.IsEnum)
                {
                    var dictVal = prop.PropertyValues.FirstOrDefault(p => p.PropertyValueId == val.KeyValue);
                    if (dictVal != null)
                    {
                        indexVal = dictVal;
                    }
                    else
                    {
                        break;
                    }
                }

                var contentField = string.Format("__content{0}",
                    prop != null && (prop.IsLocaleDependant && !string.IsNullOrWhiteSpace(indexVal.Locale)) ? "_" + indexVal.Locale.ToLower() : string.Empty);

                switch ((PropertyValueType)indexVal.ValueType)
                {

                    case PropertyValueType.LongString:
                        doc.Add(new DocumentField(contentField, ConvertToLowcase(indexVal.LongTextValue), new[] { IndexStore.YES, IndexType.ANALYZED, IndexDataType.StringCollection }));
                        break;
                    case PropertyValueType.ShortString:
                        doc.Add(new DocumentField(contentField, ConvertToLowcase(indexVal.ShortTextValue), new[] { IndexStore.YES, IndexType.ANALYZED, IndexDataType.StringCollection }));
                        break;
                }


                if (doc.ContainsKey(key))
                    continue;


                switch ((PropertyValueType)indexVal.ValueType)
                {
                    case PropertyValueType.Boolean:
                        doc.Add(new DocumentField(key, indexVal.BooleanValue, new[] { IndexStore.YES, IndexType.ANALYZED }));
                        break;
                    case PropertyValueType.DateTime:
                        doc.Add(new DocumentField(key, indexVal.DateTimeValue, new[] { IndexStore.YES, IndexType.ANALYZED }));
                        break;
                    case PropertyValueType.Decimal:
                        doc.Add(new DocumentField(key, indexVal.DecimalValue, new[] { IndexStore.YES, IndexType.ANALYZED }));
                        break;
                    case PropertyValueType.Integer:
                        doc.Add(new DocumentField(key, indexVal.IntegerValue, new[] { IndexStore.YES, IndexType.ANALYZED }));
                        break;
                    case PropertyValueType.LongString:
                        doc.Add(new DocumentField(key, ConvertToLowcase(indexVal.LongTextValue), new[] { IndexStore.YES, IndexType.ANALYZED }));
                        break;
                    case PropertyValueType.ShortString:
                        doc.Add(new DocumentField(key, ConvertToLowcase(indexVal.ShortTextValue), new[] { IndexStore.YES, IndexType.ANALYZED }));
                        break;
                }
            }
        }

        private string ConvertToLowcase(string input)
        {
            return String.IsNullOrEmpty(input) ? input : input.ToLower();
        }

        #region Category Indexing
        protected virtual void IndexItemCategories(ref ResultDocument doc, Item item)
        {
            if (item.CategoryItemRelations == null)
                return;

            foreach (var cat in item.CategoryItemRelations)
            {
                doc.Add(new DocumentField(String.Format("sort{0}{1}", cat.CatalogId, cat.CategoryId), cat.Priority, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));

                IndexCategory(ref doc, cat);
            }
        }

        protected virtual void IndexCategory(ref ResultDocument doc, CategoryItemRelation categoryRelation)
        {
            //TODO: normally categoryRelation.Category should no be null but somehow it is null sometimes after more than 300 item loads
            var cat = categoryRelation.Category ??
                CatalogRepository.Categories.Expand(c => c.LinkedCategories)
                .First(c => c.CategoryId == categoryRelation.CategoryId);

            IndexCategory(ref doc, categoryRelation.CatalogId, cat);
        }

        protected virtual void IndexCategory(ref ResultDocument doc, string catalogId, CategoryBase category)
        {
            doc.Add(new DocumentField("catalog", catalogId.ToLower(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));

            // get category path
            var outline = OutlineBuilder.BuildCategoryOutline(catalogId, category).ToString();
            doc.Add(new DocumentField("__outline", outline.ToLower(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));

            // Now index all linked categories
            foreach (var linkedCategory in category.LinkedCategories)
            {
                IndexCategory(ref doc, linkedCategory.CatalogId, linkedCategory);
            }
        }
        #endregion

        #region Price Lists Indexing
        protected virtual void IndexItemPrices(ref ResultDocument doc, Item item)
        {
            if (_prices != null)
            {
                var prices = (from p in _prices where p.ItemId.Equals(item.ItemId, StringComparison.OrdinalIgnoreCase) select p).ToArray();

                foreach (var price in prices)
                {
                    //var priceList = price.Pricelist;
                    var priceList = (from p in _priceLists where p.PricelistId == price.PricelistId select p).SingleOrDefault();
                    doc.Add(new DocumentField(String.Format("price_{0}_{1}", priceList.Currency, priceList.PricelistId), price.Sale ?? price.List, new[] { IndexStore.NO, IndexType.NOT_ANALYZED }));
                    doc.Add(new DocumentField(String.Format("price_{0}_{1}_value", priceList.Currency, priceList.PricelistId), price.Sale == null ? price.List.ToString() : price.Sale.ToString(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
                }
            }
        }
        #endregion

        protected virtual void IndexReviews(ref ResultDocument doc, Item item)
        {
            var reviews = ReviewRepository.Reviews.Where(r => r.ItemId == item.ItemId).ToArray();
            var count = reviews.Count();
            var avg = count > 0 ? Math.Round(reviews.Average(r => r.OverallRating), 2) : 0;
            doc.Add(new DocumentField("__reviewstotal", count, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__reviewsavg", avg, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
        }

        #region cache helper methods

        private Pricelist[] GetPriceLists(string jobId, bool useCache = true)
        {
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.PricelistCachePrefix, string.Format(PriceListsCacheKey, jobId)),
                () => ((from p in PriceListRepository.Pricelists select p).AsNoTracking().ToArray()),
                new TimeSpan(0, 5, 0),
                useCache);
        }

        private Property[] GetProperties(string jobId, bool useCache = true)
        {
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix, string.Format(PropertiesCacheKey, jobId)),
                () => ((from p in CatalogRepository.Properties select p).Expand(p => p.PropertyValues).AsNoTracking().ToArray()),
                new TimeSpan(0, 5, 0),
                useCache);
        }

        CacheHelper _CacheHelper = null;
        /// <summary>
        /// Gets the helper.
        /// </summary>
        /// <value>
        /// The helper.
        /// </value>
        private CacheHelper Helper
        {
            get
            {
                if (_CacheHelper == null)
                    _CacheHelper = new CacheHelper(CacheRepository);

                return _CacheHelper;
            }
        }
        #endregion
    }
}
