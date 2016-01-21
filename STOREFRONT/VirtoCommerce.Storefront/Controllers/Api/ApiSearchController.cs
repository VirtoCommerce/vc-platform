using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [RoutePrefix("storefrontapi/search")]
    public class ApiSearchController : ApiControllerBase
    {
        public ApiSearchController(WorkContext workContext, ICatalogSearchService catalogSearchService)
            : base(workContext, catalogSearchService)
        {
        }
        
        //search by keyword
        [Route("")]
        public async Task<CatalogSearchResult> GetSearchProducts()
        {
            WorkContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(WorkContext.CurrentCatalogSearchCriteria);
            return WorkContext.CurrentCatalogSearchResult;
        }

        //Load categories for main page
        [Route("categories")]
        public async Task<CatalogSearchResult> GetTopCategories()
        {
            var catalogSearchCriteria = new CatalogSearchCriteria
            {
                CatalogId = WorkContext.CurrentStore.Catalog,
                ResponseGroup = CatalogSearchResponseGroup.WithCategories,
                SortBy = "Priority",
                SearchInChildren = false
            };
            
            WorkContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(catalogSearchCriteria);
            return WorkContext.CurrentCatalogSearchResult;
        }

        //Load products for category
        [Route("products")]
        public async Task<CatalogSearchResult> GetCategoryProducts(string categoryId)
        {
            WorkContext.CurrentCatalogSearchCriteria.CategoryId = categoryId;

            WorkContext.CurrentCatalogSearchResult = await _catalogSearchService.SearchAsync(WorkContext.CurrentCatalogSearchCriteria);
            return WorkContext.CurrentCatalogSearchResult;
        }
        
        //Load product details
        [Route("products/{productId}")]
        public async Task<Product> GetProduct(string productId)
        {
            WorkContext.CurrentProduct = (await _catalogSearchService.GetProductsAsync(new[] { productId }, ItemResponseGroup.ItemLarge)).FirstOrDefault();
            return WorkContext.CurrentProduct;
        }

        //Load categories for main page
        [Route("common/getcountries")]
        public Country[] GetCountries()
        {
            //_countriesWithoutRegions = workContext.AllCountries
            //    .Select(c => new Country { Name = c.Name, Code2 = c.Code2, Code3 = c.Code3 })
            //    .ToArray();

            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(GetRegionInfo)
                .Where(r => r != null)
                .ToList();

            var countriesJson = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/countries.json"));
            var countriesDict = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(countriesJson);

            var countries = countriesDict
                .Select(kvp => ParseCountry(kvp, regions))
                .Where(c => c.Code3 != null)
                .ToArray();

            return countries;
        }

        private static RegionInfo GetRegionInfo(CultureInfo culture)
        {
            RegionInfo result = null;

            try
            {
                result = new RegionInfo(culture.LCID);
            }
            catch
            {
                // ignored
            }

            return result;
        }

        private static Country ParseCountry(KeyValuePair<string, JObject> pair, List<RegionInfo> regions)
        {
            var region = regions.FirstOrDefault(r => string.Equals(r.EnglishName, pair.Key, StringComparison.OrdinalIgnoreCase));

            var country = new Country
            {
                Name = pair.Key,
                Code2 = region != null ? region.TwoLetterISORegionName : string.Empty,
                Code3 = region != null ? region.ThreeLetterISORegionName : string.Empty,
            };

            var provinceCodes = pair.Value["province_codes"].ToObject<Dictionary<string, string>>();
            if (provinceCodes != null && provinceCodes.Any())
            {
                country.Regions = provinceCodes
                    .Select(kvp => new CountryRegion { Name = kvp.Key, Code = kvp.Value })
                    .ToArray();
            }

            return country;
        }
    }
}