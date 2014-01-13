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
                        // check for any upper-case letters:
                        if (path != path.ToLower(CultureInfo.InvariantCulture))
                        {
                            needRedirect = true;
                        }

                        // make sure request ends with a "/"
                        if (path.EndsWith("/"))
                        {
                            needRedirect = true;
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