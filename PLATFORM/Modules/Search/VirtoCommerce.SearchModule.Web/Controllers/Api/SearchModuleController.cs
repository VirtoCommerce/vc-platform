using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.SearchModule.Web.BackgroundJobs;

namespace VirtoCommerce.SearchModule.Web.Controllers.Api
{
    [RoutePrefix("api/search")]
    [CheckPermission(Permission = "VirtoCommerce.Search:Debug")]
    public class SearchModuleController : ApiController
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ISearchConnection _searchConnection;
        private readonly SearchIndexJobsScheduler _scheduler;

        public SearchModuleController(ISearchProvider searchProvider, ISearchConnection searchConnection, SearchIndexJobsScheduler scheduler)
        {
            _searchProvider = searchProvider;
            _searchConnection = searchConnection;
            _scheduler = scheduler;
        }

        [HttpGet]
        [Route("catalogitem")]
        [ResponseType(typeof(ISearchResults))]
        public IHttpActionResult Search([FromUri]CatalogIndexedSearchCriteria criteria)
        {
            criteria = criteria ?? new CatalogIndexedSearchCriteria();
            var scope = _searchConnection.Scope;
            var searchResults = _searchProvider.Search(scope, criteria);
            return Ok(searchResults);
        }

        [HttpGet]
        [Route("catalogitem/rebuild")]
        [CheckPermission(Permission = "VirtoCommerce.Search:Index:Rebuild")]
        public IHttpActionResult Rebuild()
        {
            var jobId = _scheduler.SheduleRebuildIndex();
            var result = new { Id = jobId };
            return Ok(result);
        }
    }
}
