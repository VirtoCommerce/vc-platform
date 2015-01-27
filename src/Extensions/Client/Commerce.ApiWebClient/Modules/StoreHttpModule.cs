using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Core.DataContracts.Store;

namespace VirtoCommerce.ApiWebClient.Modules
{
    /// <summary>
    /// Class StoreHttpModule.
    /// </summary>
    public class StoreHttpModule : BaseHttpModule
    {
        /// <summary>
        /// The store cookie
        /// </summary>
        protected virtual string StoreCookie { get { return "vcf.store"; } }
        /// <summary>
        /// The currency cookie
        /// </summary>
        protected virtual string CurrencyCookie { get { return "vcf.currency"; } }



        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public override void Dispose()
        {
            //clean-up code here.
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public override void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.PostAcquireRequestState += OnPostAcquireRequestState;
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        /// <summary>
        /// Called when [authenticate request].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnAuthenticateRequest(object sender, EventArgs e)
        {
            if (IsResourceFile())
                return;

            OnAuthenticateRequest(((HttpApplication)sender).Context);
        }

        /// <summary>
        /// Called when [authenticate request].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.Web.HttpException">
        /// 403;Store Closed
        /// or
        /// 403;Store Closed
        /// </exception>
        protected virtual void OnAuthenticateRequest(HttpContext context)
        {
            var session = CustomerSession;
            session.Language = GetLanguage(context);
            var store = GetStore(context);

            if (IsRequestAuthenticated(context))
            {
                session.IsRegistered = true;
                session.Username = GetRequestUserName(context);

                string errorMessage;
                if (!StoreHelper.IsUserAuthorized(session.Username, out errorMessage))
                {
                    if (store.StoreState == StoreState.Closed)
                    {
                        throw new HttpException(403, "Store Closed");
                    }

                    OnUnauthorized(context);
                }

                var userInfo = Task.Run(() => SecurityClient.GetUserInfo(session.Username)).Result;
                if (userInfo != null)
                {
                    session.CustomerId = userInfo.Id;
                    session.IsFirstTimeBuyer = userInfo.Properties != null &&
                                               userInfo.Properties.Any(x => x.Key == ContactPropertyValueName.LastOrder);
                    session.CustomerName = userInfo.FullName;

                }
            }
            else if (!context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                //Redirect to login page users that are not authenticated but try to navigate to restricted store
                if (store.StoreState == StoreState.RestrictedAccess)
                {
                    RedirectToLogin(context);
                }
                else if (store.StoreState == StoreState.Closed)
                {
                    throw new HttpException(403, "Store Closed");
                }
            }
        }

        /// <summary>
        /// Called when [post acquire request state].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            if (IsResourceFile())
                return;

            OnPostAcquireRequestState(((HttpApplication)sender).Context);
        }

        /// <summary>
        /// Called when [post acquire request state].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ApplicationException">AnonymousIdentificationModule.Enabled is set to false, for cart to work property it needs to be enabled</exception>
        protected virtual void OnPostAcquireRequestState(HttpContext context)
        {
            var session = CustomerSession;
            session.Language = GetLanguage(context);
            var store = GetStore(context);

            if (string.IsNullOrEmpty(session.Language)
                || !store.Languages.Any(s => string.Equals(s.ToSpecificLangCode(), session.Language.ToSpecificLangCode(), StringComparison.InvariantCultureIgnoreCase)))
            {
                session.Language = store.DefaultLanguage;
            }

            // set customer id to anonymousID if nothing is set, it might be overwritten if authentication is successful
            if (String.IsNullOrEmpty(session.CustomerId))
            {
                var id = Guid.NewGuid().ToString();
                if (context != null)
                {
                    if (AnonymousIdentificationModule.Enabled && context.Request.AnonymousID != null)
                    {
                        id = context.Request.AnonymousID;
                    }
                    else if (!AnonymousIdentificationModule.Enabled)
                    {
                        throw new ApplicationException("AnonymousIdentificationModule.Enabled is set to false, for cart to work property it needs to be enabled");
                    }
                }

                session.CustomerId = id;
            }
        }

        /// <summary>
        /// Called when [begin request].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnBeginRequest(object sender, EventArgs e)
        {
            if (IsResourceFile())
                return;

            OnBeginRequest(((HttpApplication)sender).Context);
        }

        /// <summary>
        /// Called when [begin request].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.Web.HttpException">404;Store Not Found</exception>
        protected virtual void OnBeginRequest(HttpContext context)
        {
            var session = CustomerSession;
            session.CategoryId = "";
            session.CategoryOutline = "";
            session.Language = GetLanguage(context);

            var store = GetStore(context);

            if (store == null)
            {
                throw new HttpException(404, "Store Not Found");
            }

            session.StoreId = store.Id;

            var currency = GetStoreCurrency(context, store);
            session.Currency = currency;
            session.StoreName = store.Name;
            session.CatalogId = store.Catalog;

            // now save store in the cookie
            StoreHelper.SetCookie(StoreCookie, session.StoreId, DateTime.Now.AddMonths(1), false);
            StoreHelper.SetCookie(CurrencyCookie, currency, DateTime.Now.AddMonths(1));

            if (context.Request.QueryString.AllKeys.Any(x => string.Equals(x, "loginas", StringComparison.OrdinalIgnoreCase)))
            {
                RedirectToLogin(context);
            }
        }

        #region Helper Methods
        /// <summary>
        /// Redirects to login.
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void RedirectToLogin(HttpContext context)
        {
            //TODO: try to find LoginPath from owin context
            if (!context.Request.Url.AbsolutePath.EndsWith("/Account/Logon", StringComparison.InvariantCultureIgnoreCase) &&
                !context.Request.Url.AbsolutePath.EndsWith("/Account/Register", StringComparison.InvariantCultureIgnoreCase) && !IsAjax)
            {
                context.Response.Redirect("~/Account/Logon" + context.Request.Url.Query);
            }
        }

        /// <summary>
        /// Called when [unauthorized].
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void OnUnauthorized(HttpContext context)
        {
            context.GetOwinContext().Authentication.SignOut();
            // now check if store is accessible
            context.Response.Redirect(context.Request.RawUrl);
        }

        /// <summary>
        /// Gets the name of the request user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected virtual string GetRequestUserName(HttpContext context)
        {
            return context.User.Identity.Name;
        }

        protected virtual string GetLanguage(HttpContext context)
        {
            var language = GetLanguageFromRoute(context.Request.RequestContext.RouteData.Values);

            if (string.IsNullOrEmpty(language))
            {
                language = GetLanguageFromUrl(context.Request.Url.Segments);
            }

            if (string.IsNullOrEmpty(language))
            {
                language = CustomerSession.Language;
            }

            return language;
        }

        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// Store.
        /// </returns>
        protected virtual Store GetStore(HttpContext context)
        {
            var loadDefault = true;
            var storeClient = DependencyResolver.Current.GetService<StoreClient>();
            var storeid = GetStoreIdFromRoute(context.Request.RequestContext.RouteData.Values);
            if (string.IsNullOrEmpty(storeid))
            {
                storeid = GetStoreIdFromUrl(context.Request.Url.Segments);
            }
            Store store = null;

            if (String.IsNullOrEmpty(storeid))
            {
                // try getting store from URL
                storeid = storeClient.GetStoreIdByUrl(context.Request.Url.AbsoluteUri);
                if (String.IsNullOrEmpty(storeid))
                {
                    // try getting store from the cookie
                    storeid = StoreHelper.GetCookieValue(StoreCookie, false);

                    // try getting default store from settings
                    if (String.IsNullOrEmpty(storeid))
                    {
                        storeid = ConfigurationManager.AppSettings["DefaultStore"];
                    }
                }

            }

            if (!String.IsNullOrEmpty(storeid))
            {
                store = storeClient.GetStore(storeid);

                if (store != null)
                {
                    if (store.StoreState != StoreState.Closed)
                    {
                        loadDefault = false;
                    }
                    else
                    {
                        store = null;
                    }
                }
            }


            if (store == null)
            {
                if (loadDefault)
                {
                    StoreHelper.ClearCookie(StoreCookie, String.Empty, false);
                    storeid = ConfigurationManager.AppSettings["DefaultStore"];
                    store = storeClient.GetStore(storeid);
                }
                else
                {
                    store = storeClient.GetStores().FirstOrDefault();
                }
            }

            return store;
        }

        /// <summary>
        /// Gets the store currency.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">current store</param>
        /// <returns>
        /// System.String.
        /// </returns>
        protected virtual string GetStoreCurrency(HttpContext context, Store store)
        {
            var currency = context.Request.QueryString["currency"];

            if (String.IsNullOrEmpty(currency))
            {
                currency = String.Empty;

                // try getting store from the cookie
                if (String.IsNullOrEmpty(currency))
                {
                    currency = StoreHelper.GetCookieValue(CurrencyCookie);
                }

                // try getting default store from settings
                if (String.IsNullOrEmpty(currency))
                {
                    currency = store.DefaultCurrency;
                }
            }
            //if currency is invalid use dafault
            else if (!store.Currencies.Any(c => c.Equals(currency, StringComparison.OrdinalIgnoreCase)))
            {
                currency = store.DefaultCurrency;
            }

            return currency.ToUpperInvariant();
        }
        #endregion

        private string GetStoreIdFromRoute(RouteValueDictionary values)
        {
            if (values != null && values.ContainsKey(Extensions.Routing.Constants.Store))
            {
                return values[Extensions.Routing.Constants.Store].ToString();
            }
            return null;
        }

        private string GetLanguageFromRoute(RouteValueDictionary values)
        {
            if (values != null && values.ContainsKey(Extensions.Routing.Constants.Language))
            {
                return values[Extensions.Routing.Constants.Language].ToString();
            }
            return null;
        }

        private string GetStoreIdFromUrl(IEnumerable<string> urlSegments)
        {
            var storeClient = DependencyResolver.Current.GetService<StoreClient>();

            foreach (var urlSegment in urlSegments)
            {
                var storeCandidate = HttpUtility.UrlDecode(urlSegment.Replace("/", ""));

                if (string.IsNullOrEmpty(storeCandidate))
                {
                    continue;
                }


                var foundStore = storeClient.GetStore(storeCandidate);

                if (foundStore != null)
                {
                    return foundStore.Id;
                }

            }

            return null;
        }

        private string GetLanguageFromUrl(IEnumerable<string> urlSegments)
        {

            foreach (var urlSegment in urlSegments)
            {
                var languageCandidate = HttpUtility.UrlDecode(urlSegment.Replace("/", ""));
                if (string.IsNullOrEmpty(languageCandidate))
                {
                    continue;
                }

                var constraintsRegEx = string.Format("^({0})$", Extensions.Routing.Constants.LanguageRegex);

                if (Regex.IsMatch(languageCandidate, constraintsRegEx, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    return languageCandidate.ToLowerInvariant();
                }

            }

            return null;
        }
    }
}
