using System;
using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class UserNameResolver : IUserNameResolver
    {
        private readonly Func<IOwinRequest> _owinRequestFactory;

        public UserNameResolver(Func<IOwinRequest> owinRequestFactory)
        {
            _owinRequestFactory = owinRequestFactory;
        }

        public string GetCurrentUserName()
        {
            string userName = null;

            var owinRequest = _owinRequestFactory != null ? _owinRequestFactory() : null;
            var identity = owinRequest != null ? owinRequest.User.Identity : null;

            if (identity != null && identity.IsAuthenticated)
            {
                userName = owinRequest.Headers.Get("VirtoCommerce-User-Name");
                if (string.IsNullOrEmpty(userName))
                {
                    userName = identity.Name;
                }
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = "unknown";
            }

            return userName;
        }
    }
}
