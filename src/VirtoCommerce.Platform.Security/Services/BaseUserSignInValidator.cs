using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public class BaseUserSignInValidator : IUserSignInValidator
    {
        public int Priority { get; set; }

        public Task<IList<TokenLoginResponse>> ValidateUserAsync(SignInValidatorContext context)
        {
            var result = new List<TokenLoginResponse>();

            if (!context.IsSucceeded)
            {
                if (!context.DetailedErrors)
                {
                    result.Add(SecurityErrorDescriber.LoginFailed());
                }
                else if (context.IsLockedOut)
                {
                    var permanentLockOut = context.User.LockoutEnd == DateTime.MaxValue.ToUniversalTime();
                    result.Add(permanentLockOut ? SecurityErrorDescriber.UserIsLockedOut() : SecurityErrorDescriber.UserIsTemporaryLockedOut());
                }
            }

            return Task.FromResult<IList<TokenLoginResponse>>(result);
        }
    }
}
