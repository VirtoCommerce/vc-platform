using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.ExternalSignIn
{
    public interface IExternalSignInProvider
    {
        int Priority { get; }

        bool HasLoginForm { get; }

        bool AllowCreateNewUser { get; }

        string GetUserName(ExternalLoginInfo externalLoginInfo);

        string GetEmail(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirstValue(OpenIddictConstants.Claims.Email) ??
                   externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        }

        string GetUserType();
        string[] GetUserRoles();
    }
}
