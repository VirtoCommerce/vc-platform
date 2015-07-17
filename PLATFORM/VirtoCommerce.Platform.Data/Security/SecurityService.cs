using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security.Converters;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security
{
    public class SecurityService
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly ApplicationUserManager _userManager;
        private readonly IApiAccountProvider _apiAccountProvider;

        public SecurityService(Func<IPlatformRepository> platformRepository, ApplicationUserManager userManager, IApiAccountProvider apiAccountProvider)
        {
            _platformRepository = platformRepository;
            _userManager = userManager;
            _apiAccountProvider = apiAccountProvider;
        }

        public async Task<ApplicationUserExtended> GetUserExtended(string userName, UserDetails detailsLevel)
        {
            var applicationUser = await _userManager.FindByNameAsync(userName);
            return GetUserExtended(applicationUser, detailsLevel);
        }

        public async Task<IdentityResult> Register(ApplicationUserExtended user)
        {
            IdentityResult result = null;

            if (user != null)
            {
                var dbUser = new ApplicationUser();
                dbUser.InjectFrom(user);

                if (user.Logins != null)
                {
                    foreach (var login in user.Logins)
                    {
                        var userLogin = dbUser.Logins.FirstOrDefault(l => l.LoginProvider == login.LoginProvider);
                        if (userLogin != null)
                        {
                            userLogin.ProviderKey = login.ProviderKey;
                        }
                        else
                        {
                            dbUser.Logins.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin
                            {
                                LoginProvider = login.LoginProvider,
                                ProviderKey = login.ProviderKey,
                                UserId = dbUser.Id
                            });
                        }
                    }
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    result = await _userManager.CreateAsync(dbUser);
                }
                else
                {
                    result = await _userManager.CreateAsync(dbUser, user.Password);
                }

                if (result.Succeeded)
                {
                    using (var repository = _platformRepository())
                    {
                        var account = new AccountEntity
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            MemberId = user.MemberId,
                            AccountState = AccountState.Approved,
                            RegisterType = (RegisterType)user.UserType,
                            StoreId = user.StoreId,
                        };

                        repository.Add(account);
                        repository.UnitOfWork.Commit();
                    }
                }
            }

            return result;
        }

        public async Task<string> CreateAsync(ApplicationUserExtended user)
        {
            var dbUser = new ApplicationUser();
            dbUser.InjectFrom(user);

            IdentityResult result;
            if (!string.IsNullOrEmpty(user.Password))
            {
                result = await _userManager.CreateAsync(dbUser, user.Password);
            }
            else
            {
                result = await _userManager.CreateAsync(dbUser);
            }

            if (result.Succeeded)
            {
                using (var repository = _platformRepository())
                {
                    var account = new AccountEntity
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        MemberId = user.MemberId,
                        AccountState = AccountState.Approved,
                        RegisterType = (RegisterType)user.UserType,
                        StoreId = user.StoreId
                    };

                    if (user.Roles != null)
                    {
                        foreach (var role in user.Roles)
                        {
                            account.RoleAssignments.Add(new RoleAssignmentEntity { RoleId = role.Id, AccountId = account.Id });
                        }
                    }

                    repository.Add(account);
                    repository.UnitOfWork.Commit();
                }

                return null;
            }

            return string.Join(" ", result.Errors);
        }

        public async Task<string> UpdateAsync(ApplicationUserExtended user)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            dbUser.InjectFrom(user);

            if (user.Logins != null)
            {
                foreach (var login in user.Logins)
                {
                    var userLogin = dbUser.Logins.FirstOrDefault(l => l.LoginProvider == login.LoginProvider);
                    if (userLogin != null)
                    {
                        userLogin.ProviderKey = login.ProviderKey;
                    }
                    else
                    {
                        dbUser.Logins.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin
                        {
                            LoginProvider = login.LoginProvider,
                            ProviderKey = login.ProviderKey,
                            UserId = dbUser.Id
                        });
                    }
                }
            }

            var result = await _userManager.UpdateAsync(dbUser);

            if (result.Succeeded)
            {
                using (var repository = _platformRepository())
                {
                    var acount = repository.GetAccountByName(user.UserName, UserDetails.Full);

                    if (acount == null)
                    {
                        return "Acount not found";
                    }

                    acount.RegisterType = (RegisterType)user.UserType;
                    acount.AccountState = (AccountState)user.UserState;
                    acount.MemberId = user.MemberId;
                    acount.StoreId = user.StoreId;

                    if (user.ApiAcounts != null)
                    {
                        var source = new ObservableCollection<ApiAccountEntity>(user.ApiAcounts.Select(x => x.ToEntity()));
                        var inventoryComparer = AnonymousComparer.Create((ApiAccountEntity x) => x.Id);
                        acount.ApiAccounts.ObserveCollection(x => repository.Add(x), x => repository.Remove(x));
                        source.Patch(acount.ApiAccounts, inventoryComparer, (sourceAccount, targetAccount) => sourceAccount.Patch(targetAccount));
                    }

                    if (user.Roles != null)
                    {
                        var sourceCollection = new ObservableCollection<RoleAssignmentEntity>(user.Roles.Select(r => new RoleAssignmentEntity { RoleId = r.Id }));
                        var comparer = AnonymousComparer.Create((RoleAssignmentEntity x) => x.RoleId);
                        acount.RoleAssignments.ObserveCollection(ra => repository.Add(ra), ra => repository.Remove(ra));
                        sourceCollection.Patch(acount.RoleAssignments, comparer, (source, target) => source.Patch(target));
                    }

                    repository.UnitOfWork.Commit();
                }

                return null;
            }

            return string.Join(" ", result.Errors);
        }

        public async Task DeleteAsync(string[] names)
        {
            foreach (var name in names)
            {
                var dbUser = await _userManager.FindByNameAsync(name);

                if (dbUser != null)
                {
                    await _userManager.DeleteAsync(dbUser);

                    using (var repository = _platformRepository())
                    {
                        var account = repository.GetAccountByName(name, UserDetails.Reduced);
                        if (account != null)
                        {
                            repository.Remove(account);
                            repository.UnitOfWork.Commit();
                        }
                    }
                }
            }
        }

        public ApiAccount GenerateNewApiAccount(ApiAccountType type)
        {
            var apiAccount = _apiAccountProvider.GenerateApiCredentials(type);
            var result = apiAccount.ToCoreModel();
            return result;
        }

        public async Task<ApplicationUserExtended> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return GetUserExtended(user, UserDetails.Reduced);
        }

        public async Task<ApplicationUserExtended> FindByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return GetUserExtended(user, UserDetails.Reduced);
        }

        public async Task<ApplicationUserExtended> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return GetUserExtended(user, UserDetails.Reduced);
        }

        public async Task<ApplicationUserExtended> FindByLoginAsync(string loginProvider, string providerKey)
        {
            var loginInfo = new UserLoginInfo(loginProvider, providerKey);
            var user = await _userManager.FindAsync(loginInfo);
            return GetUserExtended(user, UserDetails.Reduced);
        }

        public async Task<IdentityResult> ChangePassword(string name, string oldPassword, string newPassword)
        {
            IdentityResult result = null;

            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                result = await _userManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);
            }

            return result;
        }

        public async Task<UserSearchResponse> SearchUsersAsync(UserSearchRequest request)
        {
            request = request ?? new UserSearchRequest();
            var result = new UserSearchResponse();

            var query = _userManager.Users;

            if (request.Keyword != null)
            {
                query = query.Where(u => u.UserName.Contains(request.Keyword));
            }

            result.TotalCount = query.Count();

            var users = query.OrderBy(x => x.UserName)
                             .Skip(request.SkipCount)
                             .Take(request.TakeCount)
                             .ToArray();

            var extendedUsers = new List<ApplicationUserExtended>();

            foreach (var user in users)
            {
                var extendedUser = await GetUserExtended(user.UserName, UserDetails.Reduced);
                extendedUsers.Add(extendedUser);
            }

            result.Users = extendedUsers.ToArray();

            return result;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(userId);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(userId, token, newPassword);
        }


        private ApplicationUserExtended GetUserExtended(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            ApplicationUserExtended result = null;

            if (applicationUser != null)
            {
                result = new ApplicationUserExtended();
                result.InjectFrom(applicationUser);

                using (var repository = _platformRepository())
                {
                    var user = repository.GetAccountByName(applicationUser.UserName, detailsLevel);

                    if (user != null)
                    {
                        result.InjectFrom(user);

                        result.UserState = (UserState)user.AccountState;
                        result.UserType = (UserType)user.RegisterType;

                        if (detailsLevel == UserDetails.Full)
                        {
                            var roles = user.RoleAssignments.Select(x => x.Role).ToArray();
                            result.Roles = roles.Select(r => r.ToCoreModel(false)).ToArray();

                            var permissionIds = roles
                                    .SelectMany(x => x.RolePermissions)
                                    .Select(x => x.PermissionId)
                                    .Distinct()
                                    .ToArray();

                            result.Permissions = permissionIds;
                            result.ApiAcounts = user.ApiAccounts.Select(x => x.ToCoreModel()).ToArray();
                        }
                    }
                }
            }

            return result;
        }
    }
}
