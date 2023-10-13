using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Security.Extensions;

namespace VirtoCommerce.Platform.Security
{
    public class ConfigureSecurityStampValidatorOptions : IConfigureOptions<SecurityStampValidatorOptions>
    {
        public void Configure(SecurityStampValidatorOptions options)
        {
            options.ValidationInterval = TimeSpan.FromMinutes(30);

            // When refreshing the principal, ensure custom claims that
            // might have been set with an external identity continue
            // to flow through to this new one.
            options.OnRefreshingPrincipal = refreshingPrincipal =>
            {
                var newIdentity = refreshingPrincipal.NewPrincipal?.Identities.First();
                var currentIdentity = refreshingPrincipal.CurrentPrincipal?.Identities.First();

                if (currentIdentity is not null)
                {
                    // Since this is refreshing an existing principal, we want to merge all claims.
                    newIdentity?.MergeAllClaims(currentIdentity);
                }

                return Task.CompletedTask;
            };
        }
    }
}
