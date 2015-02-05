using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Caching;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Client.Extensions.Routing.Routes;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Controllers
{
    using VirtoCommerce.Web.Client.Providers;

    /// <summary>
	/// Class ControllerBase.
	/// </summary>
    [Localize(Order = 1)]
    [Canonicalized(typeof(AccountController), typeof(CheckoutController), Order = 2)]
	public abstract class ControllerBase : Controller
	{

	    private OutputCacheManager _cacheManager;
		/// <summary>
		/// Renders the razor view to string.
		/// </summary>
		/// <param name="viewName">Name of the view.</param>
		/// <param name="model">The model.</param>
		/// <returns>System.String.</returns>
		protected string RenderRazorViewToString(string viewName, object model)
		{
		    return ViewRenderer.RenderPartialView(viewName, model, ControllerContext);
		}

	    protected override void OnActionExecuted(ActionExecutedContext filterContext)
	    {
	        if (!filterContext.Canceled && filterContext.Result != null && !ControllerContext.IsChildAction)
	        {
	            FillViewBagWithMetadata(filterContext);
	        }

            //Process messages
            var messages = new MessagesModel();
	        var hasMessages = Enum.GetNames(typeof (MessageType)).Aggregate(false, (current, typeName) =>
                current | ProcessMessages((MessageType)Enum.Parse(typeof(MessageType), typeName), messages));
	        if (hasMessages)
	        {
	            this.SharedViewBag().Messages = messages;
	        }
	    }

	    protected override void OnResultExecuting(ResultExecutingContext filterContext)
	    {
            //This is needed for IE as it agresively caches
	        DontCacheAjax(filterContext);
	        base.OnResultExecuting(filterContext);
	    }

        private void DontCacheAjax(ResultExecutingContext filterContext)
	    {
            var context = filterContext.HttpContext;

            //We want to expliclilty not cache ajax get not child requests
            if (context.Request.RequestType != "GET" || filterContext.IsChildAction || !context.Request.IsAjaxRequest())
            {
                return;
            }

            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
	    }

	    private bool ProcessMessages(MessageType type, MessagesModel messages)
	    {
            var messagesTmp = TempData[GetMessageTempKey(type)] as IEnumerable;
	        var foundAny = false;

            if (messagesTmp != null)
            {
                foreach (var messageTmp in messagesTmp)
                {
                    messages.Add(new MessageModel(messageTmp.ToString(), type));
                    foundAny = true;
                }
            }

	        return foundAny;
	    }

	    public string GetMessageTempKey(MessageType type)
	    {
	        return string.Format("{0}_messages", type);
	    }

	    protected virtual bool FillViewBagWithMetadata(ActionExecutedContext filterContext)
	    {
            var storeRoute = filterContext.RouteData.Route as StoreRoute;

	        if (storeRoute != null)
	        {
	            string routeKey = storeRoute.GetMainRouteKey();

	            if (filterContext.RouteData.Values.ContainsKey(routeKey))
	            {
	                var routeValue = filterContext.RouteData.Values[routeKey] as string;
	                if (!String.IsNullOrEmpty(routeValue))
	                {
	                    SeoUrlKeywordTypes type;
	                    if (Enum.TryParse(routeKey, true, out type))
	                    {
	                        var lastIsVal = routeValue.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Last();
	                        var keyword = SettingsHelper.SeoKeyword(lastIsVal, type, byValue: false);

	                        if (keyword != null)
	                        {
	                            ViewBag.MetaDescription = keyword.MetaDescription;
	                            ViewBag.Title = keyword.Title;
	                            ViewBag.MetaKeywords = keyword.MetaKeywords;
	                            ViewBag.ImageAltDescription = keyword.ImageAltDescription;

	                            return true;
	                        }
	                    }
	                }
	            }
	        }

	        return false;
        }

        #region Cache

        public OutputCacheManager OutputCacheManager
        {
            get {
                return _cacheManager ??
                       (_cacheManager = new OutputCacheManager(OutputCache.Instance, new KeyBuilder()));
            }
        }

        #endregion

        protected override ITempDataProvider CreateTempDataProvider()
        {
            return new CookieTempDataProvider();
        }
    }
}