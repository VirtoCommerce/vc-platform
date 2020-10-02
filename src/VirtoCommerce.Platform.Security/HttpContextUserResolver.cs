using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security
{
    public class HttpContextUserResolver : IUserNameResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHangfireDataTransferService _hangfireDataTransferService;

        public HttpContextUserResolver(IHttpContextAccessor httpContextAccessor, IHangfireDataTransferService hangfireDataTransferService)
        {
            _httpContextAccessor = httpContextAccessor;
            _hangfireDataTransferService = hangfireDataTransferService;
        }

        public string GetCurrentUserName()
        {
            var result = _hangfireDataTransferService.UserName ?? "unknown";

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
