using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.MerchandisingModule.Web.Binders;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using moduleModel = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/{catalog}/{language}")]
	public class ProductController : ApiController
	{
		private readonly IItemService _itemService;
		private readonly ISearchProvider _searchService;
		private readonly ISearchConnection _searchConnection;

		public ProductController(IItemService itemService,
									   ISearchProvider indexedSearchProvider,
									   ISearchConnection searchConnection)
		{
			_searchService = indexedSearchProvider;
			_searchConnection = searchConnection;
			_itemService = itemService;
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
        [Route("products/{outline?}")]
		[ResponseType(typeof(GenericSearchResult<CatalogItem>))]
		public IHttpActionResult Search(string catalog, [ModelBinder(typeof(CatalogItemSearchCriteriaBinder))] CatalogItemSearchCriteria criteria, string outline="", string language = "en-us")
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
					var webModelProduct = product.ToWebModel();

					var searchTags = items[productId];

					webModelProduct.Outline = searchTags[criteria.OutlineField].ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
															   .FirstOrDefault(x => x.StartsWith(criteria.Catalog, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;
					retVal.Items.Add(webModelProduct);
				}
			}
			return Ok(retVal);
		}

		[HttpGet]
		[ResponseType(typeof(Product))]
		[Route("product/{productId}")]
		public IHttpActionResult GetProduct(string productId)
		{
			var product = _itemService.GetById(productId, moduleModel.ItemResponseGroup.ItemLarge);
		    if (product != null)
		    {
		        var retVal = product.ToWebModel();
		        return Ok(retVal);
		    }
		    return StatusCode(HttpStatusCode.NotFound);
		}
	}
}
