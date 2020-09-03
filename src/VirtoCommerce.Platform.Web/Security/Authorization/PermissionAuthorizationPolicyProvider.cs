using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Authorization;
using VirtoCommerce.Platform.Web.Security.Authentication;

namespace VirtoCommerce.Platform.Web.Security.Authorization
{
    /// <summary>
    /// https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/
    /// </summary>
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IPermissionsRegistrar _permissionsProvider;
        private readonly IPlatformMemoryCache _memoryCache;
        public PermissionAuthorizationPolicyProvider(IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions> options, IConfiguration configuration, IPermissionsRegistrar permissionsProvider, IPlatformMemoryCache memoryCache)
            : base(options)
        {
            _permissionsProvider = permissionsProvider;
            _memoryCache = memoryCache;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Check static policies first
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                var map = GetDynamicAuthorizationPoliciesFromModulesPermissions();
                map.TryGetValue(policyName, out policy);
            }

            return policy;
        }

        private Dictionary<string, AuthorizationPolicy> GetDynamicAuthorizationPoliciesFromModulesPermissions()
        {
            var cacheKey = CacheKey.With(GetType(), "GetDynamicAuthorizationPoliciesFromModulesPermissions");
            var result = _memoryCache.GetOrCreateExclusive(cacheKey, (cacheEntry) =>
            {
                var resultLookup = new Dictionary<string, AuthorizationPolicy>();
                foreach (var permission in _permissionsProvider.GetAllPermissions())
                {
                    resultLookup[permission.Name] = new AuthorizationPolicyBuilder().AddRequirements(new PermissionAuthorizationRequirement(permission.Name))
                    //Use the two schema (JwtBearer and ApiKey)  authentication for permission authorization policies.
                                                                                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationOptions.DefaultScheme)
                                                                                    .Build();
                }
                return resultLookup;
            });
            return result;
        }
    }
}
