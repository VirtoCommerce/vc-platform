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
                result.Add(SecurityErrorDescriber.LoginFailed());
            }

            //if (!signInResult.Succeeded)
            //{
            //    var detailedErrors = GetDetailedErrors(context);
            //    if (!detailedErrors)
            //    {
            //        result.Add(SecurityErrorDescriber.LoginFailed());
            //    }
            //    else if (signInResult.IsLockedOut)
            //    {
            //        var permanentLockOut = user.LockoutEnd == DateTime.MaxValue.ToUniversalTime();
            //        result.Add(permanentLockOut ? SecurityErrorDescriber.UserIsLockedOut() : SecurityErrorDescriber.UserIsTemporaryLockedOut());
            //    }
            //}
            //else
            //{
            //    if (user.PasswordExpired)
            //    {
            //        result.Add(SecurityErrorDescriber.PasswordExpired());
            //    }
            //}

            return Task.FromResult<IList<TokenLoginResponse>>(result);
        }
    }
}
