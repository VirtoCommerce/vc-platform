using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Services;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web2.Model;
using VirtoCommerce.MerchandisingModule.Web2.Converters;
using Microsoft.Practices.Unity;
namespace VirtoCommerce.MerchandisingModule.Web2.Controllers
{
	[RoutePrefix("api/mp/{catalogId}/{language}/categories")]
	public class CategorySearchController : ApiController
	{
		private readonly ICatalogSearchService _searchService;
		private readonly ICategoryService _categoryService;
		private readonly IPropertyService _propertyService;

		public CategorySearchController(ICatalogSearchService searchService, 
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
		/// <param name="catalogId"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(webModel.GenericSearchResult<webModel.Category>))]
		[Route("")]
		public IHttpActionResult Search(string catalogId, string language="en-us", [FromUri]string parentId = null)
		{
			var criteria = new moduleModel.SearchCriteria
			{
				CatalogId = catalogId,
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

		
	}
}
