using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowContentCorsAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public AllowContentCorsAttribute()
        {
            _policy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                SupportsCredentials = true
            };
            ConfigurationHelper.SplitAppSettingsStringValue("cors:allowOrigins").ToList().ForEach(_policy.Origins.Add);
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}
