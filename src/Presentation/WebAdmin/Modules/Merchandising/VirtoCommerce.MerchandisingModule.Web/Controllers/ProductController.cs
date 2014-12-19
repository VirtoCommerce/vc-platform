using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.MerchandisingModule.Web.Binders;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using moduleModel = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/{catalog}/{language}/products")]
	public class ProductController : ApiController
	{
		private readonly IItemService _itemService;
		private readonly ISearchProvider _searchService;
		private readonly ISearchConnection _searchConnection;
		private readonly Func<IFoundationCatalogRepository> _foundationCatalogRepositoryFactory;
		private readonly Uri _assetBaseUri;

		public ProductController(IItemService itemService,
								 ISearchProvider indexedSearchProvider,
								 ISearchConnection searchConnection,
								 Func<IFoundationCatalogRepository> foundationCatalogRepositoryFactory,
								 Uri assetBaseUri)
		{
			_searchService = indexedSearchProvider;
			_searchConnection = searchConnection;
			_itemService = itemService;
			_foundationCatalogRepositoryFactory = foundationCatalogRepositoryFactory;
			_assetBaseUri = assetBaseUri;
		}

	    /// <summary>
	    /// GET: api/mp/apple/en-us/products?q='some keyword'&outline=apple/catalog
	    /// </summary>
	    /// <param name="catalog"></param>
	    /// <param name="criteria"></param>
	    /// <param name="outline"></param>
	    /// <param name="language"></param>
	    /// <returns></returns>
	    [HttpGet]
        [Route("")]
		[ResponseType(typeof(GenericSearchResult<CatalogItem>))]
		public IHttpActionResult Search(string catalog, [ModelBinder(typeof(CatalogItemSearchCriteriaBinder))] CatalogItemSearchCriteria criteria, [FromUri]string outline="", string language = "en-us")
		{
			criteria.Locale = language;
			criteria.Catalog = catalog;
            if (!string.IsNullOrWhiteSpace(outline))
            {
                criteria.Outlines.Add(String.Format("{0}/{1}*", catalog, outline));
            }
			var result = _searchService.Search(_searchConnection.Scope, criteria) as SearchResults;
			var items = result.GetKeyAndOutlineFieldValueMap<string>();

			var retVal = new GenericSearchResult<CatalogItem> {TotalCount = result.TotalCount};
		    //Load products 
			foreach (var productId in items.Keys)
			{
				var product = _itemService.GetById(productId, moduleModel.ItemResponseGroup.ItemMedium);
				if (product != null)
				{
					var webModelProduct = product.ToWebModel(_assetBaseUri);

					var searchTags = items[productId];

					webModelProduct.Outline = searchTags[criteria.OutlineField].ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
															   .FirstOrDefault(x => x.StartsWith(criteria.Catalog, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

				    int reviewTotal;
                    if (searchTags.ContainsKey(criteria.ReviewsTotalField) && int.TryParse(searchTags[criteria.ReviewsTotalField].ToString(), out reviewTotal))
                    {
                        webModelProduct.ReviewsTotal = reviewTotal;
				    }
                    double reviewAvg;
                    if (searchTags.ContainsKey(criteria.ReviewsAverageField) && double.TryParse(searchTags[criteria.ReviewsAverageField].ToString(), out reviewAvg))
                    {
                        webModelProduct.Rating = reviewAvg;
                    }

					retVal.Items.Add(webModelProduct);
				}
			}
			return Ok(retVal);
		}

		/// GET: api/mp/apple/en-us/products?code='22'
		[HttpGet]
		[ResponseType(typeof(Product))]
		[Route("")]
		public IHttpActionResult GetProductByCode(string catalog, [FromUri]string code, string language = "en-us")
		{
			using(var repository = _foundationCatalogRepositoryFactory())
			{
				var itemId = repository.Items.Where(x => x.CatalogId == catalog && x.Code == code).Select(x => x.ItemId).FirstOrDefault();
				if(itemId != null)
				{
					return GetProduct(itemId);
				}
			}
			return StatusCode(HttpStatusCode.NotFound);
		}



		[HttpGet]
		[ResponseType(typeof(Product))]
		[Route("{productId}")]
		public IHttpActionResult GetProduct(string productId)
		{
			var product = _itemService.GetById(productId, moduleModel.ItemResponseGroup.ItemLarge);
		    if (product != null)
		    {
				var retVal = product.ToWebModel(_assetBaseUri);
		        return Ok(retVal);
		    }
		    return StatusCode(HttpStatusCode.NotFound);
		}
	}
}
