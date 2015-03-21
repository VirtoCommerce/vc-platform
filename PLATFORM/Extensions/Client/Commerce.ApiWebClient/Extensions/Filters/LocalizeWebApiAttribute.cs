using System.Threading;
using System.Globalization;
using System.Web.Http.Filters;
using Microsoft.Practices.ServiceLocation;

namespace VirtoCommerce.ApiWebClient.Extensions.Filters
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Session;

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
            get { return ClientContext.Session; }
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