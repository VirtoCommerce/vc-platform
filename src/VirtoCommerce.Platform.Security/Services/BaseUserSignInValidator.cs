using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public class BaseUserSignInValidator : IUserSignInValidator
    {
        public int Priority { get; set; }

        public Task<IList<TokenLoginResponse>> ValidateUserAsync(ApplicationUser user, IDictionary<string, object> context)
        {
            var result = new List<TokenLoginResponse>();

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

        private static bool GetDetailedErrors(IDictionary<string, object> context)
        {
            var detailedErrors = false;
            if (context.TryGetValue("detailedErrors", out var detailedErrorsValue))
            {
                detailedErrors = (bool)detailedErrorsValue;
            }
            return detailedErrors;
        }
    }
}
