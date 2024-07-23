namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public class ExternalSignInResult
{
    public bool Success { get; set; }
    public ApplicationUser User { get; set; }

    public static ExternalSignInResult Fail()
    {
        return new ExternalSignInResult
        {
            Success = false,
            User = null,
        };
    }

    public static ExternalSignInResult Succeed(ApplicationUser user)
    {
        return new ExternalSignInResult
        {
            Success = true,
            User = user,
        };
    }
}
