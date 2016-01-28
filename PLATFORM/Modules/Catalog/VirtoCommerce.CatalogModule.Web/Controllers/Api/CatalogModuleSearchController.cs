using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/search")]
    public class CatalogModuleSearchController : CatalogBaseController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public CatalogModuleSearchController(ICatalogSearchService searchService, IBlobUrlResolver blobUrlResolver, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            : base(securityService, permissionScopeService)
        {
            _searchService = searchService;
            _blobUrlResolver = blobUrlResolver;
        }


        /// <summary>
        /// Searches for the items by complex criteria
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(CatalogSearchResult))]
        public IHttpActionResult Search(SearchCriteria criteria)
        {
            ApplyRestrictionsForCurrentUser(criteria);
            var serviceResult = _searchService.Search(criteria);

            return Ok(serviceResult.ToWebModel(_blobUrlResolver));
        }
    }
}
