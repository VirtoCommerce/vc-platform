using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.SearchModule.Data.Services
{
    public class CatalogItemIndexBuilder : ISearchIndexBuilder
    {
        private const int _partitionSizeCount = 100; // the maximum partition size, keep it smaller to prevent too big of the sql requests and too large messages in the queue

        private readonly ISearchProvider _searchProvider;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly IPricingService _pricingService;
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;
        private readonly IChangeLogService _changeLogService;
        private readonly CacheManager _cacheManager;

        public CatalogItemIndexBuilder(ISearchProvider searchProvider, ICatalogSearchService catalogSearchService,
                                       IItemService itemService, IPricingService pricingService,
                                       ICategoryService categoryService, IPropertyService propertyService,
                                       IChangeLogService changeLogService, CacheManager cacheManager)
        {
            _searchProvider = searchProvider;
            _itemService = itemService;
            _catalogSearchService = catalogSearchService;
            _pricingService = pricingService;
            _categoryService = categoryService;
            _propertyService = propertyService;
            _changeLogService = changeLogService;
            _cacheManager = cacheManager;
        }

        #region ISearchIndexBuilder Members

        public string DocumentType
        {
            get
            {
                return CatalogIndexedSearchCriteria.DocType;
            }
        }

        public IEnumerable<Partition> GetPartitions(bool rebuild, DateTime startDate, DateTime endDate)
        {
            var partitions = (rebuild || startDate == DateTime.MinValue)
                ? GetPartitionsForAllProducts()
                : GetPartitionsForModifiedProducts(startDate, endDate);

            return partitions;
        }

        public IEnumerable<IDocument> CreateDocuments(Partition partition)
        {
            if (partition == null)
                throw new ArgumentNullException("partition");

            var documents = new ConcurrentBag<IDocument>();

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 5 };

            Parallel.ForEach(partition.Keys, parallelOptions, key =>
           {
               //Trace.TraceInformation(string.Format("Processing documents starting {0} of {1} - {2}%", partition.Start, partition.Total, (partition.Start * 100 / partition.Total)));
               var doc = new ResultDocument();
               IndexItem(ref doc, key);
               documents.Add(doc);
           });

            return documents;
        }

        public void PublishDocuments(string scope, IDocument[] documents)
        {
            foreach (var doc in documents)
            {
                _searchProvider.Index(scope, DocumentType, doc);
            }

            _searchProvider.Commit(scope);
            _searchProvider.Close(scope, DocumentType);
        }

        public void RemoveDocuments(string scope, string[] documents)
        {
            foreach (var doc in documents)
            {
                _searchProvider.Remove(scope, DocumentType, "__key", doc);
            }
            _searchProvider.Commit(scope);
        }

        public void RemoveAll(string scope)
        {
            _searchProvider.RemoveAll(scope, DocumentType);
        }

        #endregion

        protected virtual void IndexItem(ref ResultDocument doc, string productId)
        {
            var item = _itemService.GetById(productId, ItemResponseGroup.ItemProperties | ItemResponseGroup.Categories);

            doc.Add(new DocumentField("__key", item.Id.ToLower(), new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            //doc.Add(new DocumentField("__loc", "en-us", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__type", item.GetType().Name, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("__sort", item.Name, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("__hidden", (!item.IsActive.Value || item.MainProductId != null).ToString().ToLower(), new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("code", item.Code, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("name", item.Name, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("startdate", item.StartDate, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("enddate", item.EndDate.HasValue ? item.EndDate : DateTime.MaxValue, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("createddate", item.CreatedDate, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("lastmodifieddate", item.ModifiedDate ?? DateTime.MaxValue, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("catalog", item.CatalogId.ToLower(), new[] { IndexStore.Yes, IndexType.NotAnalyzed, IndexDataType.StringCollection }));
            doc.Add(new DocumentField("__outline", item.CatalogId.ToLower(), new[] { IndexStore.Yes, IndexType.NotAnalyzed, IndexDataType.StringCollection }));

            //Index item direct categories links
            if (item.Links != null)
            {
                foreach (var link in item.Links)
                {
                    var category = GetCategoryById(link.CategoryId);
                    if (category != null)
                    {
                        IndexCategory(ref doc, category);
                        foreach (var categoryLink in category.Links)
                        {
                            var linkCategory = GetCategoryById(categoryLink.CategoryId);
                            if (linkCategory != null)
                            {
                                IndexCategory(ref doc, linkCategory);
                            }
                        }
                    }
                }
            }

            // Index custom properties
            IndexItemCustomProperties(ref doc, item);

            // Index item prices
            IndexItemPrices(ref doc, item);

            //Index item reviews
            //IndexReviews(ref doc, item);

            // add to content
            doc.Add(new DocumentField("__content", item.Name, new[] { IndexStore.Yes, IndexType.Analyzed, IndexDataType.StringCollection }));
            doc.Add(new DocumentField("__content", item.Code, new[] { IndexStore.Yes, IndexType.Analyzed, IndexDataType.StringCollection }));
        }

        protected virtual void IndexItemCustomProperties(ref ResultDocument doc, CatalogProduct item)
        {
            foreach (var propValue in item.PropertyValues.Where(x => x.Value != null))
            {
                var properties = item.CategoryId != null ? _propertyService.GetCategoryProperties(item.CategoryId) : _propertyService.GetCatalogProperties(item.CatalogId);
                var property = properties.FirstOrDefault(x => string.Equals(x.Name, propValue.PropertyName, StringComparison.InvariantCultureIgnoreCase) && x.ValueType == propValue.ValueType);

                var contentField = string.Format("__content{0}", property != null && (property.Multilanguage && !string.IsNullOrWhiteSpace(propValue.LanguageCode)) ? "_" + propValue.LanguageCode.ToLower() : string.Empty);

                switch (propValue.ValueType)
                {
                    case PropertyValueType.LongText:
                    case PropertyValueType.ShortText:
                        doc.Add(new DocumentField(contentField, propValue.Value.ToString().ToLower(), new[] { IndexStore.Yes, IndexType.Analyzed, IndexDataType.StringCollection }));
                        break;
                }

                if (doc.ContainsKey(propValue.PropertyName))
                    continue;


                switch (propValue.ValueType)
                {
                    case PropertyValueType.Boolean:
                    case PropertyValueType.DateTime:
                    case PropertyValueType.Number:
                        doc.Add(new DocumentField(propValue.PropertyName, propValue.Value, new[] { IndexStore.Yes, IndexType.Analyzed }));
                        break;
                    case PropertyValueType.LongText:
                    case PropertyValueType.ShortText:
                        doc.Add(new DocumentField(propValue.PropertyName, propValue.Value.ToString().ToLower(), new[] { IndexStore.Yes, IndexType.Analyzed }));
                        break;
                }
            }
        }

        #region Category Indexing
        private Category GetCategoryById(string categoryId)
        {
            var cacheKey = CacheKey.Create("CatalogItemIndexBuilder.GetCategoryById", categoryId);
            var retVal = _cacheManager.Get(cacheKey, () => _categoryService.GetById(categoryId));
            return retVal;
        }


        protected virtual void IndexCategory(ref ResultDocument doc, Category category)
        {
            doc.Add(new DocumentField(string.Format("sort{0}{1}", category.CatalogId, category.Id), category.Priority, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));

            doc.Add(new DocumentField("catalog", category.CatalogId.ToLower(), new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            var outlineParts = new[] { category.CatalogId, category.Id }.Concat(category.Parents.Select(x => x.Id)).Where(x => !String.IsNullOrEmpty(x));
            // get category path
            var outline = string.Join("/", outlineParts);
            doc.Add(new DocumentField("__outline", outline.ToLower(), new[] { IndexStore.Yes, IndexType.NotAnalyzed }));

        }

        #endregion

        #region Price Lists Indexing

        protected virtual void IndexItemPrices(ref ResultDocument doc, CatalogProduct item)
        {
            var evalContext = new Domain.Pricing.Model.PriceEvaluationContext()
            {
                ProductIds = new[] { item.Id }
            };

            var prices = _pricingService.EvaluateProductPrices(evalContext);


            foreach (var price in prices)
            {
                //var priceList = price.Pricelist;
                doc.Add(new DocumentField(string.Format("price_{0}_{1}", price.Currency, price.PricelistId), price.EffectiveValue, new[] { IndexStore.No, IndexType.NotAnalyzed }));
                doc.Add(new DocumentField(string.Format("price_{0}_{1}_value", price.Currency, price.PricelistId), (price.EffectiveValue).ToString(CultureInfo.InvariantCulture), new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            }

        }

        #endregion

        //protected virtual void IndexReviews(ref ResultDocument doc, CatalogProduct item)
        //{
        //	var reviews = ReviewRepository.Reviews.Where(r => r.ItemId == item.ItemId).ToArray();
        //	var count = reviews.Count();
        //	var avg = count > 0 ? Math.Round(reviews.Average(r => r.OverallRating), 2) : 0;
        //	doc.Add(new DocumentField("__reviewstotal", count, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
        //	doc.Add(new DocumentField("__reviewsavg", avg, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
        //}


        private IEnumerable<Partition> GetPartitionsForAllProducts()
        {
            var partitions = new List<Partition>();

            var criteria = new SearchCriteria
            {
                ResponseGroup = ResponseGroup.WithProducts,
                Count = 0
            };

            var result = _catalogSearchService.Search(criteria);

            for (var start = 0; start < result.TotalCount; start += _partitionSizeCount)
            {
                criteria.Start = start;
                criteria.Count = _partitionSizeCount;

                // TODO: Need optimize search to return only product ids
                result = _catalogSearchService.Search(criteria);

                var productIds = result.Products.Select(p => p.Id).ToArray();
                partitions.Add(new Partition(OperationType.Index, productIds));
            }

            return partitions;
        }

        private IEnumerable<Partition> GetPartitionsForModifiedProducts(DateTime startDate, DateTime endDate)
        {
            var partitions = new List<Partition>();

            var productChanges = GetProductChanges(startDate, endDate);
            var deletedProductIds = productChanges.Where(c => c.OperationType == EntryState.Deleted).Select(c => c.ObjectId).ToList();
            var modifiedProductIds = productChanges.Where(c => c.OperationType != EntryState.Deleted).Select(c => c.ObjectId).ToList();

            partitions.AddRange(CreatePartitions(OperationType.Remove, deletedProductIds));
            partitions.AddRange(CreatePartitions(OperationType.Index, modifiedProductIds));

            return partitions;
        }

        private List<OperationLog> GetProductChanges(DateTime startDate, DateTime endDate)
        {
            var allProductChanges = _changeLogService.FindChangeHistory("Product", startDate, endDate).ToList();
            var allPriceChanges = _changeLogService.FindChangeHistory("Price", startDate, endDate).ToList();

            var priceIds = allPriceChanges.Select(c => c.ObjectId).ToList();
            var prices = _pricingService.GetPricesById(priceIds).ToList();

            // TODO: How to get product for deleted price?
            var productsWithChangedPrice = allPriceChanges
                .Select(c => new { c.ModifiedDate, Price = prices.FirstOrDefault(p => p.Id == c.ObjectId) })
                .Where(x => x.Price != null)
                .Select(x => new OperationLog { ObjectId = x.Price.ProductId, ModifiedDate = x.ModifiedDate, OperationType = EntryState.Modified })
                .ToList();

            allProductChanges.AddRange(productsWithChangedPrice);

            // Return latest operation type for each product
            var result = allProductChanges
                .GroupBy(c => c.ObjectId)
                .Select(g => new OperationLog { ObjectId = g.Key, OperationType = g.OrderByDescending(c => c.ModifiedDate).Select(c => c.OperationType).First() })
                .ToList();

            return result;
        }

        private static IEnumerable<Partition> CreatePartitions(OperationType operationType, List<string> allProductIds)
        {
            var partitions = new List<Partition>();

            var totalCount = allProductIds.Count;

            for (var start = 0; start < totalCount; start += _partitionSizeCount)
            {
                var productIds = allProductIds.Skip(start).Take(_partitionSizeCount).ToArray();
                partitions.Add(new Partition(operationType, productIds));
            }

            return partitions;
        }
    }
}
