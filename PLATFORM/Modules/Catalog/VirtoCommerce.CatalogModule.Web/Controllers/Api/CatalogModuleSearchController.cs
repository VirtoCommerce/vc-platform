using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Platform.Core.Security;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Platform.Core.Asset;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CatalogModule.Web.Binders;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog/search")]
    public class CatalogModuleSearchController : CatalogBaseController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public CatalogModuleSearchController(ICatalogSearchService searchService, IBlobUrlResolver blobUrlResolver, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            :base(securityService, permissionScopeService)
        {
            _searchService = searchService;
            _blobUrlResolver = blobUrlResolver;
        }


        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(webModel.CatalogSearchResult))]
        public IHttpActionResult Search([ModelBinder(typeof(CatalogSearchCriteriaBinder))]coreModel.SearchCriteria criteria)
        {
            //Filter search criteria to the corresponding user permissions 
            var searchCriteria = base.ChangeCriteriaToCurentUserPermissions(criteria);
            var serviceResult = _searchService.Search(searchCriteria);

            return Ok(serviceResult.ToWebModel(_blobUrlResolver));
        }

  

    }
}