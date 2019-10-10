using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Authorization
{
    /// <summary>
    /// https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/
    /// </summary>
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IPermissionsRegistrar _permissionsProvider;
        private readonly IPlatformMemoryCache _memoryCache;
        public PermissionAuthorizationPolicyProvider(IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions> options, IConfiguration configuration, IPermissionsRegistrar permissionsProvider, IPlatformMemoryCache memoryCache)
            : base(options)
        {
            _configuration = configuration;
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
                    resultLookup[permission.Name] = new AuthorizationPolicyBuilder().AddRequirements(new PermissionAuthorizationRequirement(permission.Name)).Build();
                }
                return resultLookup;
            });
            return result;
        }
    }
}
