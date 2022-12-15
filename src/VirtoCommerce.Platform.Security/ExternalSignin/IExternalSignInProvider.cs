using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Security.ExternalSignIn
{
    public interface IExternalSignInProvider
    {
        bool AllowCreateNewUser { get; }

        string GetUserName(ExternalLoginInfo externalLoginInfo);

        string GetUserType();
    }
}
