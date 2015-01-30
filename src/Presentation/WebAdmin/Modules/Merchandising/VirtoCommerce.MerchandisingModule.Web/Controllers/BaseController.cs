using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    public class BaseController : ApiController
    {
        private readonly Func<IStoreRepository> _storeRepository;

        public BaseController(Func<IStoreRepository> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        protected virtual Store[] GetAllStores()
        {
            using (var repository = _storeRepository())
            {
                var stores = repository.Stores.ExpandAll().ToArray();
                return stores;
            }
        }

        protected virtual string GetCatalogId(string storeId)
        {
            var stores = GetAllStores();

            if (stores.Any())
            {
                var store = stores.FirstOrDefault(x => x.StoreId.Equals(storeId, StringComparison.InvariantCultureIgnoreCase));
                if (store != null)
                {
                    return store.Catalog;
                }
            }

            throw new InvalidOperationException(string.Format("No store exits with id {0}", storeId));
        }
    }
}
