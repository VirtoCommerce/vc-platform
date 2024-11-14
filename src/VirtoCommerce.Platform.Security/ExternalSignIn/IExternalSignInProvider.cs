using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;

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
            return externalLoginInfo.Principal.FindAnyValue([OpenIddictConstants.Claims.Email, ClaimTypes.Email]);
        }

        string GetUserType();
        string[] GetUserRoles();
    }
}
