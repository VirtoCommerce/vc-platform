using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Profile;
using System.Web.Routing;
using System.Web.Security;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Extensions;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Binders;
using VirtoCommerce.Web.Virto.Helpers;
using VirtoCommerce.Web.Virto.Helpers.MVC;

namespace VirtoCommerce.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// Class MvcApplication.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (AppConfigConfiguration.Instance.Setup.IsCompleted)
            {
                //WebApiConfig.Register(GlobalConfiguration.Configuration);
                GlobalConfiguration.Configure(WebApiConfig.Register);

                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                MvcSiteMapProviderConfig.Register(DependencyResolver.Current);
               
                //AuthConfig.RegisterAuth();

                ModelBinders.Binders[typeof (SearchParameters)] = new SearchParametersBinder();
                ModelBinders.Binders[typeof(CategoryPathModel)] = new CategoryPathModelBinder();


                ModelValidatorProviders.Providers.RemoveAt(0);
                ModelValidatorProviders.Providers.Insert(0, new VirtoDataAnnotationsModelValidatorProvider());
            }
        }

        /// <summary>
        /// Profile migrate from anonymous.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="pe">The <see cref="ProfileMigrateEventArgs"/> instance containing the event data.</param>
        private void Profile_MigrateAnonymous(Object sender, ProfileMigrateEventArgs pe)
        {
            var client = ServiceLocator.Current.GetInstance<OrderClient>();
            var orders = client.GetAllCustomerOrders(pe.AnonymousID,
                                                     UserHelper.CustomerSession.StoreId);

            if (orders != null)
            {
                foreach (var order in orders)
                {
                    order.CustomerId = UserHelper.CustomerSession.CustomerId;
                    order.CustomerName = UserHelper.CustomerSession.CustomerName;
                }

                client.SaveChanges();
            }

            // Migrate shopping cart
            var cart = new CartHelper(CartHelper.CartName);
            var anonymousCart = new CartHelper(CartHelper.CartName, pe.AnonymousID);

            // Only perform merge if cart is not empty
            if (!anonymousCart.IsEmpty)
            {
                // Merge cart
                cart.Add(anonymousCart.Cart, true);
                cart.SaveChanges();

                // Delete anonymous cart
                anonymousCart.Delete();
                anonymousCart.SaveChanges();
            }

            var wishList = new CartHelper(CartHelper.WishListName);
            var anonymousWishList = new CartHelper(CartHelper.WishListName, pe.AnonymousID);

            // Only perform merge if cart is not empty
            if (!anonymousWishList.IsEmpty)
            {
                // Merge wish list
                wishList.Add(anonymousWishList.Cart, true);
                if (String.IsNullOrEmpty(wishList.Cart.BillingCurrency))
                {
                    wishList.Cart.BillingCurrency = UserHelper.CustomerSession.Currency;
                }
                wishList.SaveChanges();

                // Delete anonymous wish list
                anonymousWishList.Delete();
                anonymousWishList.SaveChanges();
            }

            //Delete the anonymous data from the database
            //ProfileManager.DeleteProfile(pe.AnonymousID);

            //Remove the anonymous identifier from the request so 
            //this event will no longer fire for a logged-in user
            AnonymousIdentificationModule.ClearAnonymousIdentifier();
        }

        /// <summary>
        /// Provides an application-wide implementation of the <see cref="P:System.Web.UI.PartialCachingAttribute.VaryByCustom" /> property.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that contains information about the current Web request.</param>
        /// <param name="custom">The custom string that specifies which cached response is used to respond to the current request.</param>
        /// <returns>If the value of the <paramref name="custom" /> parameter is "browser", the browser's <see cref="P:System.Web.Configuration.HttpCapabilitiesBase.Type" />; otherwise, null.</returns>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {

            var varyString = base.GetVaryByCustomString(context, custom) ?? string.Empty;

            //varyString += UserHelper.CustomerSession.Language;//allways vary by language

            if (SettingsHelper.OutputCacheEnabled)
            {
                foreach (var key in custom.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.Equals(key, "registered", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += UserHelper.CustomerSession.IsRegistered ? "registered" : "anonymous";
                    }
                    if (string.Equals(key, "storeparam", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += UserHelper.CustomerSession.StoreId;
                    }
                    if (string.Equals(key, "currency", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += UserHelper.CustomerSession.Currency;
                    }
                    if (string.Equals(key, "pricelist", StringComparison.OrdinalIgnoreCase))
                    {
                        varyString += CacheHelper.CreateCacheKey("_pl", UserHelper.CustomerSession.Pricelists);
                    }
                    if (string.Equals(key, "filters", StringComparison.OrdinalIgnoreCase))
                    {
                        var qs = Request.QueryString;
                        var pageSize = qs.AllKeys.Contains("pageSize") ? qs["pageSize"] : StoreHelper.GetCookieValue("pagesizecookie");
                        var sort = qs.AllKeys.Contains("sort") ? qs["sort"] : StoreHelper.GetCookieValue("sortcookie");
                        var sortorder = qs.AllKeys.Contains("sortorder") ? qs["sortorder"] : StoreHelper.GetCookieValue("sortordercookie") ?? "asc";

                        varyString += string.Format("filters_{0}_{1}_{2}", pageSize, sort, sortorder);
                    }

                    if (string.Equals(key, "cart", StringComparison.OrdinalIgnoreCase))
                    {
                        //This method is called from System.Web.Caching module before customerId set 
                        if (string.IsNullOrEmpty(UserHelper.CustomerSession.CustomerId))
                        {
                            if (UserHelper.CustomerSession.IsRegistered)
                            {
                                var account = UserHelper.UserClient.GetAccountByUserName(UserHelper.CustomerSession.Username);
                                if (account != null)
                                {
                                    UserHelper.CustomerSession.CustomerId = account.MemberId;
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(UserHelper.CustomerSession.CustomerId))
                        {
                            UserHelper.CustomerSession.CustomerId = context.Request.AnonymousID ??
                                                                    Guid.NewGuid().ToString();
                        }

                        var ch = new CartHelper(CartHelper.CartName);

                        if (ch.LineItems.Any())
                        {
                            varyString +=
                                new CartHelper(CartHelper.CartName).LineItems.Select(x => x.LineItemId)
                                                                   .Aggregate((x, y) => x + y);
                        }

                    }
                }
            }

            return varyString;
        }
    }
}