using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInMethods;

namespace VirtoCommerce.Platform.Security.SignInMethods;

public class PasswordSignInMethodProvider(
    IOptions<PasswordLoginOptions> passwordLoginOptions,
    UserManager<ApplicationUser> userManager)
    : ISignInMethodProvider
{
    private readonly PasswordLoginOptions _passwordLoginOptions = passwordLoginOptions.Value;

    public virtual async Task<IList<SignInMethod>> GetSignInMethodsAsync(ApplicationUser user = null)
    {
        if (!_passwordLoginOptions.Enabled)
        {
            return [];
        }

        var signInMethod = AbstractTypeFactory<SignInMethod>.TryCreateInstance();
        signInMethod.Kind = SignInMethodKind.Password;
        signInMethod.Name = _passwordLoginOptions.AuthenticationType;
        signInMethod.DisplayName = _passwordLoginOptions.AuthenticationType;
        signInMethod.Priority = _passwordLoginOptions.Priority;
        signInMethod.CanDeleteKey = true;

        if (user != null)
        {
            signInMethod.Keys = [];

            if (await userManager.HasPasswordAsync(user))
            {
                var key = AbstractTypeFactory<SignInKey>.TryCreateInstance();
                key.Id = "password";
                key.ModifiedDate = user.LastPasswordChangedDate;

                signInMethod.Keys.Add(key);
            }

            signInMethod.CanCreateKey = signInMethod.Keys.Count == 0;
        }

        return [signInMethod];
    }

    public virtual Task<IdentityResult> RenameKeyAsync(ApplicationUser user, string signInMethodName, string keyId, string name)
    {
        return Task.FromResult(IdentityResult.Failed(CommonErrorDescriber.OperationNotSupported()));
    }

    public virtual Task<IdentityResult> DeleteKeyAsync(ApplicationUser user, string signInMethodName, string keyId)
    {
        return userManager.RemovePasswordAsync(user);
    }
}
