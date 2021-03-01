using System.Threading;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core.Security;
using static VirtoCommerce.Platform.Data.Constants.DefaultEntityNames;

namespace VirtoCommerce.Platform.Security
{
    public class HttpContextUserResolver : IUserNameResolver
    {
        private static readonly AsyncLocal<string> _currentUserName = new AsyncLocal<string>();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            var result = _currentUserName.Value ?? UNKNOWN_USERNAME;

            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request != null && context.User != null)
            {
                var identity = context.User.Identity;
                if (identity != null && identity.IsAuthenticated)
                {
                    result = context.Request.Headers["VirtoCommerce-User-Name"];
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
