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

		// GET: api/catalog/catalogs
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

		// GET:  api/catalog/catalogs/4
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

        //// GET:  api/catalog/catalogs/4/languages
        //[HttpGet]
        //[ResponseType(typeof(webModel.CatalogLanguage[]))]
        //[Route("{id}/languages")]
        //public IHttpActionResult GetCatalogLanguages(string id)
        //{
        //    var catalog = _catalogService.GetById(id);
        //    if (catalog == null)
        //        return NotFound();

        //    var retVal = catalog.ToWebModel().Languages;

        //    var systemLanguages = GetSystemLanguages();
        //    foreach (var systemLanguage in systemLanguages)
        //    {
        //        var alreadyExistLanguage = retVal.FirstOrDefault(x => string.Equals(x.LanguageCode, systemLanguage.LanguageCode, StringComparison.CurrentCultureIgnoreCase));
        //        if (alreadyExistLanguage == null)
        //        {
        //            retVal.Add(systemLanguage);
        //        }
        //    }
        //    return Ok(retVal.OrderBy(x => x.DisplayName));
        //}

        //// POST: api/catalog/catalogs/4/languages
        //[HttpPost]
        //[ResponseType(typeof(void))]
        //[Route("{id}/languages")]
        //public IHttpActionResult UpdateCatalogLanguages(string id, [FromBody]webModel.CatalogLanguage[] languages)
        //{
        //    var catalog = _catalogService.GetById(id);

        //    if (catalog == null)
        //    {
        //        return NotFound();
        //    }

        //    var catalogModel = catalog.ToWebModel();
        //    var catalogLanguages = languages.Where(x => !string.IsNullOrEmpty(x.CatalogId));
        //    catalogModel.Languages.SetItems(catalogLanguages);

        //    UpdateCatalog(catalogModel);

        //    return Ok();
        //}

		// GET: api/catalog/catalogs/getnew
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
		[Route("getnew")]
        [CheckPermission(Permission = PredefinedPermissions.CatalogsManage)]
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
        // GET: api/catalogs/getnewvirtualcatalog
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
		[Route("getnewvirtual")]
        [CheckPermission(Permission = PredefinedPermissions.VirtualCatalogsManage)]
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

		// POST: api/catalog/catalogs
		[HttpPost]
		[ResponseType(typeof(webModel.Catalog))]
		[Route("")]
        [CheckPermission(Permissions = new[] { PredefinedPermissions.CatalogsManage, PredefinedPermissions.VirtualCatalogsManage })]
		public IHttpActionResult Create(webModel.Catalog catalog)
		{
            if ((_permissionService.UserHasAnyPermission(RequestContext.Principal.Identity.Name, PredefinedPermissions.CatalogsManage) && !catalog.Virtual)
                || (_permissionService.UserHasAnyPermission(RequestContext.Principal.Identity.Name, PredefinedPermissions.VirtualCatalogsManage) && catalog.Virtual))
            {
			var retVal = _catalogService.Create(catalog.ToModuleModel());
			return Ok(retVal.ToWebModel());
		}
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

		// PUT: api/catalog/catalogs
		[HttpPut]
        [ResponseType(typeof(void))]
		[Route("")]
        [CheckPermission(Permissions = new[] { PredefinedPermissions.CatalogsManage, PredefinedPermissions.VirtualCatalogsManage })]
        public IHttpActionResult Update(webModel.Catalog catalog)
        {
			UpdateCatalog(catalog);
            return StatusCode(HttpStatusCode.NoContent);
        }
      

        // DELETE: api/catalogs/5
		[HttpDelete]
        [ResponseType(typeof(void))]
		[Route("{id}")]
        [CheckPermission(Permissions = new[] { PredefinedPermissions.CatalogsManage, PredefinedPermissions.VirtualCatalogsManage })]
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

        private IEnumerable<webModel.CatalogLanguage> GetSystemLanguages()
        {
            var retVal = new List<webModel.CatalogLanguage>();

			var languages = _settingManager.GetArray<string>("VirtoCommerce.Core.Languages", new string[] { "en-US" });

			foreach (var languageCode in languages)
						{
							retVal.Add(new webModel.CatalogLanguage(languageCode));
						}

            return retVal;
        }
    }
}
