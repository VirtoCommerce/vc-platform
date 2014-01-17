using System;
using System.IO;
using System.Web.Mvc;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class ControllerBase.
	/// </summary>
    [Localize(Order = 1)]
    [Canonicalized(Order = 2)]
	public abstract class ControllerBase : Controller
	{
		/// <summary>
		/// Renders the razor view to string.
		/// </summary>
		/// <param name="viewName">Name of the view.</param>
		/// <param name="model">The model.</param>
		/// <returns>System.String.</returns>
		protected string RenderRazorViewToString(string viewName, object model)
		{
			ViewData.Model = model;
			using (var sw = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);
				viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
				return sw.GetStringBuilder().ToString().Replace(Environment.NewLine, string.Empty);
			}
		}

	    protected override void OnActionExecuted(ActionExecutedContext filterContext)
	    {
	        if (!filterContext.Canceled && filterContext.Result != null && !ControllerContext.IsChildAction)
	        {
                FillViewBagWithMetadata(filterContext, Constants.Store);
                FillViewBagWithMetadata(filterContext, Constants.Category);
                FillViewBagWithMetadata(filterContext, Constants.Item);
	        }
	    }

	    protected virtual void FillViewBagWithMetadata(ActionExecutedContext filterContext, string routeKey)
	    {
            if (filterContext.RouteData.Values.ContainsKey(routeKey))
            {
                var routeValue = filterContext.RouteData.Values[routeKey] as string;
                if (!String.IsNullOrEmpty(routeValue))
                {
                    SeoUrlKeywordTypes type;
                    if (Enum.TryParse(routeKey, true, out type))
                    {
                        var keyword = SettingsHelper.SeoKeyword(routeValue, type);

                        if (keyword != null)
                        {
                            ViewBag.MetaDescription = keyword.MetaDescription;
                            ViewBag.Title = keyword.Title;
                            ViewBag.MetaKeywords = keyword.MetaKeywords;
                        }
                    }
                }
            }
	    }

	}
}