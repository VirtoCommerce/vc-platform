using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class UserNameResolver : IUserNameResolver
    {
        private readonly IOwinRequest _owinRequest;

        public UserNameResolver(IOwinRequest owinRequest)
        {
            _owinRequest = owinRequest;
        }

        public string GetCurrentUserName()
        {
            string userName = null;

            var identity = _owinRequest.User.Identity;
            if (identity != null && identity.IsAuthenticated)
            {
                userName = _owinRequest.Headers.Get("VirtoCommerce-User-Name");
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
