using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Binders;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.MerchandisingModule.Web.Services;
using VirtoCommerce.Platform.Core.Caching;
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

		// GET: api/mp/products?ids=212&ids=2123&ids=434
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

			var products = InnerGetProductsByIds(fullLoadedStore, new [] { product }, responseGroup);
			var retVal = products.FirstOrDefault();
			if(retVal != null)
			{
				return Ok(retVal);
			}
			return NotFound();
		}

		/// GET: api/mp/products?store=apple&code='22'
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
				var products = InnerGetProductsByIds(fullLoadedStore, new [] { result.Products.First().Id }, responseGroup);
				var retVal = products.FirstOrDefault();
				if (retVal != null)
				{
					return Ok(retVal);
				}
			}

			return NotFound();
		}

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
				var products = InnerGetProductsByIds(fullLoadedStore, new [] { result.Products.First().Id }, responseGroup);
				var retVal = products.FirstOrDefault();
				if (retVal != null)
				{
					return Ok(retVal);
				}
			}

			return NotFound();
		}

		/// <summary>
		///     Searches the specified catalog.
		/// </summary>
		/// <param name="store">The store.</param>
		/// <param name="parameters"></param>
		/// <param name="responseGroup">The response group.</param>
		/// <param name="outline">The outline.</param>
		/// <param name="language">The language.</param>
		/// <param name="currency"></param>
		/// <param name="priceLists"></param>
		/// <returns></returns>
		[HttpGet]
		[ArrayInput(ParameterName = "priceLists")]
		[ClientCache(Duration = 30)]
		[Route("Search")]
		[ResponseType(typeof(ProductSearchResult))]
		public IHttpActionResult Search(string store, string[] priceLists, [ModelBinder(typeof(SearchParametersBinder))] SearchParameters parameters,
										[FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemMedium,
										[FromUri] string outline = "", string language = "en-us", string currency = "USD")
		{
			var context = new Dictionary<string, object>
                          {
                              { "StoreId", store },
                          };

			var fullLoadedStore = GetStoreById(store);
			if (fullLoadedStore == null)
			{
				throw new NullReferenceException(store + " not found");
			}

			var catalog = fullLoadedStore.Catalog;

			string categoryId = null;

			var criteria = new CatalogIndexedSearchCriteria { Locale = language, Catalog = catalog.ToLowerInvariant() };

			if (!string.IsNullOrWhiteSpace(outline))
			{
				criteria.Outlines.Add(String.Format("{0}/{1}*", catalog, outline));
				categoryId = outline.Split(new[] { '/' }).Last();
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
			if (parameters.Terms != null && parameters.Terms.Count > 0)
			{
				foreach (var term in parameters.Terms)
				{
					var filter = filters.SingleOrDefault(x => x.Key.Equals(term.Key, StringComparison.OrdinalIgnoreCase)
						&& (!(x is PriceRangeFilter) || ((PriceRangeFilter)x).Currency.Equals(currency, StringComparison.OrdinalIgnoreCase)));

					var appliedFilter = _browseFilterService.Convert(filter, term.Value);

					criteria.Apply(appliedFilter);
				}
            }
            #endregion

            #region Facets
            // apply facet filters
			var facets = parameters.Facets;
			if (facets.Count != 0)
			{
				foreach (var key in facets.Keys)
				{
					var filter = filters.SingleOrDefault(
						x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
							&& (!(x is PriceRangeFilter)
								|| ((PriceRangeFilter)x).Currency.Equals(currency, StringComparison.OrdinalIgnoreCase)));

					var appliedFilter = _browseFilterService.Convert(filter, facets[key]);
					criteria.Apply(appliedFilter);
				}
            }
            #endregion

            //criteria.ClassTypes.Add("Product");
			criteria.RecordsToRetrieve = parameters.PageSize == 0 ? 10 : parameters.PageSize;
			criteria.StartingRecord = parameters.StartingRecord;
			criteria.Pricelists = priceLists;
			criteria.Currency = currency;
			criteria.StartDateFrom = parameters.StartDateFrom;
			criteria.SearchPhrase = parameters.FreeSearch;

			#region sorting

			if (!string.IsNullOrEmpty(parameters.Sort))
			{
				var isDescending = "desc".Equals(parameters.SortOrder, StringComparison.OrdinalIgnoreCase);

				SearchSort sortObject = null;

				switch (parameters.Sort.ToLowerInvariant())
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
			var searchResults = _browseService.SearchItems(criteria, responseGroup);

            // populate inventory
		    if ((responseGroup & ItemResponseGroup.ItemProperties) == ItemResponseGroup.ItemProperties)
		    {
                PopulateInventory(fullLoadedStore.FulfillmentCenter, searchResults.Items);
		    }

		    return this.Ok(searchResults);
		}

		private storeModel.Store GetStoreById(string storeId)
		{
			var cacheKey = CacheKey.Create("MP", "GetStoreById", storeId);
			var retVal = _cacheManager.Get(cacheKey, () => _storeService.GetById(storeId));
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
                this.PopulateInventory(store.FulfillmentCenter, retVal);
            }
			return retVal;
		}

	    private void PopulateInventory(FulfillmentCenter center, IEnumerable<CatalogItem> items)
	    {
	        if (center == null || items == null || !items.Any())
	            return;

            var inventories = _inventoryService.GetProductsInventoryInfos(items.Select(x=>x.Id).ToArray()).ToList();

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
