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
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/catalogs")]
    public class CatalogModuleCatalogsController : ApiController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICatalogSearchService _searchService;
        private readonly IPropertyService _propertyService;
        private readonly ISettingsManager _settingManager;
        private readonly IPermissionService _permissionService;

        public CatalogModuleCatalogsController(ICatalogService catalogService,
                                  ICatalogSearchService itemSearchService,
                                  ISettingsManager settingManager,
                                  IPropertyService propertyService, IPermissionService permissionService)
        {
            _catalogService = catalogService;
            _searchService = itemSearchService;
            _propertyService = propertyService;
            _settingManager = settingManager;
            _permissionService = permissionService;
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
                ResponseGroup = moduleModel.ResponseGroup.WithCatalogs
            };
            var serviceResult = _searchService.Search(criteria);
            var retVal = serviceResult.Catalogs.Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>Gets Catalog by id with full information loaded</remarks>
        /// <param name="id">The Catalog id.</param>
		[HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("{id}")]
        [CheckPermission(Permission = PredefinedPermissions.Query)]
        public IHttpActionResult Get(string id)
        {
            var catalog = _catalogService.GetById(id);
            if (catalog == null)
            {
                return NotFound();
            }
            var allCatalogProperties = _propertyService.GetCatalogProperties(id);
            return Ok(catalog.ToWebModel(allCatalogProperties));
        }

        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>Gets the template for a new common catalog</remarks>
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("getnew")]
        [CheckPermission(Permission = PredefinedPermissions.Create)]
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

            //retVal = _catalogService.Create(retVal.ToModuleModel()).ToWebModel();
            return Ok(retVal);
        }

        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
        [Route("getnewvirtual")]
        [CheckPermission(Permission = PredefinedPermissions.Create)]
        public IHttpActionResult GetNewVirtualCatalog()
        {
            var retVal = new webModel.Catalog
            {
                Name = "New virtual catalog",
                Virtual = true,
                Languages = new List<webModel.CatalogLanguage>
                {
                    new webModel.CatalogLanguage
                    {
                        IsDefault = true,
                        LanguageCode = "en-US"
                    }
                }
            };
            //retVal = _catalogService.Create(retVal.ToModuleModel()).ToWebModel();
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
        [CheckPermission(Permissions = new[] { PredefinedPermissions.Create })]
        public IHttpActionResult Create(webModel.Catalog catalog)
        {
            //          if ((_permissionService.UserHasAnyPermission(RequestContext.Principal.Identity.Name, PredefinedPermissions.CatalogsManage) && !catalog.Virtual)
            //              || (_permissionService.UserHasAnyPermission(RequestContext.Principal.Identity.Name, PredefinedPermissions.VirtualCatalogsManage) && catalog.Virtual))
            //          {
            var retVal = _catalogService.Create(catalog.ToModuleModel());
            return Ok(retVal.ToWebModel());
            //}
            //          else
            //          {
            //              throw new UnauthorizedAccessException();
            //          }
        }

        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>Updates the specified catalog.</remarks>
        /// <param name="catalog">The catalog.</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permissions = new[] { PredefinedPermissions.Update })]
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
        [CheckPermission(Permissions = new[] { PredefinedPermissions.Delete })]
        public IHttpActionResult Delete(string id)
        {
            _catalogService.Delete(new string[] { id });
            return StatusCode(HttpStatusCode.NoContent);
        }

        private void UpdateCatalog(webModel.Catalog catalog)
        {
            var moduleCatalog = catalog.ToModuleModel();
            _catalogService.Update(new moduleModel.Catalog[] { moduleCatalog });
        }


    }
}
