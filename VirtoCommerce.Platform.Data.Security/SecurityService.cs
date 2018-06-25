using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.AspNet.Identity;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security.Converters;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Data.Security.Resources;

namespace VirtoCommerce.Platform.Data.Security
{
    public class SecurityService : ServiceBase, ISecurityService
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly Func<ApplicationUserManager> _userManagerFactory;
        private readonly IApiAccountProvider _apiAccountProvider;
        private readonly ICacheManager<object> _cacheManager;
        private readonly IModuleCatalog _moduleCatalog;
        private readonly IPermissionScopeService _permissionScopeService;
        private readonly IChangeLogService _changeLogService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISettingsManager _settingsManager;

        [CLSCompliant(false)]
        public SecurityService(Func<IPlatformRepository> platformRepository, Func<ApplicationUserManager> userManagerFactory, IApiAccountProvider apiAccountProvider,
                               IModuleCatalog moduleCatalog, IPermissionScopeService permissionScopeService, ICacheManager<object> cacheManager,
                               IChangeLogService changeLogService, IEventPublisher eventPublisher, ISettingsManager settingsManager)
        {
            _platformRepository = platformRepository;
            _userManagerFactory = userManagerFactory;
            _apiAccountProvider = apiAccountProvider;
            _cacheManager = cacheManager;
            _moduleCatalog = moduleCatalog;
            _permissionScopeService = permissionScopeService;
            _changeLogService = changeLogService;
            _eventPublisher = eventPublisher;
            _settingsManager = settingsManager;
        }

        #region ISecurityService Members
        public virtual async Task<ApplicationUserExtended> FindByNameAsync(string userName, UserDetails detailsLevel)
        {
            var user = await GetApplicationUserByNameAsync(userName);
            return await GetUserExtendedAsync(user, detailsLevel);
        }

        public virtual async Task<ApplicationUserExtended> FindByIdAsync(string userId, UserDetails detailsLevel)
        {
            var user = await GetApplicationUserByIdAsync(userId);
            return await GetUserExtendedAsync(user, detailsLevel);
        }

        public virtual async Task<ApplicationUserExtended> FindByEmailAsync(string email, UserDetails detailsLevel)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindByEmailAsync(email);
                return await GetUserExtendedAsync(user, detailsLevel);
            }
        }

        public virtual async Task<ApplicationUserExtended> FindByLoginAsync(string loginProvider, string providerKey, UserDetails detailsLevel)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindAsync(new UserLoginInfo(loginProvider, providerKey));
                return await GetUserExtendedAsync(user, detailsLevel);
            }
        }

        public virtual async Task<SecurityResult> CreateAsync(ApplicationUserExtended user)
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
                    if (string.IsNullOrEmpty(user.MemberId))
                    {
                        //Use for memberId same account id if its not set (Our current case Contact member 1 - 1 Account workaround). But client may use memberId as for any outer id.
                        dbAcount.MemberId = dbAcount.Id;
                    }
                    dbAcount.AccountState = AccountState.Approved.ToString();

                    repository.Add(dbAcount);

                    var userChangedEntry = new ChangedEntry<ApplicationUserExtended>(user, EntryState.Added);
                    await _eventPublisher.Publish(new UserChangingEvent(userChangedEntry));
                    repository.UnitOfWork.Commit();
                    await _eventPublisher.Publish(new UserChangedEvent(userChangedEntry));
                }
            }

            return result.ToCoreModel();
        }

        public virtual async Task<SecurityResult> UpdateAsync(ApplicationUserExtended user)
        {
            SecurityResult result;

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            NormalizeUser(user);

            //Update ASP.NET indentity user
            var userName = string.Empty;
            ApplicationUser dbUser = null;
            using (var userManager = _userManagerFactory())
            {
                dbUser = await userManager.FindByIdAsync(user.Id);
                result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    userName = dbUser.UserName;

                    //Update ASP.NET indentity user
                    user.Patch(dbUser);
                    var identityResult = await userManager.UpdateAsync(dbUser);
                    result = identityResult.ToCoreModel();

                    //clear cache
                    ResetCache(user.Id, userName);
                }
            }

            if (result.Succeeded)
            {
                //Update platform security user
                using (var repository = _platformRepository())
                {
                    var targetDbAcount = await repository.GetAccountByNameAsync(userName, UserDetails.Full);

                    if (targetDbAcount == null)
                    {
                        result = new SecurityResult { Errors = new[] { "Account not found." } };
                    }
                    else
                    {
                        var changedDbAccount = user.ToDataModel();
                        using (var changeTracker = GetChangeTracker(repository))
                        {
                            var userChangedEntry = new ChangedEntry<ApplicationUserExtended>(user, dbUser.ToCoreModel(targetDbAcount, _permissionScopeService), EntryState.Modified);
                            changeTracker.Attach(targetDbAcount);
                            changedDbAccount.Patch(targetDbAcount);

                            await _eventPublisher.Publish(new UserChangingEvent(userChangedEntry));
                            repository.UnitOfWork.Commit();
                            await _eventPublisher.Publish(new UserChangedEvent(userChangedEntry));
                        }
                    }
                }
            }

            return result;
        }

        public virtual async Task DeleteAsync(string[] names)
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
                            var account = await repository.GetAccountByNameAsync(name, UserDetails.Reduced);
                            if (account != null)
                            {
                                var userChangedEntry = new ChangedEntry<ApplicationUserExtended>(dbUser.ToCoreModel(account, _permissionScopeService), EntryState.Deleted);
                                repository.Remove(account);
                                await _eventPublisher.Publish(new UserChangingEvent(userChangedEntry));
                                repository.UnitOfWork.Commit();
                                await _eventPublisher.Publish(new UserChangedEvent(userChangedEntry));
                            }
                        }
                        //clear cache
                        ResetCache(dbUser.Id, name);
                    }
                }
            }
        }

        public virtual ApiAccount GenerateNewApiAccount(ApiAccountType type)
        {
            var apiAccount = _apiAccountProvider.GenerateApiCredentials(type);
            var result = apiAccount.ToCoreModel();
            return result;
        }

        public virtual ApiAccount GenerateNewApiKey(ApiAccount account)
        {
            if (account.ApiAccountType != ApiAccountType.Hmac)
            {
                throw new InvalidOperationException(SecurityAccountExceptions.NonHmacKeyGenerationException);
            }
            account = _apiAccountProvider.GenerateApiKey(account.ToDataModel()).ToCoreModel();
            return account;
        }

        public virtual async Task<SecurityResult> ChangePasswordAsync(string name, string oldPassword, string newPassword)
        {
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await GetApplicationUserByNameAsync(name);
                var result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    var identityResult = await userManager.ChangePasswordAsync(dbUser.Id, oldPassword, newPassword);
                    ResetCache(dbUser.Id, dbUser.UserName);
                    result = identityResult.ToCoreModel();

                    if (result.Succeeded)
                    {
                        await _eventPublisher.Publish(new UserPasswordChangedEvent(dbUser.Id));
                    }
                }
                return result;
            }
        }

        public virtual async Task<SecurityResult> ResetPasswordAsync(string name, string newPassword)
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

                    if (result.Succeeded)
                    {
                        await _eventPublisher.Publish(new UserResetPasswordEvent(dbUser.Id));
                        //clear cache
                        ResetCache(dbUser.Id, dbUser.UserName);
                    }
                }

                return result;
            }
        }

        public virtual async Task<SecurityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            using (var userManager = _userManagerFactory())
            {
                var dbUser = await GetApplicationUserByIdAsync(userId);
                var result = ValidateUser(dbUser);

                if (result.Succeeded)
                {
                    var identityResult = await userManager.ResetPasswordAsync(userId, token, newPassword);
                    result = identityResult.ToCoreModel();

                    if (result.Succeeded)
                    {
                        await _eventPublisher.Publish(new UserResetPasswordEvent(userId));
                        //clear cache
                        ResetCache(dbUser.Id, dbUser.UserName);
                    }
                }

                return result;
            }
        }

        public virtual async Task<UserSearchResponse> SearchUsersAsync(UserSearchRequest request)
        {
            request = request ?? new UserSearchRequest();
            var result = new UserSearchResponse();

            var users = new AccountEntity[] { };
            using (var repository = _platformRepository())
            {
                repository.DisableChangesTracking();

                var query = repository.Accounts;

                if (request.Keyword != null)
                {
                    query = query.Where(u => u.UserName.Contains(request.Keyword));
                }

                if (!string.IsNullOrEmpty(request.MemberId))
                {
                    //Find all accounts with specified memberId
                    query = query.Where(u => u.MemberId == request.MemberId);
                }
                else if (!request.MemberIds.IsNullOrEmpty())
                {
                    query = query.Where(u => request.MemberIds.Contains(u.MemberId));
                }

                if (request.AccountTypes != null && request.AccountTypes.Any())
                {
                    query = query.Where(x => request.AccountTypes.Contains(x.UserType));
                }
                result.TotalCount = await query.CountAsync();

                users = await query.OrderBy(x => x.UserName)
                                 .Skip(request.SkipCount)
                                 .Take(request.TakeCount)
                                 .ToArrayAsync();
            }
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

        public virtual async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            using (var userManager = _userManagerFactory())
            {
                return await userManager.GeneratePasswordResetTokenAsync(userId);
            }
        }

        public virtual Permission[] GetAllPermissions()
        {
            return _cacheManager.Get("AllPermissions", SecurityConstants.CacheRegion, LoadAllPermissions);
        }

        public virtual bool UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds)
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

        public virtual Permission[] GetUserPermissions(string userName)
        {
            var user = FindByName(userName, UserDetails.Full);
            var result = user != null ? user.Roles.SelectMany(x => x.Permissions).Distinct().ToArray() : Enumerable.Empty<Permission>().ToArray();
            return result;
        }

        public virtual async Task<bool> IsUserLockedAsync(string userId)
        {
            using (var userManager = _userManagerFactory())
            {
                var result = await userManager.IsLockedOutAsync(userId);
                return result;
            }
        }

        public virtual async Task<SecurityResult> UnlockUserAsync(string userId)
        {
            using (var userManager = _userManagerFactory())
            {
                await userManager.ResetAccessFailedCountAsync(userId);
                var identityResult = await userManager.SetLockoutEndDateAsync(userId, DateTimeOffset.MinValue);
                if (identityResult.Succeeded)
                {
                    var user = await GetApplicationUserByIdAsync(userId);
                    ResetCache(userId, user.UserName);
                }
                var result = identityResult.ToCoreModel();
                return result;
            }
        }

        #endregion

        protected virtual ApplicationUserExtended FindByName(string userName, UserDetails detailsLevel)
        {
            var user = GetApplicationUserByName(userName);
            return GetUserExtended(user, detailsLevel);
        }

        protected virtual Permission[] LoadAllPermissions()
        {
            var manifestPermissions = new List<Permission>();

            foreach (var module in _moduleCatalog.Modules.OfType<ManifestModuleInfo>())
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

        protected virtual SecurityResult ValidateUser(ApplicationUser dbUser)
        {
            var result = new SecurityResult { Succeeded = true };

            if (dbUser == null)
            {
                result = new SecurityResult { Errors = new[] { "User not found." } };
            }

            return result;
        }

        protected virtual async Task<ApplicationUser> GetApplicationUserByIdAsync(string userId)
        {
            var result = await _cacheManager.GetAsync($"GetUserById-{userId}", SecurityConstants.CacheRegion, async () =>
            {
                using (var userManager = _userManagerFactory())
                {
                    return await userManager.FindByIdAsync(userId);
                }
            }, cacheNullValue: false);

            return result;
        }

        protected virtual ApplicationUser GetApplicationUserByName(string userName)
        {
            return Task.Factory.StartNew(() => GetApplicationUserByNameAsync(userName), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }

        protected virtual async Task<ApplicationUser> GetApplicationUserByNameAsync(string userName)
        {
            var result = await _cacheManager.GetAsync($"GetUserByName-{userName}", SecurityConstants.CacheRegion, async () =>
            {
                using (var userManager = _userManagerFactory())
                {
                    return await userManager.FindByNameAsync(userName);
                }
            }, cacheNullValue: false);

            return result;
        }

        protected ApplicationUserExtended GetUserExtended(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            return Task.Factory.StartNew(() => GetUserExtendedAsync(applicationUser, detailsLevel), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }

        protected virtual async Task<ApplicationUserExtended> GetUserExtendedAsync(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            ApplicationUserExtended result = null;
            if (applicationUser != null)
            {
                result = await _cacheManager.GetAsync($"GetUserByName-{applicationUser.UserName}-{detailsLevel}", SecurityConstants.CacheRegion, async () =>
                {
                    ApplicationUserExtended retVal;
                    using (var repository = _platformRepository())
                    {
                        var user = await repository.GetAccountByNameAsync(applicationUser.UserName, detailsLevel);
                        retVal = applicationUser.ToCoreModel(user, _permissionScopeService);
                        //Populate available permission scopes
                        if (retVal.Roles != null)
                        {
                            foreach (var permission in retVal.Roles.SelectMany(x => x.Permissions).Where(x => x != null))
                            {
                                permission.AvailableScopes = _permissionScopeService.GetAvailablePermissionScopes(permission.Id).ToList();
                            }
                        }

                        //Load log entities to account
                        if (detailsLevel.HasFlag(UserDetails.Full) || detailsLevel.HasFlag(UserDetails.Export))
                        {
                            _changeLogService.LoadChangeLogs(retVal);
                        }
                    }
                    var suppressForcingCredentialsChange = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Security:SuppressForcingCredentialsChange", false);
                    if (!suppressForcingCredentialsChange)
                    {
                        //Setting the flags which indicates a necessity of security credentials change
                        retVal.PasswordExpired = retVal.PasswordHash == Resources.Default.DefaultPasswordHash;
                        if (retVal.ApiAccounts != null)
                        {
                            foreach (var apiAccount in retVal.ApiAccounts)
                            {
                                apiAccount.SecretKeyExpired = apiAccount.SecretKey == Resources.Default.DefaultSecretKey;
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



        protected virtual void ResetCache(string userId, string userName)
        {
            _cacheManager.Remove($"GetUserById-{userId}", SecurityConstants.CacheRegion);
            _cacheManager.Remove($"GetUserByName-{userName}", SecurityConstants.CacheRegion);
            //For normalized user name
            _cacheManager.Remove($"GetUserByName-{userName.Normalize().ToUpperInvariant()}", SecurityConstants.CacheRegion);
            foreach (var detailLevel in Enum.GetNames(typeof(UserDetails)))
            {
                _cacheManager.Remove($"GetUserByName-{userName}-{detailLevel}", SecurityConstants.CacheRegion);
                //For normalized user name
                _cacheManager.Remove($"GetUserByName-{userName.Normalize().ToUpperInvariant()}-{detailLevel}", SecurityConstants.CacheRegion);
            }
        }

        protected virtual void NormalizeUser(ApplicationUserExtended user)
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
