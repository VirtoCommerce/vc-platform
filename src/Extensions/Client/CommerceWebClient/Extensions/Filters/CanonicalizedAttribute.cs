using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Client.Extensions.Filters
{
    public class CanonicalizedAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            if (context.Request.Url != null)
            {
                var path = HttpUtility.UrlDecode(context.Request.Url.AbsolutePath);

                if (!string.IsNullOrEmpty(path))
                {
                    var query = context.Request.Url.Query;
                    var needRedirect = false;

                    // don't 'rewrite' POST requests
                    if (context.Request.RequestType == "GET" && !filterContext.IsChildAction)
                    {                   
                        // make sure request ends with a "/"
                        if (path.EndsWith("/"))
                        {
                            needRedirect = true;
                        }

                        //make language code allways be five symbols
                        if (filterContext.RouteData.Values.ContainsKey("lang"))
                        {
                            if (filterContext.RouteData.Values["lang"] as string != null)
                            {
                                if (filterContext.RouteData.Values["lang"].ToString().Length < 5)
                                {
                                    try
                                    {
                                        var cult = CultureInfo.CreateSpecificCulture(
                                            filterContext.RouteData.Values["lang"].ToString());
                                        path = path.Replace(filterContext.RouteData.Values["lang"].ToString(), cult.Name);
                                        needRedirect = true;
                                    }
                                    catch
                                    {
                                        //Something wrong with language??
                                    }
                                }
                            }
                        }
                    }

                    if (needRedirect)
                    {
                        Redirect(context, path, query);
                        return;
                    }
                }
            }

            base.OnActionExecuting(filterContext);

        }



        // correct as many 'rules' as possible per redirect to avoid
        // issuing too many redirects per request.
        private void Redirect(HttpContextBase context, string path, string query)
        {
            var newLocation = path;

            if (newLocation.EndsWith("/"))
                newLocation = newLocation.Substring(0, newLocation.Length - 1);

            newLocation = newLocation.ToLower(CultureInfo.InvariantCulture);

            context.Response.RedirectPermanent(newLocation + query, true);
        }

    }
}