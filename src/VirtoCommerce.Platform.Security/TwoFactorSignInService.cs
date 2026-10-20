using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security;

public class TwoFactorSignInService : ITwoFactorSignInService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly List<ITwoFactorPolicy> _twoFactorPolicies;
    private readonly List<ITwoFactorSignInHandler> _twoFactorSignInHandlers;
    private readonly Dictionary<string, bool> _twoFactorRequiredByUserId = new(StringComparer.OrdinalIgnoreCase);

    public TwoFactorSignInService(
        UserManager<ApplicationUser> userManager,
        IEnumerable<ITwoFactorPolicy> twoFactorPolicies,
        IEnumerable<ITwoFactorSignInHandler> twoFactorSignInHandlers)
    {
        _userManager = userManager;
        _twoFactorPolicies = twoFactorPolicies.ToList();
        _twoFactorSignInHandlers = twoFactorSignInHandlers.ToList();
    }

    public virtual bool IsTwoFactorAvailable => _twoFactorSignInHandlers.Count > 0;

    public virtual async Task<bool> IsTwoFactorRequiredAsync(ApplicationUser user)
    {
        return await IsTwoFactorRequiredByUserAsync(user) || await IsTwoFactorRequiredByPoliciesAsync(user);
    }

    protected virtual async Task<bool> IsTwoFactorRequiredByUserAsync(ApplicationUser user)
    {
        return _userManager.SupportsUserTwoFactor &&
            await _userManager.GetTwoFactorEnabledAsync(user) &&
            (await _userManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
    }

    protected virtual async Task<bool> IsTwoFactorRequiredByPoliciesAsync(ApplicationUser user)
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

    public virtual async Task<ActionResult> ChallengeAsync(TokenRequestContext context)
    {
        var twoFactorSignInHandler = _twoFactorSignInHandlers.LastOrDefault();
        if (twoFactorSignInHandler == null)
        {
            context.FailureReason = SignInFailureReason.TwoFactorUnavailable;

            return new BadRequestObjectResult(SecurityErrorDescriber.TwoFactorUnavailable());
        }

        var challengeResult = await twoFactorSignInHandler.ChallengeAsync(context);
        context.FailureReason ??= SignInFailureReason.RequiresTwoFactor;

        return challengeResult;
    }
}
