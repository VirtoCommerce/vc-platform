using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Common;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.MerchandisingModule.Web.Model.Stores;
using foundation = VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/stores")]
    public class StoreController : BaseController
    {
        #region Fields

        private readonly Func<IFoundationAppConfigRepository> _appConfigRepFactory;
        private readonly Func<IStoreRepository> _storeRepository;

        #endregion

        #region Constructors and Destructors

        public StoreController(
            Func<IStoreRepository> storeRepository,
            Func<IFoundationAppConfigRepository> appConfigRepFactory,
            ISettingsManager settingsManager,
            ICacheRepository cache)
            : base(storeRepository, settingsManager, cache)
        {
            this._storeRepository = storeRepository;
            this._appConfigRepFactory = appConfigRepFactory;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        [ResponseType(typeof(Store[]))]
        [ClientCache(Duration = 60)]
        [Route("")]
        public IHttpActionResult GetStores()
        {
            var retVal = new List<Store>();

            foundation.Store[] stores;
            using (var repository = this._storeRepository())
            {
                stores = repository.Stores.ExpandAll().ToArray();
            }

            if (stores.Any())
            {
                using (var appConfig = this._appConfigRepFactory())
                {
                    retVal.AddRange(
                        stores.Select(store => store.ToWebModel(appConfig.GetAllSeoInformation(store.StoreId))));
                }
            }

            return this.Ok(retVal.ToArray());
        }

        #endregion
    }
}
