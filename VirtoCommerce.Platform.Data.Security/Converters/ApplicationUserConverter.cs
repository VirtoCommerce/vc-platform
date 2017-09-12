using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class ApplicationUserConverter
    {
        public static ApplicationUserExtended ToCoreModel(this ApplicationUser applicationUser, AccountEntity dbEntity, IPermissionScopeService scopeService)
        {
            var retVal = new ApplicationUserExtended();
            retVal = new ApplicationUserExtended();
            retVal.InjectFrom(applicationUser);
            retVal.InjectFrom(dbEntity);
            retVal.UserState = EnumUtility.SafeParse<Core.Security.AccountState>( dbEntity.AccountState, Core.Security.AccountState.Approved);
 
            retVal.Roles = dbEntity.RoleAssignments.Select(x => x.Role.ToCoreModel(scopeService)).ToArray();
            retVal.Permissions = retVal.Roles.SelectMany(x => x.Permissions).SelectMany(x=> x.GetPermissionWithScopeCombinationNames()).Distinct().ToArray();
            retVal.ApiAccounts = dbEntity.ApiAccounts.Select(x => x.ToCoreModel()).ToArray();

            return retVal;
        }

        public static ApplicationUser ToIdentityModel(this ApplicationUserExtended user)
        {
            var retVal = new ApplicationUser();
            user.Patch(retVal);
            return retVal;
        }

        public static AccountEntity ToDataModel(this ApplicationUserExtended user)
        {
            var retVal = new AccountEntity();
            retVal.InjectFrom(user);

            retVal.AccountState = user.UserState.ToString();

            if(user.Roles != null)
            {
                retVal.RoleAssignments = new ObservableCollection<RoleAssignmentEntity>(user.Roles.Select(x => x.ToAssignmentDataModel()));
            }
            if(user.ApiAccounts != null)
            {
                retVal.ApiAccounts = new ObservableCollection<ApiAccountEntity>(user.ApiAccounts.Select(x => x.ToDataModel()));
            }
            return retVal;     
        }

        public static void Patch(this AccountEntity source, AccountEntity target)
        {
            var patchInjection = new PatchInjection<AccountEntity>(x => x.UserType, x => x.AccountState, x => x.MemberId,
                                                                   x => x.StoreId, x => x.IsAdministrator);
            target.InjectFrom(patchInjection, source);

            if (!source.ApiAccounts.IsNullCollection())
            {
                source.ApiAccounts.Patch(target.ApiAccounts, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
            }
            if (!source.RoleAssignments.IsNullCollection())
            {
                source.RoleAssignments.Patch(target.RoleAssignments, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
            }
        }

        public static void Patch(this ApplicationUserExtended user, ApplicationUser dbUser)
        {
            var patchInjection = new PatchInjection<ApplicationUser>(x => x.Id, x => x.PasswordHash, x => x.SecurityStamp,
                                                                     x => x.UserName, x => x.Email, x => x.PhoneNumber);
            dbUser.InjectFrom(patchInjection, user);

            // Copy logins
            if (user.Logins != null)
            {
                var changedLogins = user.Logins.Select(x => new IdentityUserLogin
                {
                    LoginProvider = x.LoginProvider,
                    ProviderKey = x.ProviderKey,
                    UserId = dbUser.Id
                }).ToList();

                var comparer = AnonymousComparer.Create((IdentityUserLogin x) => x.LoginProvider);
                changedLogins.Patch(dbUser.Logins, comparer, (sourceItem, targetItem) => { sourceItem.ProviderKey = targetItem.ProviderKey; });
            }
        
        }
    }
}
