using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/catalogs")]
    public class CatalogModuleCatalogsController : CatalogBaseController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICatalogSearchService _searchService;
        private readonly ISettingsManager _settingManager;

        public CatalogModuleCatalogsController(ICatalogService catalogService, ICatalogSearchService itemSearchService,
                                  ISettingsManager settingManager, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            : base(securityService, permissionScopeService)
        {
            _catalogService = catalogService;
            _searchService = itemSearchService;
            _settingManager = settingManager;
        }

        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>Get common and virtual Catalogs list with minimal information included. Returns array of Catalog</remarks>
		[HttpGet]
        [ResponseType(typeof(webModel.Catalog[]))]
        [Route("")]
        public IHttpActionResult GetCatalogs()
        {
            var criteria = new moduleModel.SearchCriteria
            {
                ResponseGroup = moduleModel.SearchResponseGroup.WithCatalogs
            };

            ApplyRestrictionsForCurrentUser(criteria);

            var serviceResult = _searchService.Search(criteria);
            var retVal = new List<webModel.Catalog>();

            foreach (var catalog in serviceResult.Catalogs)
            {
                var webCatalog = catalog.ToWebModel();
                webCatalog.SecurityScopes = GetObjectPermissionScopeStrings(catalog);
                retVal.Add(webCatalog);
            }

            return Ok(retVal.ToArray());
        }

        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>Gets Catalog by id with full information loaded</remarks>
        /// <param name="id">The Catalog id.</param>
		[HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var catalog = _catalogService.GetById(id);
            if (catalog == null)
            {
                return NotFound();
            }
            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, catalog);

            var retVal = catalog.ToWebModel();

            retVal.SecurityScopes = GetObjectPermissionScopeStrings(catalog);

            return Ok(retVal);
        }

        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>Gets the template for a new common catalog</remarks>
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("getnew")]
        [CheckPermission(Permission = CatalogPredefinedPermissions.Create)]
        public IHttpActionResult GetNewCatalog()
        {
            var retVal = new webModel.Catalog
            {
                Name = "New catalog",
                Languages = new List<webModel.CatalogLanguage>
                {
                    new webModel.CatalogLanguage
                    {
                        IsDefault = true,
                        LanguageCode = "en-US"
                    }
                }
            };

            retVal.SecurityScopes = GetObjectPermissionScopeStrings(retVal);

            return Ok(retVal);
        }

        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("getnewvirtual")]
        [CheckPermission(Permission = CatalogPredefinedPermissions.Create)]
        public IHttpActionResult GetNewVirtualCatalog()
        {
            var retVal = new webModel.Catalog
            {
                Name = "New virtual catalog",
                IsVirtual = true,
                Languages = new List<webModel.CatalogLanguage>
                {
                    new webModel.CatalogLanguage
                    {
                        IsDefault = true,
                        LanguageCode = "en-US"
                    }
                }
            };
            retVal.SecurityScopes = GetObjectPermissionScopeStrings(retVal);
            return Ok(retVal);
        }

        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>Creates the specified catalog</remarks>
        /// <param name="catalog">The catalog to create</param>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
		[HttpPost]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("")]
        [CheckPermission(Permission = CatalogPredefinedPermissions.Create)]
        public IHttpActionResult Create(webModel.Catalog catalog)
        {
            var newCatalog = _catalogService.Create(catalog.ToModuleModel());
            var retVal = newCatalog.ToWebModel();
            //Need for UI permission checks
            retVal.SecurityScopes = GetObjectPermissionScopeStrings(newCatalog);
            return Ok(retVal);
        }

        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>Updates the specified catalog.</remarks>
        /// <param name="catalog">The catalog.</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Update(webModel.Catalog catalog)
        {
            UpdateCatalog(catalog);
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>Deletes catalog by id</remarks>
        /// <param name="id">Catalog id.</param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            var catalog = _catalogService.GetById(id);
            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Delete, catalog);

            _catalogService.Delete(new[] { id });
            return StatusCode(HttpStatusCode.NoContent);
        }

        private void UpdateCatalog(webModel.Catalog catalog)
        {
            var moduleCatalog = catalog.ToModuleModel();

            CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Update, catalog);

            _catalogService.Update(new[] { moduleCatalog });
        }


    }
}
