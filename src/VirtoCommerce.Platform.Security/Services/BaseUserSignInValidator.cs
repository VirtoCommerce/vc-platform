using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public class BaseUserSignInValidator : IUserSignInValidator
    {
        public int Priority { get; set; }

        public Task<IList<TokenLoginResponse>> ValidateUserAsync(ApplicationUser user, SignInResult signInResult, IDictionary<string, object> context)
        {
            var detailedErrors = false;
            if (context.TryGetValue("detailedErrors", out var detailedErrorsValue))
            {
                detailedErrors = (bool)detailedErrorsValue;
            }

            var result = new List<TokenLoginResponse>();

            if (!signInResult.Succeeded)
            {
                if (detailedErrors)
                {
                    if (signInResult.IsLockedOut)
                    {
                        if (user.LockoutEnd != DateTime.MaxValue.ToUniversalTime())
                        {
                            result.Add(SecurityErrorDescriber.UserIsTemporaryLockedOut());
                        }
                        else
                        {
                            result.Add(SecurityErrorDescriber.UserIsLockedOut());
                        }
                    }
                }
                else
                {
                    result.Add(SecurityErrorDescriber.LoginFailed());
                }
            }
            else
            {
                if (user.PasswordExpired)
                {
                    result.Add(SecurityErrorDescriber.PasswordExpired());
                }
            }

            return Task.FromResult<IList<TokenLoginResponse>>(result);
        }
    }
}
