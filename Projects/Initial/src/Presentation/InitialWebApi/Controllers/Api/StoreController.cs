using System.Linq;
using System.Web.Http;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Stores.Model;

namespace Initial.WebApi.Controllers.Api
{
    public class StoreController : ApiController
    {
        private readonly StoreClient _storeClient;

        public StoreController(StoreClient storeClient)
        {
            _storeClient = storeClient;
        }

        [Queryable]
        [HttpGet]
        public IQueryable<Store> Get(string id = null)
        {
            return _storeClient.GetStores().Where(s=>s.StoreId == id || string.IsNullOrEmpty(id)).AsQueryable();
        }
    }
}
