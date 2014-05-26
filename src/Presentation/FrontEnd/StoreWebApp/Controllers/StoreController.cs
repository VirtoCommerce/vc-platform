using System;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Extensions;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class StoreController.
    /// </summary>
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
        public ActionResult Currencies()
        {
            var store = _storeClient.GetCurrentStore();

            var currencies =
                (from c in store.Currencies select new CurrencyModel(c.CurrencyCode, c.CurrencyCode.GetCurrencyName())).ToArray();

            var currenciesModel = new CurrenciesModel(UserHelper.CustomerSession.Currency, currencies);
            return PartialView("Currencies", currenciesModel);
        }

        /// <summary>
        /// Shows the available store picker
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly, DonutOutputCache(CacheProfile = "StoreCache", VaryByParam = Constants.Language, AnonymousOnly = true)]
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
                                  Id = store.StoreId,
                                  Name = store.Name,
                                  Url = String.IsNullOrEmpty(store.Url)
                                      ? Url.RouteUrl("Store", new { store = store.StoreId })
                                      : store.Url
                              }).ToArray(),
                    SelectedStore = UserHelper.CustomerSession.StoreId,
                    SelectedStoreName = UserHelper.CustomerSession.StoreName
                };

                return model.Stores.Any() ? PartialView("StorePicker", model) : null;
            }
            return null;
        }

        /// <summary>
        /// Quicks the access.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[DonutOutputCache(CacheProfile = "StoreCache", Duration = 0)]
        public ActionResult QuickAccess()
        {
            var wishListHelper = new CartHelper(CartHelper.WishListName);
            var cartHelper = new CartHelper(CartHelper.CartName);
            return PartialView("QuickAccess",
                               (new
                                   {
                                       Cart = cartHelper.CreateCartModel(true),
                                       WishList = wishListHelper.CreateCartModel(true),
                                       UserHelper.CustomerSession.CustomerName
                                   }).ToExpando());
        }

        /// <summary>
        /// Quicks the access.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[ChildActionOnly, DonutOutputCache(CacheProfile = "StoreCache", Duration = 0)]
        public ActionResult CartOptions()
        {
            var compareListHelper = new CartHelper(CartHelper.CompareListName);
            var cartHelper = new CartHelper(CartHelper.CartName);
            return PartialView("CartOptions",
                               (new
                               {
                                   Cart = cartHelper.CreateCartModel(true),
                                   CompareList = compareListHelper.CreateCompareModel()
                               }).ToExpando());
        }


        [ChildActionOnly, DonutOutputCache(CacheProfile = "LayoutStatic")]
        public ActionResult Footer()
        {
            return PartialView("_Footer");
        }

        [ChildActionOnly, DonutOutputCache(CacheProfile = "LayoutStatic", VaryByParam = Constants.Store + ";" + Constants.Language, VaryByCustom = "registered")]
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
            if (UserHelper.CustomerSession.IsRegistered)
            {
                string errorMessage;
                return StoreHelper.IsUserAuthorized(UserHelper.CustomerSession.Username, store.StoreId, out errorMessage);
            }

            return store.StoreState == StoreState.Open.GetHashCode() ||
                   store.StoreState == StoreState.RestrictedAccess.GetHashCode();
        }

        #endregion

    }
}