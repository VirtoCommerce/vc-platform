using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Owin
{
    /// <summary>
    /// Normalize all storefront requests to special storefront schema {storeId}/{language}/{path} 
    /// http://docs.virtocommerce.com/display/vc2devguide/Storefront+SEO+routing
    /// </summary>
    public class StorefrontUrlRewriterOwinMiddleware : OwinMiddleware
    {
        private readonly UnityContainer _container;

        public StorefrontUrlRewriterOwinMiddleware(OwinMiddleware next, UnityContainer container)
            : base(next)
        {
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var workContext = _container.Resolve<WorkContext>();

            if (workContext.RequestUrl != null)
            {
                var currentStoreId = workContext.CurrentStore != null ? workContext.CurrentStore.Id : "-";
                var currentCultureName = workContext.CurrentStore != null ? workContext.CurrentLanguage.CultureName : "en-US";

                var normalizedPath = new PathString();
                //add store to path
                normalizedPath = normalizedPath.Add(new PathString("/" + currentStoreId));
                //add language to path
                normalizedPath = normalizedPath.Add(new PathString("/" + currentCultureName));

                //add remaining path part without store and language
                var requestPath = context.Request.Path.Value;
                requestPath = Regex.Replace(requestPath, "/" + currentStoreId + "/?", "/", RegexOptions.IgnoreCase);
                requestPath = Regex.Replace(requestPath, "/" + currentCultureName + "/?", "/", RegexOptions.IgnoreCase);
                normalizedPath = normalizedPath.Add(new PathString(requestPath));

                context.Request.Path = normalizedPath;

                //http://stackoverflow.com/questions/28252230/url-rewrite-in-owin-middleware
                var httpContext = context.Environment["System.Web.HttpContextBase"] as System.Web.HttpContextWrapper;
                if (httpContext != null)
                {
                    httpContext.RewritePath("~" + normalizedPath.Value);
                }
            }

            await Next.Invoke(context);
        }
    }
}
