using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data
{
    public class CurrentUserOwinMiddleware : OwinMiddleware
    {
        private readonly Func<ICurrentUser> _currentUserFactory;

        public CurrentUserOwinMiddleware(OwinMiddleware next, Func<ICurrentUser> currentUserFactory)
            : base(next)
        {
            _currentUserFactory = currentUserFactory;
        }

        public override async Task Invoke(IOwinContext context)
        {
            string userName = null;

            if (context != null && context.Request != null && context.Request.User != null)
            {
                var identity = context.Request.User.Identity;
                if (identity != null && identity.IsAuthenticated)
                {
                    userName = context.Request.Headers.Get("VirtoCommerce-User-Name");
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = identity.Name;
                    }
                }
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = "unknown";
            }

            var currentUser = _currentUserFactory != null ? _currentUserFactory() : null;
            if (currentUser != null)
            {
                currentUser.UserName = userName;
            }

            await Next.Invoke(context);
        }
    }
}
