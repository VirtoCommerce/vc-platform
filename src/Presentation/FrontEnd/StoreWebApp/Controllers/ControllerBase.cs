using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Client.Extensions.Filters;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class ControllerBase.
	/// </summary>
	[Localize]
    [Canonicalized]
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
	}
}