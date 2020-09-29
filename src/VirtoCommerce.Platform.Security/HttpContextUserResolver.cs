using System.Threading;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security
{
    public class HttpContextUserResolver : IUserNameResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public const string USER_NAME_THREAD_SLOT_NAME = "UserName";

        public HttpContextUserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            var result = Thread.GetData(Thread.GetNamedDataSlot(USER_NAME_THREAD_SLOT_NAME)) as string ?? "unknown";

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
    }
}
