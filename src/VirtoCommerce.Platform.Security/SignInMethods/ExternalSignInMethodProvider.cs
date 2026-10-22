using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInMethods;
using VirtoCommerce.Platform.Security.ExternalSignIn;

namespace VirtoCommerce.Platform.Security.SignInMethods;

public class ExternalSignInMethodProvider(
    SignInManager<ApplicationUser> signInManager,
    IEnumerable<ExternalSignInProviderConfiguration> externalSignInProviderConfigs)
    : ISignInMethodProvider
{
    public virtual async Task<IList<SignInMethod>> GetSignInMethodsAsync(ApplicationUser user = null)
    {
        var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        return schemes
            .Select(scheme =>
            {
                var signInMethod = AbstractTypeFactory<SignInMethod>.TryCreateInstance();
                var config = externalSignInProviderConfigs.FirstOrDefault(x => x.AuthenticationType.EqualsIgnoreCase(scheme.Name));

                signInMethod.Kind = SignInMethodKind.External;
                signInMethod.Name = scheme.Name;
                signInMethod.DisplayName = scheme.DisplayName;
                signInMethod.LogoUrl = config?.LogoUrl;
                signInMethod.Priority = config?.Provider?.Priority ?? 0;
                signInMethod.CanDeleteKey = true;

                signInMethod.Keys = user?.Logins?
                    .Where(x => x.LoginProvider == scheme.Name)
                    .Select(x =>
                    {
                        var key = AbstractTypeFactory<SignInKey>.TryCreateInstance();
                        key.Id = x.ProviderKey;
                        return key;
                    })
                    .ToList();

                return signInMethod;
            })
            .ToList();
    }

    public virtual Task<IdentityResult> RenameKeyAsync(ApplicationUser user, string signInMethodName, string keyId, string name)
    {
        return Task.FromResult(IdentityResult.Failed(CommonErrorDescriber.OperationNotSupported()));
    }

    public virtual Task<IdentityResult> DeleteKeyAsync(ApplicationUser user, string signInMethodName, string keyId)
    {
        return signInManager.UserManager.RemoveLoginAsync(user, signInMethodName, keyId);
    }
}
