using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public static class ApplicationUserExtensions
    {
        public static ListDictionary<string, string> DetectUserChanges(this ApplicationUser newUser, ApplicationUser oldUser)
        {
            //Log changes
            var result = new ListDictionary<string, string>();
            if (newUser.UserName != oldUser.UserName)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: user name: {oldUser.UserName} -> {newUser.UserName}");
            }
            if (newUser.UserType != oldUser.UserType)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: user type: {oldUser.UserType} -> {newUser.UserType}");
            }
            if (newUser.IsAdministrator != oldUser.IsAdministrator)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: root: {oldUser.IsAdministrator} -> {newUser.IsAdministrator}");
            }

            if (newUser.PasswordHash != oldUser.PasswordHash)
            {
                result.Add(PlatformConstants.Security.Changes.UserPasswordChanged, $"Password changed");
            }

            //todo add after the implementation
            //if (!newUser.ApiAccounts.IsNullOrEmpty())
            //{
            //    var apiAccountComparer = AnonymousComparer.Create((ApiAccount x) => $"{x.ApiAccountType}-{x.SecretKey}");
            //    newUser.ApiAccounts.CompareTo(oldUser.ApiAccounts ?? Array.Empty<ApiAccount>(), apiAccountComparer, (state, sourceItem, targetItem) =>
            //    {
            //        if (state == EntryState.Added)
            //        {
            //            result.Add("Activated Api Key(s) [{0}] ", $"{sourceItem.Name} ({sourceItem.ApiAccountType})");
            //        }
            //        else if (state == EntryState.Deleted)
            //        {
            //            result.Add("Deactivated Api Key(s) [{0}]</value>", $"{sourceItem.Name} ({sourceItem.ApiAccountType})");
            //        }
            //    }
            //    );
            //}

            return result;
        }

        public static ListDictionary<string, string> DetectUserRoleChanges(this ApplicationUser newUser, ApplicationUser oldUser)
        {
            var result = new ListDictionary<string, string>();

            if (!newUser.Roles.IsNullOrEmpty() || !oldUser.Roles.IsNullOrEmpty())
            {
                newUser.Roles.CompareTo(oldUser.Roles ?? Array.Empty<Role>(), EqualityComparer<Role>.Default, (state, sourceItem, targetItem) =>
                {
                    if (state == EntryState.Added)
                    {
                        result.Add(PlatformConstants.Security.Changes.RoleAdded, $"Added role(s) [{sourceItem?.Name}]");
                    }
                    else if (state == EntryState.Deleted)
                    {
                        result.Add(PlatformConstants.Security.Changes.RoleRemoved, $"Removed role(s) [{sourceItem?.Name}]");
                    }
                });
            }

            return result;
        }
    }


}
