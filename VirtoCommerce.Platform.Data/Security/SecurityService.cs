using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.AspNet.Identity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security.Converters;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security
{
    public class SecurityService : ServiceBase, ISecurityService
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly Func<ApplicationUserManager> _userManagerFactory;
        private readonly IApiAccountProvider _apiAccountProvider;
        private readonly ICacheManager<object> _cacheManager;
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly IPermissionScopeService _permissionScopeService;

        [CLSCompliant(false)]
        public SecurityService(Func<IPlatformRepository> platformRepository, Func<ApplicationUserManager> userManagerFactory, IApiAccountProvider apiAccountProvider,
                               IModuleManifestProvider manifestProvider, IPermissionScopeService permissionScopeService, ICacheManager<object> cacheManager)
        {
            _platformRepository = platformRepository;
            _userManagerFactory = userManagerFactory;
            _apiAccountProvider = apiAccountProvider;
            _cacheManager = cacheManager;
            _manifestProvider = manifestProvider;
            _permissionScopeService = permissionScopeService;
        }

        #region ISecurityService Members
        public async Task<ApplicationUserExtended> FindByNameAsync(string userName, UserDetails detailsLevel)
        {
            var user = await GetApplicationUserByNameAsync(userName);
            return GetUserExtended(user, detailsLevel);
        }

        public async Task<ApplicationUserExtended> FindByIdAsync(string userId, UserDetails detailsLevel)
        {
            var user = await GetApplicationUserByIdAsync(userId);
            return GetUserExtended(user, detailsLevel);
        }

        public async Task<ApplicationUserExtended> FindByEmailAsync(string email, UserDetails detailsLevel)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindByEmailAsync(email);
                return GetUserExtended(user, detailsLevel);
            }
        }

        public async Task<ApplicationUserExtended> FindByLoginAsync(string loginProvider, string providerKey, UserDetails detailsLevel)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindAsync(new UserLoginInfo(loginProvider, providerKey));
                return GetUserExtended(user, detailsLevel);
            }
        }

        public async Task<SecurityResult> CreateAsync(ApplicationUserExtended user)
        {
            IdentityResult result;

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            NormalizeUser(user);

            //Update ASP.NET indentity user
            using (var userManager = _userManagerFactory())
            {
                var dbUser = user.ToIdentityModel();
                user.Id = dbUser.Id;

                if (string.IsNullOrEmpty(user.Password))
                {
                    result = await userManager.CreateAsync(dbUser);
                }
                else
                {
                    result = await userManager.CreateAsync(dbUser, user.Password);
                }
            }

            if (result.Succeeded)
            {
                using (var repository = _platformRepository())
                {
                    var dbAcount = user.ToDataModel();
                    if(string.IsNullOrEmpty(user.MemberId))
                    {
                        //Use for memberId same account id if its not set (Our current case Contact member 1 - 1 Account workaround). But client may use memberId as for any outer id.
                        dbAcount.MemberId = dbAcount.Id;
                    }
                    dbAcount.AccountState = AccountState.Approved.ToString();

                    repository.Add(dbAcount);
                    repository.UnitOfWork.Commit();
                }
            }

            return result.ToCoreModel();
        }

        public async Task<SecurityResult> UpdateAsync(ApplicationUserExtended user)
        {
            SecurityResult result;

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            NormalizeUser(user);

            //Update ASP.NET indentity user
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await userManager.FindByIdAsync(user.Id);
                result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    var userName = dbUser.UserName;

                    //Update ASP.NET indentity user
                    user.Patch(dbUser);
                    var identityResult = await userManager.UpdateAsync(dbUser);
                    result = identityResult.ToCoreModel();

                    //clear cache
                    RemoveUserFromCache(user.Id, userName);
                }
            }

            if (result.Succeeded)
            {
                //Update platform security user
                using (var repository = _platformRepository())
                {
                    var targetDbAcount = repository.GetAccountByName(user.UserName, UserDetails.Full);

                    if (targetDbAcount == null)
                    {
                        result = new SecurityResult { Errors = new[] { "Account not found." } };
                    }
                    else
                    {
                        var changedDbAccount = user.ToDataModel();
                        using (var changeTracker = GetChangeTracker(repository))
                        {
                            changeTracker.Attach(targetDbAcount);

                            changedDbAccount.Patch(targetDbAcount);
                            repository.UnitOfWork.Commit();
                        }
                    }
                }
            }

            return result;
        }

        public async Task DeleteAsync(string[] names)
        {
            using (var userManager = _userManagerFactory())
            {
                foreach (var name in names)
                {
                    var dbUser = await userManager.FindByNameAsync(name);

                    if (dbUser != null)
                    {
                        await userManager.DeleteAsync(dbUser);

                        using (var repository = _platformRepository())
                        {
                            var account = repository.GetAccountByName(name, UserDetails.Reduced);
                            if (account != null)
                            {
                                repository.Remove(account);
                                repository.UnitOfWork.Commit();
                            }
                        }

                        //clear cache
                        RemoveUserFromCache(dbUser.Id, name);
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

        public async Task<SecurityResult> ChangePasswordAsync(string name, string oldPassword, string newPassword)
        {
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await GetApplicationUserByNameAsync(name);
                var result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    var identityResult = await userManager.ChangePasswordAsync(dbUser.Id, oldPassword, newPassword);
                    result = identityResult.ToCoreModel();
                }

                return result;
            }
        }

        public async Task<SecurityResult> ResetPasswordAsync(string name, string newPassword)
        {
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await GetApplicationUserByNameAsync(name);
                var result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(dbUser.Id);
                    var identityResult = await userManager.ResetPasswordAsync(dbUser.Id, token, newPassword);
                    result = identityResult.ToCoreModel();
                }

                return result;
            }
        }

        public async Task<SecurityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await GetApplicationUserByIdAsync(userId);
                var result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    var identityResult = await userManager.ResetPasswordAsync(userId, token, newPassword);
                    result = identityResult.ToCoreModel();
                }

                return result;
            }
        }

        public async Task<UserSearchResponse> SearchUsersAsync(UserSearchRequest request)
        {
            request = request ?? new UserSearchRequest();
            var result = new UserSearchResponse();

            using (var repository = _platformRepository())
            {
                var query = repository.Accounts;

                if (request.Keyword != null)
                {
                    query = query.Where(u => u.UserName.Contains(request.Keyword));
                }

                if(!string.IsNullOrEmpty(request.MemberId))
                {
                    //Find all accounts with specified memberId
                    query = query.Where(u => u.MemberId == request.MemberId);
                }

                if (request.AccountTypes != null && request.AccountTypes.Any())
                {
                    query = query.Where(x => request.AccountTypes.Contains(x.UserType));
                }
                result.TotalCount = query.Count();

                var users = query.OrderBy(x => x.UserName)
                                 .Skip(request.SkipCount)
                                 .Take(request.TakeCount)
                                 .ToArray();

                var extendedUsers = new List<ApplicationUserExtended>();

                foreach (var user in users)
                {
                    var extendedUser = await FindByNameAsync(user.UserName, UserDetails.Reduced);
                    if (extendedUser != null)
                    {
                        extendedUsers.Add(extendedUser);
                    }
                }

                result.Users = extendedUsers.ToArray();

                return result;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            using (var userManager = _userManagerFactory())
            {
                return await userManager.GeneratePasswordResetTokenAsync(userId);
            }
        }

        public Permission[] GetAllPermissions()
        {
            return _cacheManager.Get("AllPermissions", "PlatformRegion", LoadAllPermissions);
        }

        public bool UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds)
        {
            if (permissionIds == null)
            {
                throw new ArgumentNullException("permissionIds");
            }

            var user = FindByName(userName, UserDetails.Full);

            var result = user != null && user.UserState == AccountState.Approved;

            if (result && user.IsAdministrator)
            {
                return true;
            }

            //For managers always allow to call api
            if (result && permissionIds.Length == 1 && permissionIds.Contains(PredefinedPermissions.SecurityCallApi)
               && (string.Equals(user.UserType, AccountType.Manager.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(user.UserType, AccountType.Administrator.ToString(), StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }

            if (result)
            {
                var fqUserPermissions = user.Roles.SelectMany(x => x.Permissions).SelectMany(x => x.GetPermissionWithScopeCombinationNames()).Distinct();
                var fqCheckPermissions = permissionIds.Concat(permissionIds.LeftJoin(scopes, ":"));
                result = fqUserPermissions.Intersect(fqCheckPermissions, StringComparer.OrdinalIgnoreCase).Any();
            }

            return result;
        }

        public Permission[] GetUserPermissions(string userName)
        {
            var user = FindByName(userName, UserDetails.Full);
            var result = user != null ? user.Roles.SelectMany(x => x.Permissions).Distinct().ToArray() : Enumerable.Empty<Permission>().ToArray();
            return result;
        }
        #endregion

        private ApplicationUserExtended FindByName(string userName, UserDetails detailsLevel)
        {
            var user = GetApplicationUserByName(userName);
            return GetUserExtended(user, detailsLevel);
        }

        private Permission[] LoadAllPermissions()
        {
            var manifestPermissions = new List<Permission>();

            var manifestsWithPermissions = _manifestProvider.GetModuleManifests().Values
                .Where(m => m.Permissions != null);

            foreach (var module in manifestsWithPermissions)
            {
                foreach (var group in module.Permissions)
                {
                    if (group.Permissions != null)
                    {
                        foreach (var modulePermission in group.Permissions)
                        {
                            var permission = modulePermission.ToCoreModel(module.Id, group.Name);
                            permission.AvailableScopes = _permissionScopeService.GetAvailablePermissionScopes(permission.Id).ToList();
                            manifestPermissions.Add(permission);
                        }
                    }
                }
            }

            var allPermissions = PredefinedPermissions.Permissions.Union(manifestPermissions).ToArray();
            return allPermissions;
        }

        private SecurityResult ValidateUser(ApplicationUser dbUser)
        {
            var result = new SecurityResult { Succeeded = true };

            if (dbUser == null)
            {
                result = new SecurityResult { Errors = new[] { "User not found." } };
            }

            return result;
        }

        private async Task<ApplicationUser> GetApplicationUserByIdAsync(string userId)
        {
            var cacheRegion = GetUserCacheRegion(userId);

            var result = await _cacheManager.GetAsync(cacheRegion, cacheRegion, async () =>
            {
                using (var userManager = _userManagerFactory())
                {
                    return await userManager.FindByIdAsync(userId);
                }
            });

            return result;
        }

        private ApplicationUser GetApplicationUserByName(string userName)
        {
            var cacheRegion = GetUserCacheRegion(userName);

            var result = _cacheManager.Get(cacheRegion, cacheRegion, () =>
            {
                using (var userManager = _userManagerFactory())
                {
                    return Task.Run(async () => await userManager.FindByNameAsync(userName)).Result;
                }
            });

            return result;
        }

        private async Task<ApplicationUser> GetApplicationUserByNameAsync(string userName)
        {
            var cacheRegion = GetUserCacheRegion(userName);

            var result = await _cacheManager.GetAsync(cacheRegion, cacheRegion, async () =>
            {
                using (var userManager = _userManagerFactory())
                {
                    return await userManager.FindByNameAsync(userName);
                }
            });

            return result;
        }

        private void RemoveUserFromCache(string userId, string userName)
        {
            _cacheManager.ClearRegion(GetUserCacheRegion(userId));
            _cacheManager.ClearRegion(GetUserCacheRegion(userName));
        }

        private ApplicationUserExtended GetUserExtended(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            ApplicationUserExtended result = null;
            if (applicationUser != null)
            {
                var cacheRegion = GetUserCacheRegion(applicationUser.Id);
                result = _cacheManager.Get(cacheRegion + ":" + detailsLevel, cacheRegion, () =>
                {
                    ApplicationUserExtended retVal;
                    using (var repository = _platformRepository())
                    {
                        var user = repository.GetAccountByName(applicationUser.UserName, detailsLevel);
                        retVal = applicationUser.ToCoreModel(user, _permissionScopeService);
                        //Populate available permission scopes
                        if (retVal.Roles != null)
                        {
                            foreach (var permission in retVal.Roles.SelectMany(x => x.Permissions).Where(x => x != null))
                            {
                                permission.AvailableScopes = _permissionScopeService.GetAvailablePermissionScopes(permission.Id).ToList();
                            }
                        }
                    }

                    if (detailsLevel != UserDetails.Export)
                    {
                        retVal.PasswordHash = null;
                        retVal.SecurityStamp = null;
                    }
                    return retVal;
                });
            }
            return result;
        }

        private static string GetUserCacheRegion(string userId)
        {
            return "AppUserRegion:" + userId;
        }

        private static void NormalizeUser(ApplicationUserExtended user)
        {
            if (user.UserName != null)
                user.UserName = user.UserName.Trim();

            if (user.Email != null)
                user.Email = user.Email.Trim();

            if (user.PhoneNumber != null)
                user.PhoneNumber = user.PhoneNumber.Trim();
        }
    }
}
