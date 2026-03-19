using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Core.Extensions;

public static class UserManagerExtensions
{
    public static string SanitizeUserName(this UserManager<ApplicationUser> userManager, string userName, char replacementCharacter = '?')
    {
        if (userName.IsNullOrEmpty())
        {
            return string.Empty;
        }

        var characters = userName.ToCharArray();
        var allowedCharacters = userManager.Options.User.AllowedUserNameCharacters;

        for (var i = characters.Length - 1; i >= 0; i--)
        {
            if (!allowedCharacters.Contains(characters[i]))
            {
                characters[i] = replacementCharacter;
            }
        }

        return new string(characters);
    }
}
