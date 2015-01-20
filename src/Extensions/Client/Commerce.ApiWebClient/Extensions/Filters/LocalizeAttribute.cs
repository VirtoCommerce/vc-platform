using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace VirtoCommerce.ApiWebClient.Extensions.Filters
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Session;

    using StoreClient = VirtoCommerce.ApiWebClient.Clients.StoreClient;

    /// <summary>
	/// Class LocalizeAttribute.
	/// </summary>
	public class LocalizeAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Gets the customer session.
		/// </summary>
		/// <value>The customer session.</value>
        public ICustomerSession CustomerSession
        {
            get
            {
                return ClientContext.Session;
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

            var lang = filterContext.RouteData.Values[Routing.Constants.Language] as string;
			if (!string.IsNullOrWhiteSpace(lang))
			{
				// set the culture from the route data (url)
                try
                {
                    var newCulture = lang.ToSpecificLangCode();

                    var store = StoreClient.GetCurrentStore();
                    if (store.Languages.Any(
                            s =>
                            string.Equals(s.ToSpecificLangCode(), newCulture, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        CustomerSession.Language = newCulture;
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
                //Change CurrentCulture so that dates and numbers are formated too
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(CustomerSession.Language);
			}
			// set the lang value into route data
            //filterContext.RouteData.Values[Routing.Constants.Language] = CustomerSession.Language;
		}
	}
}
