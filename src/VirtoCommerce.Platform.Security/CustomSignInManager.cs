using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security;

public class CustomSignInManager : SignInManager<ApplicationUser>
{
    private static readonly HashSet<string> SecretParameters = new(StringComparer.OrdinalIgnoreCase)
    {
        OpenIddictConstants.Parameters.Password,
        OpenIddictConstants.Parameters.ClientSecret,
        OpenIddictConstants.Parameters.RefreshToken,
        OpenIddictConstants.Parameters.Code,
        OpenIddictConstants.Parameters.ClientAssertion,
        OpenIddictConstants.Parameters.Assertion,
    };

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly List<ITwoFactorPolicy> _twoFactorPolicies;
    private readonly Dictionary<string, bool> _twoFactorRequiredByUserId = [];

    public CustomSignInManager(
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
        _httpContextAccessor = contextAccessor;
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
            isRequired = await EvaluateTwoFactorPoliciesAsync(CreateTwoFactorPolicyContext(user));
            _twoFactorRequiredByUserId[user.Id] = isRequired;
        }

        return isRequired;
    }

    protected virtual async Task<bool> EvaluateTwoFactorPoliciesAsync(TwoFactorPolicyContext context)
    {
        foreach (var policy in _twoFactorPolicies)
        {
            if (await policy.IsTwoFactorRequiredAsync(context))
            {
                return true;
            }
        }

        return false;
    }

    protected virtual TwoFactorPolicyContext CreateTwoFactorPolicyContext(ApplicationUser user)
    {
        var context = AbstractTypeFactory<TwoFactorPolicyContext>.TryCreateInstance();
        context.User = user;

        var request = _httpContextAccessor.HttpContext?.GetOpenIddictServerRequest();
        if (request != null)
        {
            context.GrantType = request.GrantType;

            foreach (var (name, value) in request.GetParameters().Where(x => !SecretParameters.Contains(x.Key)))
            {
                context.Parameters[name] = (string)value;
            }
        }

        return context;
    }
}
