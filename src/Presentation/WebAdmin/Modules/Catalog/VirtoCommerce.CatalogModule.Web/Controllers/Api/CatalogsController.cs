using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using Microsoft.Practices.Unity;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class CatalogsController : ApiController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICatalogSearchService _searchService;
        private readonly IAppConfigRepository _appConfigRepository;

        public CatalogsController([Dependency("Catalog")]ICatalogService catalogService,
								  [Dependency("Catalog")]ICatalogSearchService itemSearchService,
								  [Dependency("Catalog")]IAppConfigRepository appConfigRepository)
        {
            _catalogService = catalogService;
            _searchService = itemSearchService;
            _appConfigRepository = appConfigRepository;
        }

        // GET: api/catalogs/itemssearch
        [ResponseType(typeof(webModel.Catalog[]))]
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

        // GET: api/catalogs/5
        [ResponseType(typeof(webModel.Catalog))]
        public IHttpActionResult Get(string id)
        {
            var catalog = _catalogService.GetById(id);
            if (catalog == null)
            {
                return NotFound();
            }
            return Ok(catalog.ToWebModel());
        }

        [HttpGet]
        public IHttpActionResult GetCatalogLanguages(string id)
        {
            var catalog = _catalogService.GetById(id);
            if (catalog == null)
                return NotFound();

            var retVal = catalog.ToWebModel().Languages;

            var systemLanguages = GetSystemLanguages();
            foreach (var systemLanguage in systemLanguages)
            {
                var alreadyExistLanguage = retVal.FirstOrDefault(x => string.Equals(x.LanguageCode, systemLanguage.LanguageCode, StringComparison.CurrentCultureIgnoreCase));
                if (alreadyExistLanguage == null)
                {
                    retVal.Add(systemLanguage);
                }
            }
            return Ok(retVal.OrderBy(x => x.DisplayName));
        }

        // POST: api/catalogs/updateCatalogLanguages
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCatalogLanguages(string catalogId, webModel.CatalogLanguage[] languages)
        {
            var catalog = _catalogService.GetById(catalogId);

            if (catalog == null)
            {
                return NotFound();
            }

            var catalogModel = catalog.ToWebModel();
            var catalogLanguages = languages.Where(x => !string.IsNullOrEmpty(x.CatalogId));
            catalogModel.Languages.SetItems(catalogLanguages);

            UpdateCatalog(catalogModel);

            return Ok();
        }

        // GET: api/catalogs/getnewcatalog
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
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
                        LanguageCode = "en-us"
                    }
                }
            };

            retVal = _catalogService.Create(retVal.ToModuleModel()).ToWebModel();
            return Ok(retVal);
        }
        // GET: api/catalogs/getnewvirtualcatalog
        [HttpGet]
        [ResponseType(typeof(webModel.Catalog))]
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
                        LanguageCode = "en-us"
                    }
                }
            };
            retVal = _catalogService.Create(retVal.ToModuleModel()).ToWebModel();
            return Ok(retVal);
        }

        // PUT: api/catalogs
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(webModel.Catalog catalog)
        {
            UpdateCatalog(catalog);
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Catalogs
        [ResponseType(typeof(webModel.Catalog))]
        public IHttpActionResult Post(webModel.Catalog catalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UpdateCatalog(catalog);

            return CreatedAtRoute("DefaultApi", new { id = catalog.Id }, catalog);
        }

        // DELETE: api/catalogs/5
        [ResponseType(typeof(void))]
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

			if (_appConfigRepository != null)
			{
				var languageSetting = _appConfigRepository.Settings.Expand(x => x.SettingValues).FirstOrDefault(x => x.Name.Equals("Languages"));
				if (languageSetting != null)
				{
					foreach (var languageCode in languageSetting.SettingValues.Select(x => x.ToString()))
					{
						retVal.Add(new webModel.CatalogLanguage(languageCode));
					}
				}
			}
            return retVal;
        }
    }
}
