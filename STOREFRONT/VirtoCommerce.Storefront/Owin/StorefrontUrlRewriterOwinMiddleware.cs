using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
            var requestPath = context.Request.Path.Value;
            var normalizedPath = new PathString();
            //add store to path
            normalizedPath = normalizedPath.Add(new PathString("/" + workContext.CurrentStore.Id));
            //add language to path
            normalizedPath = normalizedPath.Add(new PathString("/" + workContext.CurrentLanguage.CultureName));
            //add remaining path part without store and language
            requestPath = Regex.Replace(requestPath, "/" + workContext.CurrentStore.Id + "/", "/", RegexOptions.IgnoreCase);
            requestPath = Regex.Replace(requestPath, "/" + workContext.CurrentLanguage.CultureName + "/", "/", RegexOptions.IgnoreCase);
            normalizedPath = normalizedPath.Add(new PathString(requestPath));
        
            //http://stackoverflow.com/questions/28252230/url-rewrite-in-owin-middleware
            System.Web.Routing.RequestContext requestContext = context.Environment["System.Web.Routing.RequestContext"] as System.Web.Routing.RequestContext;
            requestContext.HttpContext.RewritePath("~" + normalizedPath.Value);
            context.Request.Path = normalizedPath;

            await Next.Invoke(context);
        }
    }

}