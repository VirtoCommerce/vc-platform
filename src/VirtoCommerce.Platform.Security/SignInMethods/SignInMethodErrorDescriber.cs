using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Security.SignInMethods;

public static class SignInMethodErrorDescriber
{
    public static IdentityError SignInKeyNotFound()
    {
        return new IdentityError
        {
            Code = nameof(SignInKeyNotFound).ToSnakeCase(),
            Description = "The sign-in key was not found.",
        };
    }

    public static IdentityError LastSignInKey()
    {
        return new IdentityError
        {
            Code = nameof(LastSignInKey).ToSnakeCase(),
            Description = "The last sign-in key cannot be removed.",
        };
    }
}
