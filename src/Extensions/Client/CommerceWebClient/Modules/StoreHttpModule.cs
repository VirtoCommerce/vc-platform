using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Stores.Model;
using System.Web.Mvc;
using System.Configuration;
using VirtoCommerce.Client;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Modules
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
            if (ConnectionHelper.IsDatabaseInstalled)
            {
                context.BeginRequest += OnBeginRequest;
                context.PostAcquireRequestState += OnPostAcquireRequestState;
                context.AuthenticateRequest += OnAuthenticateRequest;
            }
            else
            {
                context.BeginRequest +=
                    delegate
                    {
                        if (!HttpContext.Current.Request.RawUrl.EndsWith("/virto/admin"))
                        {
                            HttpContext.Current.Response.Redirect("~/virto/admin");
                        }
                    };
            }
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
            var store = GetStore(context);

            if (IsRequestAuthenticated(context))
            {
                session.IsRegistered = true;
                session.Username = GetRequestUserName(context);

                string errorMessage;
                if (!StoreHelper.IsUserAuthorized(session.Username, out errorMessage))
                {
                    if (store.StoreState == StoreState.Closed.GetHashCode())
                    {
                        throw new HttpException(403, "Store Closed");
                    }

                    OnUnauthorized(context);
                }
                var account = StoreHelper.UserClient.GetAccountByUserName(session.Username);
                if (account != null)
                {
                    session.CustomerId = account.MemberId;
                    var customer = StoreHelper.UserClient.GetCustomer(account.MemberId);
                    if (customer != null)
                    {
                        session.IsFirstTimeBuyer =
                            !customer.ContactPropertyValues.Any(x => x.Name == ContactPropertyValueName.LastOrder && x.DateTimeValue.HasValue);
                    }
                }
                var contact = StoreHelper.UserClient.GetCurrentCustomer();
                session.CustomerName = contact != null ? contact.FullName : session.Username;
            }
            else
            {
                //Redirect to login page users that are not authenticated but try to navigate to restricted store
                if (store.StoreState == StoreState.RestrictedAccess.GetHashCode())
                {
                    var loginUrlWithoutAppPath = FormsAuthentication.LoginUrl.Substring(context.Request.ApplicationPath.Length);
                    if (!HttpContext.Current.Request.RawUrl.EndsWith(loginUrlWithoutAppPath, StringComparison.InvariantCultureIgnoreCase) &&
                        !HttpContext.Current.Request.RawUrl.EndsWith("Account/Register", StringComparison.InvariantCultureIgnoreCase))
                    {
                        HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                    }
                }
                else if (store.StoreState == StoreState.Closed.GetHashCode())
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
            var store = GetStore(context);

            if (string.IsNullOrEmpty(session.Language)
                || !store.Languages.Any(s => string.Equals(s.LanguageCode, session.Language, StringComparison.InvariantCultureIgnoreCase)))
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
            var store = GetStore(context);

            if (store == null)
            {
                throw new HttpException(404, "Store Not Found");
            }

            var session = CustomerSession;
            session.StoreId = store.StoreId;

            var currency = GetStoreCurrency(context, store.DefaultCurrency);
            session.Currency = currency;
            session.StoreName = store.Name;
            session.CatalogId = store.Catalog;

            // now save store in the cookie
            StoreHelper.SetCookie(StoreCookie, session.StoreId, DateTime.Now.AddMonths(1), false);
            StoreHelper.SetCookie(CurrencyCookie, currency, DateTime.Now.AddMonths(1));           
        }

		#region Helper Methods
        /// <summary>
        /// Redirects to login.
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void RedirectToLogin(HttpContext context)
        {
            var loginUrlWithoutAppPath = FormsAuthentication.LoginUrl.Substring(context.Request.ApplicationPath.Length);
            if (!context.Request.RawUrl.EndsWith(loginUrlWithoutAppPath, StringComparison.InvariantCultureIgnoreCase) &&
                !context.Request.RawUrl.EndsWith("Account/Register", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.Redirect(FormsAuthentication.LoginUrl);
            }
        }

        /// <summary>
        /// Called when [unauthorized].
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void OnUnauthorized(HttpContext context)
        {
            //WebSecurity.Logout();
            FormsAuthentication.SignOut(); // it is ok to use this here, since that is what WebSecurity calls anyway
            // now check if store is accessible
            context.Response.Redirect(context.Request.RawUrl);
        }

        /// <summary>
        /// Determines whether [is request authenticated] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if [is request authenticated] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsRequestAuthenticated(HttpContext context)
        {
            return context.Request.IsAuthenticated;
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
			var storeid = context.Request.QueryString["store"];
			var storeClient = DependencyResolver.Current.GetService<StoreClient>();
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
				store = storeClient.GetStoreById(storeid);

				if (store != null)
				{
					if (store.StoreState != (int) StoreState.Closed)
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
					store = storeClient.GetStoreById(storeid);
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
        /// <param name="defaultCurrency">The default currency.</param>
        /// <returns>
        /// System.String.
        /// </returns>
		protected virtual string GetStoreCurrency(HttpContext context, string defaultCurrency)
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
					currency = defaultCurrency;
				}
			}

			return currency;
		}
		#endregion
	}
}
