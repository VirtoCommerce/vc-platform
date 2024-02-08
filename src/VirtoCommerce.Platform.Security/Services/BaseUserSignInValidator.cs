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
                var error = SecurityErrorDescriber.LoginFailed();

                if (context.DetailedErrors && context.IsLockedOut)
                {
                    var permanentLockOut = context.User.LockoutEnd == DateTime.MaxValue.ToUniversalTime();
                    error = permanentLockOut ? SecurityErrorDescriber.UserIsLockedOut() : SecurityErrorDescriber.UserIsTemporaryLockedOut();
                }

                result.Add(error);
            }

            return Task.FromResult<IList<TokenLoginResponse>>(result);
        }
    }
}
