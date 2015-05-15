using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.SearchModule.Web.Controllers.Api
{
    [RoutePrefix("api/search")]
    [CheckPermission(Permission = "VirtoCommerce.Search:Debug")]
    public class SearchModuleController : ApiController
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ISearchConnection _searchConnection;

        public SearchModuleController(ISearchProvider searchProvider, ISearchConnection searchConnection)
        {
            _searchProvider = searchProvider;
            _searchConnection = searchConnection;
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
    }
}
