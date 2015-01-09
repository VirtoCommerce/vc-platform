using System.Threading;
using System.Globalization;
using System.Web.Http.Filters;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ApiWebClient.Customer;
using VirtoCommerce.ApiWebClient.Customer.Services;

namespace VirtoCommerce.ApiWebClient.Extensions.Filters
{
	/// <summary>
	/// Class LocalizeWebApiAttribute.
	/// </summary>
	public class LocalizeWebApiAttribute : ActionFilterAttribute
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
		/// Occurs before the action method is invoked.
		/// </summary>
		/// <param name="actionContext">The action context.</param>
		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
		{

			try
			{
				if (CustomerSession.Language != null && Thread.CurrentThread.CurrentUICulture.Name != CustomerSession.Language)
				{
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CustomerSession.Language);
				}
			}
			catch
			{
				
			}

			base.OnActionExecuting(actionContext);
		}
    }
}