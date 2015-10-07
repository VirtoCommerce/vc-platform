using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CustomerModule.Web.Binders;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Web.Security;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing")]
    [CheckPermission(Permission = MarketingPredefinedPermissions.Read)]
    public class MarketingModuleController : ApiController
    {
		private readonly IMarketingSearchService _marketingSearchService;
		public MarketingModuleController(IMarketingSearchService marketingSearchService)
		{
			_marketingSearchService = marketingSearchService;
		}

		/// <summary>
		/// Search marketing objects by given criteria
		/// </summary>
		/// <remarks>Allow to find all marketing module objects (Promotions, Dynamic content objects)</remarks>
		/// <param name="criteria">criteria</param>
		[HttpGet]
		[ResponseType(typeof(webModel.MarketingSearchResult))]
		[Route("search")]
		public IHttpActionResult Search([ModelBinder(typeof(MarketingSearchCriteriaBinder))] coreModel.MarketingSearchCriteria criteria)
		{
			var retVal = new webModel.MarketingSearchResult();
			var coreResult = _marketingSearchService.SearchResources(criteria);

			retVal.Promotions = coreResult.Promotions.Select(x => x.ToWebModel()).ToList();
			retVal.ContentPlaces = coreResult.ContentPlaces.Select(x => x.ToWebModel()).ToList();
			retVal.ContentItems = coreResult.ContentItems.Select(x => x.ToWebModel()).ToList();
			retVal.ContentPublications = coreResult.ContentPublications.Select(x => x.ToWebModel()).ToList();
			retVal.ContentFolders = coreResult.ContentFolders.Select(x => x.ToWebModel()).ToList();
			retVal.TotalCount = coreResult.TotalCount;
			return Ok(retVal);
		}

	
    }
}
