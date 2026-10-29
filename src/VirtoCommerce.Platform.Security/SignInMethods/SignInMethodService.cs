using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInMethods;

namespace VirtoCommerce.Platform.Security.SignInMethods;

public class SignInMethodService(IEnumerable<ISignInMethodProvider> providers) : ISignInMethodService
{
    public virtual async Task<IList<SignInMethod>> GetSignInMethodsAsync(ApplicationUser user = null)
    {
        var signInMethods = await GetProviderSignInMethodsAsync(user);

        return signInMethods
            .Select(x => x.Method)
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.DisplayName)
            .ToList();
    }

    public virtual async Task<IdentityResult> RenameKeyAsync(ApplicationUser user, string signInMethodName, string keyId, string name)
    {
        var signInMethods = await GetProviderSignInMethodsAsync(user);
        var (key, method, provider) = FindKey(signInMethods, signInMethodName, keyId);

        if (key == null)
        {
            return KeyNotFound();
        }

        return await provider.RenameKeyAsync(user, method.Name, keyId, name);
    }

    public virtual async Task<IdentityResult> DeleteKeyAsync(ApplicationUser user, string signInMethodName, string keyId)
    {
        var signInMethods = await GetProviderSignInMethodsAsync(user);
        var (key, method, provider) = FindKey(signInMethods, signInMethodName, keyId);

        if (key == null)
        {
            return KeyNotFound();
        }

        if (signInMethods.Sum(x => x.Method.Keys?.Count ?? 0) == 1)
        {
            return IdentityResult.Failed(SignInMethodErrorDescriber.LastSignInKey());
        }

        return await provider.DeleteKeyAsync(user, method.Name, keyId);
    }

    protected virtual async Task<IList<ProviderSignInMethod>> GetProviderSignInMethodsAsync(ApplicationUser user)
    {
        var result = new List<ProviderSignInMethod>();

        // Providers are called one by one because some of them query the same scoped DbContext, which does not support parallel queries
        foreach (var provider in providers)
        {
            var signInMethods = await provider.GetSignInMethodsAsync(user);
            result.AddRange(signInMethods.Select(method => new ProviderSignInMethod(provider, method)));
        }

        return result;
    }

    protected virtual (SignInKey Key, SignInMethod Method, ISignInMethodProvider Provider) FindKey(IList<ProviderSignInMethod> signInMethods, string signInMethodName, string keyId)
    {
        foreach (var signInMethod in signInMethods.Where(x => x.Method.Name.EqualsIgnoreCase(signInMethodName)))
        {
            var key = signInMethod.Method.Keys?.FirstOrDefault(x => x.Id == keyId);
            if (key != null)
            {
                return (key, signInMethod.Method, signInMethod.Provider);
            }
        }

        return (null, null, null);
    }

    protected virtual IdentityResult KeyNotFound()
    {
        return IdentityResult.Failed(SignInMethodErrorDescriber.SignInKeyNotFound());
    }

    protected sealed record ProviderSignInMethod(ISignInMethodProvider Provider, SignInMethod Method);
}
