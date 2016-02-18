using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class UserNameOwinMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Instantiates the middleware with an optional pointer to the next component.
        /// </summary>
        /// <param name="next"/>
        public UserNameOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context"/>
        /// <returns/>
        public override async Task Invoke(IOwinContext context)
        {
            var identity = context.Request.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userName = context.Request.Headers.Get("VirtoCommerce-User-Name");

                if (!string.IsNullOrEmpty(userName))
                {
                    identity.AddClaim(new Claim(VirtoCommerceClaimTypes.UserName, userName));
                }
            }

            await Next.Invoke(context);
        }
    }
}
