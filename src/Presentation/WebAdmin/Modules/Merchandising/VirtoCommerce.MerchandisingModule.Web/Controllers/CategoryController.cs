using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
	[RoutePrefix("api/mp/{catalog}/{language}")]
	public class CategoryController : ApiController
	{
		private readonly ICatalogSearchService _searchService;
		private readonly ICategoryService _categoryService;
		private readonly IPropertyService _propertyService;

		public CategoryController(ICatalogSearchService searchService, 
										ICategoryService categoryService,
										IPropertyService propertyService)
		{
			_searchService = searchService;
			_categoryService = categoryService;
			_propertyService = propertyService;
		}


	    /// <summary>
	    ///  GET: api/mp/apple/en-us/categories?parentId='22'
	    /// </summary>
	    /// <param name="catalog"></param>
	    /// <param name="language"></param>
	    /// <param name="parentId"></param>
	    /// <returns></returns>
	    [HttpGet]
		[ResponseType(typeof(webModel.GenericSearchResult<webModel.Category>))]
        [Route("categories")]
		public IHttpActionResult Search(string catalog, string language="en-us", [FromUri]string parentId = null)
		{
			var criteria = new moduleModel.SearchCriteria
			{
				CatalogId = catalog,
				CategoryId = parentId,
				Start = 0,
				Count = int.MaxValue,
				ResponseGroup = moduleModel.ResponseGroup.WithCategories
			};
			var result = _searchService.Search(criteria);
			var retVal = new webModel.GenericSearchResult<webModel.Category>
			{
				TotalCount = result.Categories.Count(),
				Items = result.Categories.Select(x => x.ToWebModel()).ToList()
			};
		
			return Ok(retVal);
		}

        [HttpGet]
        [ResponseType(typeof(webModel.GenericSearchResult<webModel.Category>))]
        [Route("category/{categoryId}")]
        public IHttpActionResult Get(string categoryId, string catalog, string language = "en-us")
        {
            var result = _categoryService.GetById(categoryId);

            if (result != null)
            {
                var retVal = result.ToWebModel();

                return Ok(retVal);
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

		
	}
}
