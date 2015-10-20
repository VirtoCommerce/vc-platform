using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.MerchandisingModule.Web.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using storeModel = VirtoCommerce.Domain.Store.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/products")]
    public class MerchandisingModuleProductController : ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;
        private readonly ICatalogSearchService _searchService;
        private readonly IStoreService _storeService;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly IBrowseFilterService _browseFilterService;
        private readonly IItemBrowsingService _browseService;
        private readonly IPropertyService _propertyService;
        private readonly IInventoryService _inventoryService;
        private readonly ICommerceService _commerceService;
        private readonly CacheManager _cacheManager;

        public MerchandisingModuleProductController(ICatalogSearchService searchService, ICategoryService categoryService,
                                 IInventoryService inventoryService, IStoreService storeService, IItemService itemService, IBlobUrlResolver blobUrlResolver,
                                 IBrowseFilterService browseFilterService, IItemBrowsingService browseService, IPropertyService propertyService, ICommerceService commerceService,
                                 CacheManager cacheManager)
        {
            _itemService = itemService;
            _storeService = storeService;
            _searchService = searchService;
            _categoryService = categoryService;
            _blobUrlResolver = blobUrlResolver;
            _browseFilterService = browseFilterService;
            _browseService = browseService;
            _propertyService = propertyService;
            _commerceService = commerceService;
            _inventoryService = inventoryService;
            _cacheManager = cacheManager;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Get store products collection by their ids
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="ids">Product ids</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param>
        [HttpGet]
        [ArrayInput(ParameterName = "ids")]
        [ResponseType(typeof(CatalogItem[]))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetProductsByIds(string store, [FromUri] string[] ids, [FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemInfo)
        {
            var fullLoadedStore = GetStoreById(store);
            if (fullLoadedStore == null)
            {
                throw new NullReferenceException(store + " not found");
            }

            var retVal = InnerGetProductsByIds(fullLoadedStore, ids, responseGroup);
            return Ok(retVal);
        }

        /// <summary>
        /// Get store product by id
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="product">Product id</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <response code="200"></response>
        /// <response code="404">Product not found</response>
		[HttpGet]
        [ResponseType(typeof(CatalogItem))]
        [ClientCache(Duration = 30)]
        [Route("{product}")]
        public IHttpActionResult GetProduct(string store, string product, [FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemLarge, string language = "en-us")
        {
            var fullLoadedStore = GetStoreById(store);
            if (fullLoadedStore == null)
            {
                throw new NullReferenceException(store + " not found");
            }

            var products = InnerGetProductsByIds(fullLoadedStore, new[] { product }, responseGroup);
            var retVal = products.FirstOrDefault();
            if (retVal != null)
            {
                return Ok(retVal);
            }
            return NotFound();
        }

        /// <summary>
        /// Get store product by code
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="code">Product code</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <response code="200"></response>
        /// <response code="404">Product not found</response>
        [HttpGet]
        [ResponseType(typeof(CatalogItem))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetProductByCode(string store, [FromUri] string code, [FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemLarge, string language = "en-us")
        {
            var fullLoadedStore = GetStoreById(store);
            if (fullLoadedStore == null)
            {
                throw new NullReferenceException(store + " not found");
            }

            var searchCriteria = new SearchCriteria
            {
                ResponseGroup = ResponseGroup.WithProducts | ResponseGroup.WithVariations,
                Code = code,
                //CatalogId = fullLoadedStore.Catalog
            };

            var result = _searchService.Search(searchCriteria);
            if (result.Products != null && result.Products.Any())
            {
                var products = InnerGetProductsByIds(fullLoadedStore, new[] { result.Products.First().Id }, responseGroup);
                var retVal = products.FirstOrDefault();
                if (retVal != null)
                {
                    return Ok(retVal);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get store product by SEO keyword
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="keyword">Product SEO keyword</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <response code="200"></response>
        /// <response code="404">Product not found</response>
		[HttpGet]
        [ResponseType(typeof(CatalogItem))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetProductByKeyword(string store, [FromUri] string keyword, [FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemLarge, string language = "en-us")
        {
            var fullLoadedStore = GetStoreById(store);
            if (fullLoadedStore == null)
            {
                throw new NullReferenceException(store + " not found");
            }

            var searchCriteria = new SearchCriteria
            {
                ResponseGroup = ResponseGroup.WithProducts | ResponseGroup.WithVariations,
                SeoKeyword = keyword,
                //CatalogId = fullLoadedStore.Catalog
            };

            var result = _searchService.Search(searchCriteria);
            if (result.Products != null && result.Products.Any())
            {
                var products = InnerGetProductsByIds(fullLoadedStore, new[] { result.Products.First().Id }, responseGroup);
                var retVal = products.FirstOrDefault();
                if (retVal != null)
                {
                    return Ok(retVal);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Search for store products
        /// </summary>
        /// <param name="request">Search parameters</param>
        [HttpGet]
        [ClientCache(Duration = 30)]
        [Route("search")]
        [ResponseType(typeof(ProductSearchResult))]
        public IHttpActionResult Search([FromUri] ProductSearchRequest request)
        {
            request = request ?? new ProductSearchRequest();
            request.Normalize();

            var context = new Dictionary<string, object>
            {
                { "StoreId", request.Store },
            };

            var fullLoadedStore = GetStoreById(request.Store);
            if (fullLoadedStore == null)
            {
                throw new NullReferenceException(request.Store + " not found");
            }

            var catalog = fullLoadedStore.Catalog;

            string categoryId = null;

            var criteria = new CatalogIndexedSearchCriteria { Locale = request.Language, Catalog = catalog.ToLowerInvariant(), IsFuzzySearch = true };

            if (!string.IsNullOrWhiteSpace(request.Outline))
            {
                criteria.Outlines.Add(string.Format("{0}/{1}*", catalog, request.Outline));
                categoryId = request.Outline.Split(new[] { '/' }).Last();
                context.Add("CategoryId", categoryId);
            }

            #region Filters
            // Now fill in filters
            var filters = _browseFilterService.GetFilters(context);

            // Add all filters
            foreach (var filter in filters)
            {
                criteria.Add(filter);
            }

            // apply terms
            var terms = ParseKeyValues(request.Terms);
            if (terms.Any())
            {
                var filtersWithValues = filters
                    .Where(x => (!(x is PriceRangeFilter) || ((PriceRangeFilter)x).Currency.Equals(request.Currency, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => new { Filter = x, Values = x.GetValues() })
                    .ToList();

                foreach (var term in terms)
                {
                    var filter = filters.SingleOrDefault(x => x.Key.Equals(term.Key, StringComparison.OrdinalIgnoreCase)
                        && (!(x is PriceRangeFilter) || ((PriceRangeFilter)x).Currency.Equals(request.Currency, StringComparison.OrdinalIgnoreCase)));

                    // handle special filter term with a key = "tags", it contains just values and we need to determine which filter to use
                    if (filter == null && term.Key == "tags")
                    {
                        foreach (var termValue in term.Values)
                        {
                            // try to find filter by value
                            var foundFilter = filtersWithValues.FirstOrDefault(x => x.Values.Any(y => y.Id.Equals(termValue)));

                            if (foundFilter != null)
                            {
                                filter = foundFilter.Filter;

                                var appliedFilter = _browseFilterService.Convert(filter, term.Values);
                                criteria.Apply(appliedFilter);
                            }
                        }
                    }
                    else
                    {
                        var appliedFilter = _browseFilterService.Convert(filter, term.Values);
                        criteria.Apply(appliedFilter);
                    }
                }
            }
            #endregion

            #region Facets
            // apply facet filters
            var facets = ParseKeyValues(request.Facets);
            foreach (var facet in facets)
            {
                var filter = filters.SingleOrDefault(
                    x => x.Key.Equals(facet.Key, StringComparison.OrdinalIgnoreCase)
                        && (!(x is PriceRangeFilter)
                            || ((PriceRangeFilter)x).Currency.Equals(request.Currency, StringComparison.OrdinalIgnoreCase)));

                var appliedFilter = _browseFilterService.Convert(filter, facet.Values);
                criteria.Apply(appliedFilter);
            }
            #endregion

            //criteria.ClassTypes.Add("Product");
            criteria.RecordsToRetrieve = request.Take <= 0 ? 10 : request.Take;
            criteria.StartingRecord = request.Skip;
            criteria.Pricelists = request.Pricelists;
            criteria.Currency = request.Currency;
            criteria.StartDateFrom = request.StartDateFrom;
            criteria.SearchPhrase = request.SearchPhrase;

            #region sorting

            if (!string.IsNullOrEmpty(request.Sort))
            {
                var isDescending = "desc".Equals(request.SortOrder, StringComparison.OrdinalIgnoreCase);

                SearchSort sortObject = null;

                switch (request.Sort.ToLowerInvariant())
                {
                    case "price":
                        if (criteria.Pricelists != null)
                        {
                            sortObject = new SearchSort(
                                criteria.Pricelists.Select(
                                    priceList =>
                                        new SearchSortField(String.Format("price_{0}_{1}", criteria.Currency.ToLower(), priceList.ToLower()))
                                        {
                                            IgnoredUnmapped = true,
                                            IsDescending = isDescending,
                                            DataType = SearchSortField.DOUBLE
                                        })
                                    .ToArray());
                        }
                        break;
                    case "position":
                        sortObject =
                            new SearchSort(
                                new SearchSortField(string.Format("sort{0}{1}", catalog, categoryId).ToLower())
                                {
                                    IgnoredUnmapped = true,
                                    IsDescending = isDescending
                                });
                        break;
                    case "name":
                        sortObject = new SearchSort("name", isDescending);
                        break;
                    case "rating":
                        sortObject = new SearchSort(criteria.ReviewsAverageField, isDescending);
                        break;
                    case "reviews":
                        sortObject = new SearchSort(criteria.ReviewsTotalField, isDescending);
                        break;
                    default:
                        sortObject = CatalogIndexedSearchCriteria.DefaultSortOrder;
                        break;
                }

                criteria.Sort = sortObject;
            }

            #endregion

            //Load ALL products 
            var searchResults = _browseService.SearchItems(criteria, request.ResponseGroup);

            // populate inventory
            if ((request.ResponseGroup & ItemResponseGroup.ItemProperties) == ItemResponseGroup.ItemProperties)
            {
                PopulateInventory(fullLoadedStore.FulfillmentCenter, searchResults.Items);
            }

            return Ok(searchResults);
        }

        private static List<StringKeyValues> ParseKeyValues(string[] items)
        {
            var result = new List<StringKeyValues>();

            if (items != null)
            {
                var nameValueDelimeter = new[] { ':' };
                var valuesDelimeter = new[] { ',' };

                result.AddRange(items
                    .Select(item => item.Split(nameValueDelimeter, 2))
                    .Where(item => item.Length == 2)
                    .Select(item => new StringKeyValues { Key = item[0], Values = item[1].Split(valuesDelimeter, StringSplitOptions.RemoveEmptyEntries) })
                    .GroupBy(item => item.Key)
                    .Select(g => new StringKeyValues { Key = g.Key, Values = g.SelectMany(i => i.Values).Distinct().ToArray() })
                    );
            }

            return result;
        }

        private storeModel.Store GetStoreById(string storeId)
        {
            var retVal = _storeService.GetById(storeId);
            return retVal;
        }

        private IEnumerable<CatalogItem> InnerGetProductsByIds(storeModel.Store store, String[] ids, ItemResponseGroup responseGroup)
        {
            var retVal = new List<CatalogItem>();
            var products = _itemService.GetByIds(ids, responseGroup);//.Where(p=>p.CatalogId == store.Catalog);

            foreach (var product in products)
            {
                coreModel.Property[] properties = null;
                if ((responseGroup & ItemResponseGroup.ItemProperties) == ItemResponseGroup.ItemProperties)
                {
                    properties = GetAllProductProperies(product);
                }

                if (product != null)
                {
                    var webModelProduct = product.ToWebModel(_blobUrlResolver, properties);
                    if (product.CategoryId != null)
                    {
                        var category = _categoryService.GetById(product.CategoryId);
                        webModelProduct.Outline = string.Join("/", category.Parents.Select(x => x.Id)) + "/" + category.Id;
                    }
                    retVal.Add(webModelProduct);
                }
            }

            if ((responseGroup & ItemResponseGroup.Inventory) == ItemResponseGroup.Inventory)
            {
                PopulateInventory(store.FulfillmentCenter, retVal);
            }
            return retVal;
        }

        private void PopulateInventory(FulfillmentCenter center, IEnumerable<CatalogItem> items)
        {
            if (center == null || items == null || !items.Any())
                return;

            var inventories = _inventoryService.GetProductsInventoryInfos(items.Select(x => x.Id).ToArray()).ToList();

            foreach (var catalogItem in items)
            {
                var productInventory = inventories.FirstOrDefault(x => x.ProductId == catalogItem.Id && x.FulfillmentCenterId == center.Id);
                if (productInventory != null)
                    catalogItem.Inventory = productInventory.ToWebModel();
            }
        }

        private coreModel.Property[] GetAllProductProperies(coreModel.CatalogProduct product)
        {
            coreModel.Property[] retVal = null;
            if (!String.IsNullOrEmpty(product.CategoryId))
            {
                retVal = _propertyService.GetCategoryProperties(product.CategoryId);
            }
            else
            {
                retVal = _propertyService.GetCatalogProperties(product.CatalogId);
            }
            return retVal;
        }
        #endregion
    }
}
