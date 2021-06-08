using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public static class ApplicationUserExtensions
    {
        public static ListDictionary<string, string> DetectUserChanges(this ApplicationUser newUser, ApplicationUser oldUser)
        {
            var result = new ListDictionary<string, string>();

            if (newUser.UserName != oldUser.UserName)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: user name: {oldUser.UserName} -> {newUser.UserName}");
            }

            if (newUser.Email != oldUser.Email)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: email: {oldUser.Email} -> {newUser.Email}");
            }

            if (newUser.UserType != oldUser.UserType)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: user type: {oldUser.UserType} -> {newUser.UserType}");
            }

            if (newUser.IsAdministrator != oldUser.IsAdministrator)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: root: {oldUser.IsAdministrator} -> {newUser.IsAdministrator}");
            }

            if (newUser.MemberId != oldUser.MemberId)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: member ID: {oldUser.MemberId} -> {newUser.MemberId}");
            }

            if (newUser.PasswordHash != oldUser.PasswordHash)
            {
                result.Add(PlatformConstants.Security.Changes.UserPasswordChanged, $"Password changed");
            }

            return result;
        }
    }


}
