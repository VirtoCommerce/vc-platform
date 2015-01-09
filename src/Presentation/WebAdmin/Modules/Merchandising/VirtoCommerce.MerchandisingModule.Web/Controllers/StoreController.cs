using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.MerchandisingModule.Web.Model.Store;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{language}/stores")]
    public class StoreController : ApiController
    {
        private readonly Func<IStoreRepository> _storeRepository;

        public StoreController(Func<IStoreRepository> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpGet]
        [ResponseType(typeof (Store[]))]
        [Route("")]
        public IHttpActionResult GetStores(string language = "en-us")
        {
            using (var repository = _storeRepository())
            {
                var stores = repository.Stores.ExpandAll().ToArray();
                var retVal = stores.Select(x => x.ToWebModel());
                return Ok(retVal);
            }
        }
    }

}
