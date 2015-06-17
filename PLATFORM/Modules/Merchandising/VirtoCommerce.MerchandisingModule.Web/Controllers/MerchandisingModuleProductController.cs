using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
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
		private readonly CacheManager _cacheManager;
		private readonly IPropertyService _propertyService;

		public MerchandisingModuleProductController(ICatalogSearchService searchService, ICategoryService categoryService,
								 IStoreService storeService, IItemService itemService, IBlobUrlResolver blobUrlResolver,
								 IBrowseFilterService browseFilterService, IItemBrowsingService browseService, CacheManager cacheManager, IPropertyService propertyService)
		{
			_itemService = itemService;
			_storeService = storeService;
			_searchService = searchService;
			_categoryService = categoryService;
			_blobUrlResolver = blobUrlResolver;
			_browseFilterService = browseFilterService;
			_browseService = browseService;
			_cacheManager = cacheManager;
			_propertyService = propertyService;
		}


		#region Public Methods and Operators
		/// GET: api/mp/products?ids=212&ids=2123&ids=434
		[HttpGet]
		[ArrayInput(ParameterName = "ids")]
		[ResponseType(typeof(CatalogItem[]))]
		[ClientCache(Duration = 30)]
		[Route("")]
		public IHttpActionResult GetProductsByIds([FromUri] string[] ids, [FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemInfo)
		{
			var retVal = InnerGetProductsByIds(ids, responseGroup);
			return Ok(retVal);
		}


		[HttpGet]
		[ResponseType(typeof(CatalogItem))]
		[ClientCache(Duration = 30)]
		[Route("{product}")]
		public IHttpActionResult GetProduct(string store, string product, [FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemLarge, string language = "en-us")
		{
			var products = InnerGetProductsByIds(new string[] { product }, responseGroup);
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

			var searchCriteria = new SearchCriteria
			{
				ResponseGroup = ResponseGroup.WithProducts | ResponseGroup.WithVariations,
				Code = code,
			};

			var result = _searchService.Search(searchCriteria);
			if (result.Products != null && result.Products.Any())
			{
				var products = InnerGetProductsByIds(new string[] { result.Products.First().Id }, ItemResponseGroup.ItemLarge);
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
			var searchCriteria = new SearchCriteria
			{
				ResponseGroup = ResponseGroup.WithProducts,
				SeoKeyword = keyword,
			};

			var result = _searchService.Search(searchCriteria);
			if (result.Products != null && result.Products.Any())
			{
				var products = InnerGetProductsByIds(new string[] { result.Products.First().Id }, ItemResponseGroup.ItemLarge);
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
		[Route("")]
		[ResponseType(typeof(ProductSearchResult))]
		public IHttpActionResult Search(string store, string[] priceLists, [ModelBinder(typeof(SearchParametersBinder))] SearchParameters parameters,
										[FromUri] coreModel.ItemResponseGroup responseGroup = coreModel.ItemResponseGroup.ItemMedium,
										[FromUri] string outline = "", string language = "en-us", string currency = "USD")
		{
			var context = new Dictionary<string, object>
                          {
                              { "StoreId", store },
                          };



			var fullLoadedStore = _storeService.GetById(store);
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

			// apply filters
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
			//var cacheKey = CacheKey.Create("ProductController.Search", criteria.CacheKey);
			//var searchResults = _cacheManager.Get(cacheKey, () => _browseService.SearchItems(criteria, responseGroup));
			var searchResults = _browseService.SearchItems(criteria, responseGroup);



			return this.Ok(searchResults);
		}

		private IEnumerable<CatalogItem> InnerGetProductsByIds(String[] ids, ItemResponseGroup responseGroup)
		{
			var retVal = new List<CatalogItem>();
			var products = _itemService.GetByIds(ids, responseGroup);
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
			return retVal;
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
