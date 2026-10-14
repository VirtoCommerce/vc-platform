using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public class GrantValidationResult
{
    public bool Success => Error == null && ErrorResult == null;

    public ApplicationUser User { get; set; }

    public TokenResponse Error { get; set; }

    public ActionResult ErrorResult { get; set; }

    public static GrantValidationResult Succeed(ApplicationUser user)
    {
        return new GrantValidationResult
        {
            User = user,
        };
    }

    public static GrantValidationResult Fail(TokenResponse error, ApplicationUser user = null)
    {
        return new GrantValidationResult
        {
            Error = error,
            User = user,
        };
    }

    public static GrantValidationResult Fail(ActionResult errorResult, ApplicationUser user = null)
    {
        return new GrantValidationResult
        {
            ErrorResult = errorResult,
            User = user,
        };
    }
}
