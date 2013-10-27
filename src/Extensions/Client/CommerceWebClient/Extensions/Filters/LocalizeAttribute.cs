using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.Web.Client.Extensions.Filters
{

	/// <summary>
	/// Class LocalizeAttribute.
	/// </summary>
	public class LocalizeAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Gets the customer session.
		/// </summary>
		/// <value>The customer session.</value>
		private static ICustomerSession CustomerSession
		{
			get
			{
				var session = ServiceLocator.Current.GetInstance<ICustomerSessionService>();
				return session.CustomerSession;
			}
		}

		/// <summary>
		/// Gets the store client.
		/// </summary>
		/// <value>The store client.</value>
		private static StoreClient StoreClient
		{
			get
			{
				return ServiceLocator.Current.GetInstance<StoreClient>();
			}
		}

		/// <summary>
		/// Called by the ASP.NET MVC framework before the action method executes.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			var lang = filterContext.RouteData.Values["lang"] as string;
			if (!string.IsNullOrWhiteSpace(lang))
			{
				// set the culture from the route data (url)
				try
				{
					var newCulture = CultureInfo.CreateSpecificCulture(lang);

					var store = StoreClient.GetCurrentStore();
					if (store.Languages.Any(
							s =>
							string.Equals(s.LanguageCode, newCulture.Name, StringComparison.InvariantCultureIgnoreCase)))
					{
						CustomerSession.Language = newCulture.Name;
					}
				}
				catch
				{
					//do not change language
				}
			}

			if (!string.IsNullOrWhiteSpace(CustomerSession.Language) && !Thread.CurrentThread.CurrentUICulture.Name.Equals(CustomerSession.Language, StringComparison.InvariantCultureIgnoreCase))
			{
				Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CustomerSession.Language);
			}
			// set the lang value into route data
			filterContext.RouteData.Values["lang"] = CustomerSession.Language;
		}
	}
}
