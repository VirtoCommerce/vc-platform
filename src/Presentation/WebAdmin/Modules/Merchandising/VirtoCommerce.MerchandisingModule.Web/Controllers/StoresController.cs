using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.MerchandisingModule.Model;
using VirtoCommerce.MerchandisingModule.Models;
using VirtoCommerce.MerchandisingModule.Services;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    using System.Linq;

    using VirtoCommerce.Foundation.Search.Schemas;

    [RoutePrefix("api/stores/{store}/{language?}")]
    public class StoresController : ApiController
    {
        private readonly IItemBrowsingService _browsingService = null;

        public StoresController(IItemBrowsingService browsingService)
        {
            _browsingService = browsingService;
        }


        [HttpGet]
        [Route("products/{code}")]
        public Product GetProduct(string code, string store, 
            string language = "en-us", [FromUri] string outline = "", [FromUri] string groups = "",
            [FromUri] string currency = "USD")
        {
            var rg = ParseResponseGroup(groups);
            return _browsingService.GetItem<Product>(code, rg); 
        }

        [HttpGet]
        [Route("products")]
        public ResponseCollection<Product> GetProducts(string store, [ModelBinder] SearchParameters parameters, string language = "en-us", [FromUri]string outline = "", [FromUri] string groups = "", [FromUri] string currency = "USD")
        {
            var criteria = new CatalogItemSearchCriteria();

            var catalog = GetCatalogFromStore(store);
            if (!String.IsNullOrEmpty(outline)) criteria.Outlines.Add(String.Format("{0}{1}*", catalog, outline));
            criteria.Locale = language;
            criteria.Catalog = catalog;

            // apply vendor filter if one specified
            if (parameters.Terms != null && parameters.Terms.Count > 0)
            {
                foreach (var term in parameters.Terms)
                {
                    var termFilter = new AttributeFilter()
                                       {
                                           Key = term.Key,
                                           Values = term.Value.Select(x => new AttributeFilterValue() { Id = x.ToLowerInvariant(), Value = x.ToLowerInvariant() }).ToArray()
                                       };

                    criteria.Apply(termFilter);
                }
            }

            criteria.ClassTypes.Add("Product");
            criteria.RecordsToRetrieve = parameters.PageSize == 0 ? 10 : parameters.PageSize;
            criteria.StartingRecord = (parameters.PageIndex - 1) * criteria.RecordsToRetrieve;
            criteria.Pricelists = null;//UserHelper.CustomerSession.Pricelists;
            criteria.Currency = currency;

            var rg = ParseResponseGroup(groups);
            var response = _browsingService.SearchItems<Product>(criteria,rg);
            return response;
        }

        [HttpGet]
        [Route("categories")]
        public ResponseCollection<Category> GetCategories(string store, string language = "en-us", [FromUri]string outline = "")
        {
            var catalog = GetCatalogFromStore(store);
            var response = _browsingService.GetCategories(String.Format("{0}/{1}", catalog, outline), language);
            return response;
        }

        private static ItemResponseGroups ParseResponseGroup(string responseGroup)
        {
            var retVal = ItemResponseGroups.ItemInfo;
            if (!String.IsNullOrEmpty(responseGroup))
            {
                Enum.TryParse(responseGroup, true, out retVal);
            }
            return retVal;
        }

        private string GetCatalogFromStore(string storeId)
        {
            return "vendorvirtual";
        }
    }
}
