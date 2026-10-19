using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    [Obsolete("The password grant and GrantTypeHandlerBase do not call token request validators after a failed sign-in.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class BaseUserSignInValidator : ITokenRequestValidator
    {
        public int Priority { get; set; }

        public Task<IList<TokenResponse>> ValidateAsync(TokenRequestContext context)
        {
            IList<TokenResponse> result = [];

            if (context.SignInResult != null && !context.SignInResult.Succeeded)
            {
                context.FailureReason = GetFailureReason(context.SignInResult);

                TokenResponse error;

                if (context.DetailedErrors && context.SignInResult.IsLockedOut)
                {
                    error = SecurityErrorDescriber.GetLockoutError(context.User);
                }
                else
                {
                    error = SecurityErrorDescriber.LoginFailed();
                }

                result.Add(error);
            }

            return Task.FromResult(result);
        }

        private static string GetFailureReason(SignInResult signInResult)
        {
            if (signInResult.IsLockedOut)
            {
                return SignInFailureReason.LockedOut;
            }

            if (signInResult.IsNotAllowed)
            {
                return SignInFailureReason.NotAllowed;
            }

            if (signInResult.RequiresTwoFactor)
            {
                return SignInFailureReason.RequiresTwoFactor;
            }

            return SignInFailureReason.InvalidPassword;
        }
    }
}
