using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public class GrantValidationResult
{
    public bool Success => Error == null;

    public ApplicationUser User { get; set; }

    public TokenResponse Error { get; set; }

    public static GrantValidationResult Succeed(ApplicationUser user)
    {
        return new GrantValidationResult
        {
            User = user,
        };
    }

    public static GrantValidationResult Fail(TokenResponse error)
    {
        return new GrantValidationResult
        {
            Error = error,
        };
    }
}
