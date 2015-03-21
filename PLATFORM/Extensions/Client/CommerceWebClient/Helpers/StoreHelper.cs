using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks.Currencies;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.Web.Client.Helpers
{

    /// <summary>
    /// Class StoreHelper.
    /// </summary>
	public class StoreHelper
	{
        /// <summary>
        /// Gets the customer session.
        /// </summary>
        /// <value>The customer session.</value>
		public static ICustomerSession CustomerSession
		{
			get
			{
				var session = ServiceLocator.Current.GetInstance<ICustomerSessionService>();
				return session.CustomerSession;
			}
		}

        /// <summary>
        /// Gets the security service.
        /// </summary>
        /// <value>The security service.</value>
		public static ISecurityService SecurityService
		{
			get
			{
				return ServiceLocator.Current.GetInstance<ISecurityService>();
			}
		}

        /// <summary>
        /// Gets the currency service.
        /// </summary>
        /// <value>The currency service.</value>
		public static ICurrencyService CurrencyService
		{
			get
			{
				return ServiceLocator.Current.GetInstance<ICurrencyService>();
			}
		}

        /// <summary>
        /// Gets the user client.
        /// </summary>
        /// <value>The user client.</value>
		public static UserClient UserClient
		{
			get
			{
				return ServiceLocator.Current.GetInstance<UserClient>();
			}
		}

        /// <summary>
        /// Gets the store client.
        /// </summary>
        /// <value>The store client.</value>
		public static StoreClient StoreClient
		{
			get
			{
				return ServiceLocator.Current.GetInstance<StoreClient>();
			}
		}

        public static ISearchFilterService SearchFilter
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ISearchFilterService>();
            }
        }

		#region Authorization Methods

        /// <summary>
        /// Determines whether the specified user is authorized to access current site.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="errorMessage">reson why user is not authorized</param>
        /// <returns><c>true</c> if [is user authorized] [the specified user]; otherwise, <c>false</c>.</returns>
		public static bool IsUserAuthorized(string userName, out string errorMessage)
		{
			return IsUserAuthorized(userName, string.Empty, out errorMessage);
		}

        /// <summary>
        /// Determines whether [is user authorized] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="storeId">The store id. Empty means current store</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><c>true</c> if [is user authorized] [the specified user name]; otherwise, <c>false</c>.</returns>
		public static bool IsUserAuthorized(string userName, string storeId, out string errorMessage)
		{
			Account account;
			var isAuthorized = UserClient.IsAuthorized(userName, out account);
			errorMessage = null;

			if (isAuthorized && account != null)
			{
                if (account.RegisterType != (int)RegisterType.Administrator && account.RegisterType != (int)RegisterType.SiteAdministrator)
				{
					isAuthorized = StoreClient.IsLinkedAccountAuthorized(account.StoreId, storeId);

					//Check if user has access to current store
					if (isAuthorized)
					{
						var store = StoreClient.GetStoreById(CustomerSession.StoreId);

						if (store.StoreState == StoreState.Closed.GetHashCode())
						{
							isAuthorized = SecurityService.CheckMemberPermission(account.MemberId,
								new Permission{ PermissionId = PredefinedPermissions.ShopperClosedAccess});

							if (!isAuthorized)
							{
								var setting = store.Settings.SingleOrDefault(n => n.Name.Equals("StoreClosedMessage"));
								errorMessage = setting != null
									               ? setting.ShortTextValue
									               : "The store is temporarily closed for maintenance. Please try again later.";
							}
						}
						else if (store.StoreState == StoreState.RestrictedAccess.GetHashCode())
						{
							isAuthorized = SecurityService.CheckMemberPermission(account.MemberId,
							                                                     new Permission
								                                                     {
									                                                     PermissionId =
										                                                     PredefinedPermissions.ShopperRestrictedAccess
								                                                     });

							if (!isAuthorized)
							{
								var setting = store.Settings.SingleOrDefault(n => n.Name.Equals("StoreRestrictedMessage"));
								errorMessage = setting != null
									               ? setting.ShortTextValue
									               : "You do not have permissions to view this store";
							}
						}
					}
					else
					{
						errorMessage = "You do not have permissions to view this store";
					}
				}
			}
			else
			{
				errorMessage = "Account not authorized";
			}

			return isAuthorized;
		}

        public static string GetDefaultLanguageCode()
        {
            var store = StoreClient.GetCurrentStore();
            return store != null ? store.DefaultLanguage : "";
        }


		#endregion

        /// <summary>
        /// Attempt to format the currency based on the browser's locale, but if that currency
        /// is not in the database, then fallback to current thread's culture.
        /// </summary>
        /// <param name="amount">the amount to be formatted</param>
        /// <param name="currencyCode">currency code which will be used to find the
        /// effective culture</param>
        /// <returns>Formatted currency in String</returns>
		public static string FormatCurrency(decimal amount, string currencyCode)
		{
			return CurrencyService.FormatCurrency(amount, currencyCode);
		}

        /// <summary>
        /// Converts the currency.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyFrom">The currency from.</param>
        /// <param name="currencyTo">The currency to.</param>
        /// <returns>System.Decimal.</returns>
		public static decimal ConvertCurrency(decimal amount, string currencyFrom, ref string currencyTo)
		{
			return CurrencyService.ConvertCurrency(amount, currencyFrom, ref currencyTo);
		}

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>StoreSetting[][].</returns>
		public static StoreSetting[] GetSettings(string name)
		{
			return StoreClient.GetCurrentStore()
				.Settings.Where(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).ToArray();
		}

        /// <summary>
        /// Gets the setting value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetSettingValue<T>(string name, T defaultValue = default(T))
        {
            var setting = GetSettings(name).FirstOrDefault(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (setting != null)
            {
                return (T)setting.RawValue();
            }

            return defaultValue;
        }

		#region Cookie Management

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
		public static void SetCookie(string key, string val, DateTime? expires = null, bool prefix = true)
		{
			var cookieName = prefix ? MakeStoreCookieName(key) : key;
            Foundation.Customers.CustomerSession.SetCookie(cookieName, val, expires);
		}

        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        /// <returns>System.String.</returns>
		public static string GetCookieValue(string key, bool prefix = true)
			{
			var cookieName = prefix ? MakeStoreCookieName(key) : key;
            return Foundation.Customers.CustomerSession.GetCookieValue(cookieName);
		}

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
		public static void SetCookie(string key, NameValueCollection values, DateTime expires, bool prefix = true)
		{
			var cookieName = prefix ? MakeStoreCookieName(key) : key;
			var httpCookie = HttpContext.Current.Request.Cookies.Get(cookieName) ?? new HttpCookie(cookieName);

			// Set cookie value
			httpCookie.Values.Clear();
			httpCookie.Values.Add(values);

			httpCookie.Expires = expires;
			HttpContext.Current.Response.Cookies.Set(httpCookie);
		}


        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
        /// <returns>NameValueCollection.</returns>
		public static NameValueCollection GetCookie(string key, bool prefix = true)
		{
			var cookieName = prefix ? MakeStoreCookieName(key) : key;

			HttpCookie cookie = null;

			foreach (string cookieKey in HttpContext.Current.Response.Cookies.Keys)
			{
				if (cookieName.Equals(cookieKey, StringComparison.OrdinalIgnoreCase))
					cookie = HttpContext.Current.Response.Cookies.Get(cookieName);
			}

			if (cookie != null)
				return cookie.Values;

			cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
			return cookie != null ? cookie.Values : null;
		}

        /// <summary>
        /// Clears the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="prefix">if set to <c>true</c> [prefix].</param>
		public static void ClearCookie(string key, string value, bool prefix = true)
		{
			var cookieName = prefix ? MakeStoreCookieName(key) : key;
			var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
			if (cookie != null)
			{
				if (String.IsNullOrEmpty(value))
				{
					cookie.Values.Clear();
					cookie.Expires = DateTime.Now.AddYears(-1);
					HttpContext.Current.Response.Cookies.Set(cookie);
				}
				else
				{
					var values = cookie.Values.GetValues(value);
					if (values != null && values.Length > 0)
					{
						cookie.Values.Remove(value);
						cookie.Expires = DateTime.Now.AddYears(-1);
						HttpContext.Current.Response.Cookies.Set(cookie);
					}
				}
			}
		}

        /// <summary>
        /// Makes the name of the store cookie.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <returns>System.String.</returns>
		private static string MakeStoreCookieName(string baseName)
		{
			return baseName + CustomerSession.StoreId;
		}
		#endregion
	}
}