using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using static VirtoCommerce.Platform.Data.Constants.DefaultEntityNames;

namespace VirtoCommerce.Platform.Security
{
    public class HttpContextUserResolver(IHttpContextAccessor httpContextAccessor) : IUserNameResolver
    {
        private const string _anonymousUserName = "http:anonymous";

        private static readonly AsyncLocal<string> _currentUserName = new();

        public string GetCurrentUserName()
        {
            var result = _currentUserName.Value ?? UNKNOWN_USERNAME;

            var context = httpContextAccessor.HttpContext;
            if (context != null)
            {
                result = _anonymousUserName;

                var identity = context.User.Identity;
                if (identity != null && identity.IsAuthenticated)
                {
                    // Login-on-behalf operator from token
                    result = context.User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserName);

                    if (string.IsNullOrEmpty(result))
                    {
                        result = identity.Name;
                    }
                }
            }

            return result;
        }

        public void SetCurrentUserName(string userName)
        {
            _currentUserName.Value = userName;
        }
    }
}
