namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public class ExternalSignInResult
{
    public bool Success { get; set; }
    public string LoginProvider { get; set; }
    public ApplicationUser User { get; set; }

    public static ExternalSignInResult Fail()
    {
        return new ExternalSignInResult
        {
            Success = false,
            User = null,
        };
    }

    public static ExternalSignInResult Succeed(string loginProvider, ApplicationUser user)
    {
        return new ExternalSignInResult
        {
            Success = true,
            LoginProvider = loginProvider,
            User = user,
        };
    }
}
