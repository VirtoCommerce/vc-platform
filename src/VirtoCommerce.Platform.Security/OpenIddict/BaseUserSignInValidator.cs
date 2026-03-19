using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    public class BaseUserSignInValidator : ITokenRequestValidator
    {
        public int Priority { get; set; }

        public Task<IList<TokenResponse>> ValidateAsync(TokenRequestContext context)
        {
            IList<TokenResponse> result = [];

            if (context.SignInResult != null && !context.SignInResult.Succeeded)
            {
                TokenResponse error;

                if (context.DetailedErrors && context.SignInResult.IsLockedOut)
                {
                    var permanentLockOut = context.User.LockoutEnd == DateTime.MaxValue.ToUniversalTime();
                    error = permanentLockOut
                        ? SecurityErrorDescriber.UserIsLockedOut()
                        : SecurityErrorDescriber.UserIsTemporaryLockedOut();
                }
                else
                {
                    error = SecurityErrorDescriber.LoginFailed();
                }

                result.Add(error);
            }

            return Task.FromResult(result);
        }
    }
}
