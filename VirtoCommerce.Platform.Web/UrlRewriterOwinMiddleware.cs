using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace VirtoCommerce.Platform.Web
{
    public class UrlRewriterOwinMiddleware : OwinMiddleware
    {
        private readonly UrlRewriterOptions _options;

        public UrlRewriterOwinMiddleware(OwinMiddleware next, UrlRewriterOptions options)
            : base(next)
        {
            _options = options;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var requestPath = context.Request.Path;

            foreach (var pair in _options.Items)
            {
                PathString remainingPath;
                if (requestPath.StartsWithSegments(pair.Key, out remainingPath))
                {
                    context.Request.Path = new PathString(pair.Value + remainingPath.Value);
                    break;
                }
            }

            await Next.Invoke(context);
        }
    }

    public class UrlRewriterOptions
    {
        public IDictionary<PathString, string> Items { get; private set; }

        public UrlRewriterOptions()
        {
            Items = new Dictionary<PathString, string>();
        }
    }
}
