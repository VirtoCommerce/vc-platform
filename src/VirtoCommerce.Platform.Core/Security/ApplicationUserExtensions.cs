using VirtoCommerce.Platform.Core.ChangesUtils;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public static class ApplicationUserExtensions
    {
        public static ListDictionary<string, string> DetectUserChanges(this ApplicationUser newUser, ApplicationUser oldUser)
        {
            var result = ChangesDetector.Gather(newUser, oldUser);

            if (newUser.UserName != oldUser.UserName)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: user name: {oldUser.UserName} -> {newUser.UserName}");
            }

            if (newUser.Email != oldUser.Email)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: email: {oldUser.Email} -> {newUser.Email}");
            }

            if (newUser.PasswordHash != oldUser.PasswordHash)
            {
                result.Add(PlatformConstants.Security.Changes.UserPasswordChanged, $"Password changed");
            }

            return result;
        }
    }
}
