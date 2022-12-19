using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Security.ExternalSignIn
{
    public interface IExternalSignInProvider
    {
        int Priority { get; }

        bool HasLoginForm { get; }

        bool AllowCreateNewUser { get; }

        string GetUserName(ExternalLoginInfo externalLoginInfo);

        string GetUserType();
    }
}
