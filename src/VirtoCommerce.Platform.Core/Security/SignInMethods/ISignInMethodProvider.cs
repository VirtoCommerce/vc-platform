using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Core.Security.SignInMethods;

public interface ISignInMethodProvider
{
    Task<IList<SignInMethod>> GetSignInMethodsAsync(ApplicationUser user = null);

    Task<IdentityResult> RenameKeyAsync(ApplicationUser user, string signInMethodName, string keyId, string name);

    Task<IdentityResult> DeleteKeyAsync(ApplicationUser user, string signInMethodName, string keyId);
}
