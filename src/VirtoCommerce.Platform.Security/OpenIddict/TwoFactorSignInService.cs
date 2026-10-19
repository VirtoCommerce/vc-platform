using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public class TwoFactorSignInService : ITwoFactorSignInService
{
    private readonly IEnumerable<ITwoFactorSignInHandler> _twoFactorSignInHandlers;

    public TwoFactorSignInService(IEnumerable<ITwoFactorSignInHandler> twoFactorSignInHandlers)
    {
        _twoFactorSignInHandlers = twoFactorSignInHandlers;
    }

    public virtual bool IsAvailable => _twoFactorSignInHandlers.Any();

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
