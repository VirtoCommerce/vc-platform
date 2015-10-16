using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
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
        private readonly ISecurityOptions _securityOptions;
        private readonly CacheManager _cacheManager;
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly IPermissionScopeService _permissionScopeService;

        public SecurityService(Func<IPlatformRepository> platformRepository, Func<ApplicationUserManager> userManagerFactory, IApiAccountProvider apiAccountProvider,
                               ISecurityOptions securityOptions, IModuleManifestProvider manifestProvider, IPermissionScopeService permissionScopeService, CacheManager cacheManager)
        {
            _platformRepository = platformRepository;
            _userManagerFactory = userManagerFactory;
            _apiAccountProvider = apiAccountProvider;
            _securityOptions = securityOptions;
            _cacheManager = cacheManager;
            _manifestProvider = manifestProvider;
            _permissionScopeService = permissionScopeService;
        }

        #region ISecurityService Members
        public async Task<ApplicationUserExtended> FindByNameAsync(string userName, UserDetails detailsLevel)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindByNameAsync(userName);
                return GetUserExtended(user, detailsLevel);
            }
        }

        public async Task<ApplicationUserExtended> FindByIdAsync(string userId, UserDetails detailsLevel)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindByIdAsync(userId);
                return GetUserExtended(user, detailsLevel);
            }
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
            IdentityResult result = null;
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
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
                    dbAcount.AccountState = AccountState.Approved.ToString();

                    repository.Add(dbAcount);
                    repository.UnitOfWork.Commit();
                }
            }

            return result == null ? null : result.ToCoreModel();
        }

        public async Task<SecurityResult> UpdateAsync(ApplicationUserExtended user)
        {
            SecurityResult result = null;

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //Update ASP.NET indentity user
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await userManager.FindByIdAsync(user.Id);
                result = ValidateUser(dbUser);
                if (result.Succeeded)
                {
                    //Update ASP.NET indentity user
                    user.Patch(dbUser);
                    var identityResult = await userManager.UpdateAsync(dbUser);
                    result = identityResult.ToCoreModel();
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
                foreach (var name in names.Where(IsEditableUser))
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
                var dbUser = await userManager.FindByNameAsync(name);
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
                var dbUser = await userManager.FindByNameAsync(name);
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
                var dbUser = await userManager.FindByIdAsync(userId);
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

            using (var repository = _platformRepository() )
            {
                var query = repository.Accounts;

                if (request.Keyword != null)
                {
                    query = query.Where(u => u.UserName.Contains(request.Keyword));
                }

                if(request.AccountTypes != null && request.AccountTypes.Any())
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
            return _cacheManager.Get(
                CacheKey.Create(CacheGroups.Security, "AllPermissions"),
                LoadAllPermissions);
        }
    
        public bool UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds)
        {
            var user = Task.Run(async () => await FindByNameAsync(userName, UserDetails.Full)).Result;
            if(user == null)
            {
                return false;
            }

            if (user.IsAdministrator)
            {
                return true;
            }
           
            var retVal = user.UserState == Core.Security.AccountState.Approved;
            if (retVal)
            {
                var fqUserPermissions = user.Roles.SelectMany(x => x.Permissions).SelectMany(x => x.GetPermissionWithScopeCombinationNames()).Distinct();
                var fqCheckPermissions = permissionIds.Concat(permissionIds.LeftJoin(scopes, ":"));
                retVal = fqUserPermissions.Intersect(fqCheckPermissions, StringComparer.OrdinalIgnoreCase).Any();
            }
            return retVal;
        }

        public Permission[] GetUserPermissions(string userName)
        {
            var user = Task.Run(async () => await FindByNameAsync(userName, UserDetails.Full)).Result;
            var retVal = Enumerable.Empty<Permission>().ToArray();
            if(user != null)
            {
                retVal = user.Roles.SelectMany(x => x.Permissions).Distinct().ToArray();
            }
            return retVal;
        }
        #endregion


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
            SecurityResult result;

            if (dbUser == null)
            {
                result = new SecurityResult { Errors = new[] { "User not found." } };
            }
            else
            {
                if (!IsEditableUser(dbUser.UserName))
                {
                    result = new SecurityResult { Errors = new[] { "It is forbidden to edit this user." } };
                }
                else
                {
                    result = new SecurityResult { Succeeded = true };
                }
            }

            return result;
        }

        private bool IsEditableUser(string userName)
        {
            var result = true;

            if (_securityOptions != null && _securityOptions.NonEditableUsers != null)
                result = !_securityOptions.NonEditableUsers.Contains(userName);

            return result;
        }

        private ApplicationUserExtended GetUserExtended(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            ApplicationUserExtended result = null;

            if (applicationUser != null)
            {
                using (var repository = _platformRepository())
                {
                    var user = repository.GetAccountByName(applicationUser.UserName, detailsLevel);
                    result = applicationUser.ToCoreModel(user, _permissionScopeService);
                    //Populate available permission scopes
                    if (result.Roles != null)
                    {
                        foreach (var permission in result.Roles.SelectMany(x => x.Permissions).Where(x => x != null))
                        {
                            permission.AvailableScopes = _permissionScopeService.GetAvailablePermissionScopes(permission.Id).ToList();
                        }
                    }

                }

                if (detailsLevel != UserDetails.Export)
                {
                    result.PasswordHash = null;
                    result.SecurityStamp = null;
                }
            }
            return result;
        }

   
    }
}
