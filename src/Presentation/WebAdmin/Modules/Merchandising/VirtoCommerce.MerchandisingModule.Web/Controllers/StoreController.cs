using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model.Store;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/stores")]
    public class StoreController : BaseController
    {
        private readonly Func<IStoreRepository> _storeRepository;
        private readonly Func<IFoundationAppConfigRepository> _appConfigRepFactory;

        public StoreController(Func<IStoreRepository> storeRepository, Func<IFoundationAppConfigRepository> appConfigRepFactory)
            : base(storeRepository)
        {
            _storeRepository = storeRepository;
            _appConfigRepFactory = appConfigRepFactory;
        }

        [HttpGet]
        [ResponseType(typeof(Store[]))]
        [Route("")]
        public IHttpActionResult GetStores()
        {
            var retVal = new List<Store>();

            var stores = GetAllStores();
            if (stores.Any())
            {
                using (var appConfig = _appConfigRepFactory())
                {
                    retVal.AddRange(stores.Select(store => store.ToWebModel(appConfig.GetAllSeoInformation(store.StoreId))));
                }
            }

            return Ok(retVal.ToArray());
        }

    }

}
