using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Hmac
{
    public class HmacAuthenticationMiddleware : AuthenticationMiddleware<HmacAuthenticationOptions>
    {
        public HmacAuthenticationMiddleware(OwinMiddleware next, HmacAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<HmacAuthenticationOptions> CreateHandler()
        {
            return new HmacAuthenticationHandler();
        }
    }
}
