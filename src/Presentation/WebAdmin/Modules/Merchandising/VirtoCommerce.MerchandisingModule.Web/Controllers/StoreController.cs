using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model.Store;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/stores")]
    public class StoreController : ApiController
    {
        private readonly Func<IStoreRepository> _storeRepository;
        private readonly Func<IFoundationAppConfigRepository> _appConfigRepFactory;

        public StoreController(Func<IStoreRepository> storeRepository, Func<IFoundationAppConfigRepository> appConfigRepFactory)
        {
            _storeRepository = storeRepository;
            _appConfigRepFactory = appConfigRepFactory;
        }

        [HttpGet]
        [ResponseType(typeof (Store[]))]
        [Route("")]
        public IHttpActionResult GetStores()
        {
            var retVal = new List<Store>();
            using (var repository = _storeRepository())
            {
                var stores = repository.Stores.ExpandAll().ToArray();
                if (stores.Any())
                {
                    using (var appConfig = _appConfigRepFactory())
                    {
                        retVal.AddRange(stores.Select(store => store.ToWebModel(appConfig.GetAllSeoInformation(store.StoreId))));
                    }
                }
            }

            return Ok(retVal.ToArray());
        }

    }

}
