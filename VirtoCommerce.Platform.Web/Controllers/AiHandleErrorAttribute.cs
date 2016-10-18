using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.Platform.Web.Controllers
{
	/// <summary>
	/// If the custom errors configuration is Off, then exceptions will be available for the HTTPModule to collect. 
	/// However, if it is RemoteOnly (default), or On – then the exception will be cleared and not available for us to automatically collect. 
	/// You can fix that by adding an attribute derived from HandleErrorAttribute and then registering it as global filter as shown below.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AiHandleErrorAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
			{
				//If customError is Off, then AI HTTPModule will report the exception
				//if (filterContext.HttpContext.IsCustomErrorEnabled)
				//{
					// Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
					var ai = new TelemetryClient();

					var properties = new Dictionary<string, string>();
					if(filterContext.HttpContext!=null)
						FillFormProperties(filterContext.HttpContext.Request, properties);

					ai.TrackException(filterContext.Exception, properties);
				//}
			}
			base.OnException(filterContext);
		}

		private void FillFormProperties(HttpRequestBase request, Dictionary<string, string> dictionary)
		{
			if (request.Form == null) return;

			var items = request.Form.AllKeys.SelectMany(request.Form.GetValues, (k, v) => new { key = k, value = v });
			foreach (var item in items)
			{
				dictionary.Add(item.key, item.value);
			}
		}

	}
}