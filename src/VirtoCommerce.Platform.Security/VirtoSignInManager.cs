using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security;

public class VirtoSignInManager : SignInManager<ApplicationUser>
{
    private readonly List<ITwoFactorPolicy> _twoFactorPolicies;
    private readonly Dictionary<string, bool> _twoFactorRequiredByUserId = new(StringComparer.OrdinalIgnoreCase);

    public VirtoSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation,
        IEnumerable<ITwoFactorPolicy> twoFactorPolicies)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _twoFactorPolicies = twoFactorPolicies.ToList();
    }

    public override async Task<bool> IsTwoFactorEnabledAsync(ApplicationUser user)
    {
        return await base.IsTwoFactorEnabledAsync(user) || await IsTwoFactorRequiredAsync(user);
    }

    protected virtual async Task<bool> IsTwoFactorRequiredAsync(ApplicationUser user)
    {
        if (_twoFactorPolicies.Count == 0)
        {
            return false;
        }

        if (!_twoFactorRequiredByUserId.TryGetValue(user.Id, out var isRequired))
        {
            isRequired = await EvaluateTwoFactorPoliciesAsync(user);
            _twoFactorRequiredByUserId[user.Id] = isRequired;
        }

        return isRequired;
    }

    protected virtual async Task<bool> EvaluateTwoFactorPoliciesAsync(ApplicationUser user)
    {
        foreach (var policy in _twoFactorPolicies)
        {
            if (await policy.IsTwoFactorRequiredAsync(user))
            {
                return true;
            }
        }

        return false;
    }
}
