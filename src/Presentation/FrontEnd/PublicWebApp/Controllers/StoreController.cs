using System;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Extensions.Routing;
using VirtoCommerce.Web.Core.DataContracts.Store;
using VirtoCommerce.Web.Models;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class StoreController.
    /// </summary>
    [RoutePrefix("stores")]
    public class StoreController : ControllerBase
    {
        /// <summary>
        /// The _store client
        /// </summary>
        private readonly StoreClient _storeClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreController"/> class.
        /// </summary>
        /// <param name="storeClient">The store client.</param>
        public StoreController(StoreClient storeClient)
        {
            _storeClient = storeClient;
        }

        /// <summary>
        /// Show available currencies
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly, DonutOutputCache(CacheProfile = "StoreCache", VaryByParam = Constants.Language, VaryByCustom = "currency")]
        [Route("currencies")]
        public ActionResult Currencies()
        {
            var store = _storeClient.GetCurrentStore();

            var currencies =
                (from c in store.Currencies select new CurrencyModel(c, c.GetCurrencyName())).ToArray();

            var currenciesModel = new CurrenciesModel(StoreHelper.CustomerSession.Currency, currencies);
            return PartialView("Currencies", currenciesModel);
        }

        /// <summary>
        /// Shows the available store picker
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly, DonutOutputCache(CacheProfile = "StoreCache", VaryByParam = Constants.Language, AnonymousOnly = true)]
        [Route("")]
        public ActionResult StorePicker()
        {
            var stores = _storeClient.GetStores();

            if (stores.Any())
            {
                var model = new StoresModel
                {
                    Stores = (from store in stores
                              where IsStoreVisible(store)
                              select new StoreModel
                              {
                                  Id = store.Id,
                                  Name = store.Name,
                                  Url = String.IsNullOrEmpty(store.Url)
                                      ? Url.RouteUrl("Store", new { store = store.Id })
                                      : store.Url
                              }).ToArray(),
                    SelectedStore = StoreHelper.StoreClient.GetCurrentStore().Id,
                    SelectedStoreName = StoreHelper.CustomerSession.StoreName
                };

                return model.Stores.Any() ? PartialView("StorePicker", model) : null;
            }
            return null;
        }

        /// <summary>
        /// Quicks the access.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [DonutOutputCache(CacheProfile = "StoreCache", Duration = 0)]
        [Route("quickaccess")]
        public ActionResult QuickAccess()
        {
            //var wishListHelper = new CartHelper(CartHelper.WishListName);
            //var cartHelper = new CartHelper(CartHelper.CartName);
            return PartialView("QuickAccess",
                               (new
                                   {
                                       //Cart = cartHelper.CreateCartModel(true),
                                       //WishList = wishListHelper.CreateCartModel(true),
                                       StoreHelper.CustomerSession.CustomerName
                                   }).ToExpando());

            return null;
        }

        /// <summary>
        /// Quicks the access.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[ChildActionOnly, DonutOutputCache(CacheProfile = "StoreCache", Duration = 0)]
        [Route("quickcart")]
        public ActionResult CartOptions()
        {
            //var compareListHelper = new CartHelper(CartHelper.CompareListName);
            //var cartHelper = new CartHelper(CartHelper.CartName);
            //return PartialView("CartOptions",
            //                   (new
            //                   {
            //                       Cart = cartHelper.CreateCartModel(true),
            //                       CompareList = compareListHelper.CreateCompareModel()
            //                   }).ToExpando());

            return null;
        }


        [ChildActionOnly, DonutOutputCache(CacheProfile = "LayoutStatic")]
        [Route("footer")]
        public ActionResult Footer()
        {
            return PartialView("_Footer");
        }

        [ChildActionOnly, DonutOutputCache(CacheProfile = "LayoutStatic", VaryByParam = Constants.Store + ";" + Constants.Language, VaryByCustom = "registered")]
        [Route("menu")]
        public ActionResult Menu()
        {
            return PartialView("_Menu");
        }

        #region Private Helpers

        /// <summary>
        /// Determines whether [is store visible] [the specified store].
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns><c>true</c> if [is store visible] [the specified store]; otherwise, <c>false</c>.</returns>
        private bool IsStoreVisible(Store store)
        {
            //if (StoreHelper.CustomerSession.IsRegistered)
            //{
            //    string errorMessage;
            //    return StoreHelper.IsUserAuthorized(StoreHelper.CustomerSession.Username, store.Id, out errorMessage);
            //}

            return store.StoreState == StoreState.Open ||
                   store.StoreState == StoreState.RestrictedAccess;
        }

        #endregion

    }
}