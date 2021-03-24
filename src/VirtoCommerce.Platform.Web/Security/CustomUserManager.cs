using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Security.Caching;

namespace VirtoCommerce.Platform.Web.Security
{
    public class CustomUserManager : AspNetUserManager<ApplicationUser>
    {
        private readonly IPlatformMemoryCache _memoryCache;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserPasswordHasher _userPasswordHasher;

        public CustomUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IUserPasswordHasher userPasswordHasher,
                                 IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
                                 ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services,
                                 ILogger<UserManager<ApplicationUser>> logger, RoleManager<Role> roleManager, IPlatformMemoryCache memoryCache, IEventPublisher eventPublisher)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _memoryCache = memoryCache;
            _roleManager = roleManager;
            _eventPublisher = eventPublisher;
            _userPasswordHasher = userPasswordHasher;
        }

        public override async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(FindByLoginAsync), loginProvider, providerKey);
            var result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var user = await base.FindByLoginAsync(loginProvider, providerKey);
                if (user != null)
                {
                    await LoadUserDetailsAsync(user);
                    cacheEntry.AddExpirationToken(SecurityCacheRegion.CreateChangeTokenForUser(user));
                }
                return user;
            }, cacheNullValue: false);

            return result;
        }

        public override async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(FindByEmailAsync), email);
            var result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var user = await base.FindByEmailAsync(email);
                if (user != null)
                {
                    await LoadUserDetailsAsync(user);
                    cacheEntry.AddExpirationToken(SecurityCacheRegion.CreateChangeTokenForUser(user));
                }
                return user;
            }, cacheNullValue: false);
            return result;
        }

        public override async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(FindByNameAsync), userName);
            var result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var user = await base.FindByNameAsync(userName);
                if (user != null)
                {
                    await LoadUserDetailsAsync(user);
                    cacheEntry.AddExpirationToken(SecurityCacheRegion.CreateChangeTokenForUser(user));
                }
                return user;
            }, cacheNullValue: false);
            return result;
        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(FindByIdAsync), userId);
            var result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var user = await base.FindByIdAsync(userId);
                if (user != null)
                {
                    await LoadUserDetailsAsync(user);
                    cacheEntry.AddExpirationToken(SecurityCacheRegion.CreateChangeTokenForUser(user));
                }
                return user;
            }, cacheNullValue: false);
            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            //It is important to call base.FindByIdAsync method to avoid of update a cached user.
            var existUser = await base.FindByIdAsync(user.Id);
            existUser.LastPasswordChangedDate = DateTime.UtcNow;

            var result = await base.ResetPasswordAsync(existUser, token, newPassword);
            if (result == IdentityResult.Success)
            {
                SecurityCacheRegion.ExpireUser(user);
                // Calculate password hash for external hash storage. This provided as workaround until password hash storage would implemented
                var customPasswordHash = _userPasswordHasher.HashPassword(user, newPassword);
                await _eventPublisher.Publish(new UserResetPasswordEvent(user.Id, customPasswordHash));
            }

            return result;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            user.LastPasswordChangedDate = DateTime.UtcNow;

            var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result == IdentityResult.Success)
            {
                SecurityCacheRegion.ExpireUser(user);
                // Calculate password hash for external hash storage. This provided as workaround until password hash storage would implemented
                var customPasswordHash = _userPasswordHasher.HashPassword(user, newPassword);
                await _eventPublisher.Publish(new UserPasswordChangedEvent(user.Id, customPasswordHash));
            }

            return result;
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            var changedEntries = new List<GenericChangedEntry<ApplicationUser>>
            {
                new GenericChangedEntry<ApplicationUser>(user, EntryState.Deleted)
            };
            await _eventPublisher.Publish(new UserChangingEvent(changedEntries));
            var result = await base.DeleteAsync(user);
            if (result.Succeeded)
            {
                SecurityCacheRegion.ExpireUser(user);
                await _eventPublisher.Publish(new UserChangedEvent(changedEntries));
            }
            return result;
        }

        protected override async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            var existentUser = await LoadExistingUser(user);

            //We cant update not existing user
            if (existentUser == null)
            {
                return IdentityResult.Failed(ErrorDescriber.DefaultError());
            }

            var changedEntries = new List<GenericChangedEntry<ApplicationUser>>
            {
                new GenericChangedEntry<ApplicationUser>(user, (ApplicationUser)existentUser.Clone(), EntryState.Modified)
            };

            await _eventPublisher.Publish(new UserChangingEvent(changedEntries));

            //We need to use Patch method to update already tracked by DbContent entity, unless the UpdateAsync for passed user will throw exception
            //"The instance of entity type 'ApplicationUser' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached"
            user.Patch(existentUser);

            var result = await base.UpdateUserAsync(existentUser);

            if (result.Succeeded)
            {
                SecurityCacheRegion.ExpireUser(existentUser);
                var events = changedEntries.GenerateSecurityEventsByChanges();
                var tasks = events.Select(x => _eventPublisher.Publish(x));
                await Task.WhenAll(tasks);
            }

            return result;
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            var result = await base.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (user.Roles != null)
                {
                    var targetRoles = await GetRolesAsync(user);
                    var sourceRoles = user.Roles.Select(x => x.Name);

                    //Add
                    foreach (var newRole in sourceRoles.Except(targetRoles))
                    {
                        await AddToRoleAsync(user, newRole);
                    }

                    //Remove
                    foreach (var removeRole in targetRoles.Except(sourceRoles))
                    {
                        await RemoveFromRoleAsync(user, removeRole);
                    }
                }

                if (user.Logins != null)
                {
                    var targetLogins = await GetLoginsAsync(user);
                    var sourceLogins = user.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, null));

                    foreach (var item in sourceLogins.Where(x => targetLogins.All(y => x.LoginProvider + x.ProviderKey != y.LoginProvider + y.ProviderKey)))
                    {
                        await AddLoginAsync(user, item);
                    }

                    foreach (var item in targetLogins.Where(x => sourceLogins.All(y => x.LoginProvider + x.ProviderKey != y.LoginProvider + y.ProviderKey)))
                    {
                        await RemoveLoginAsync(user, item.LoginProvider, item.ProviderKey);
                    }
                }
            }

            return result;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var changedEntries = new List<GenericChangedEntry<ApplicationUser>>
            {
                new GenericChangedEntry<ApplicationUser>(user, EntryState.Added)
            };
            await _eventPublisher.Publish(new UserChangingEvent(changedEntries));
            var result = await base.CreateAsync(user);
            if (result.Succeeded)
            {
                if (!user.Roles.IsNullOrEmpty())
                {
                    //Add
                    foreach (var newRole in user.Roles)
                    {
                        await AddToRoleAsync(user, newRole.Name);
                    }
                }

                // add external logins
                if (!user.Logins.IsNullOrEmpty())
                {
                    foreach (var login in user.Logins)
                    {
                        await AddLoginAsync(user, new UserLoginInfo(login.LoginProvider, login.ProviderKey, null));
                    }
                }

                SecurityCacheRegion.ExpireUser(user);
                await _eventPublisher.Publish(new UserChangedEvent(changedEntries));
            }
            return result;
        }

        public override async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            var result = await base.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                await _eventPublisher.Publish(new UserRoleAddedEvent(user, role));
            }
            return result;
        }

        public override async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            var result = await base.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                await _eventPublisher.Publish(new UserRoleRemovedEvent(user, role));
            }
            return result;
        }


        /// <summary>
        /// Load detailed user information: Roles, external logins, claims (permissions)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task LoadUserDetailsAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.Roles = new List<Role>();
            foreach (var roleName in await base.GetRolesAsync(user))
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    user.Roles.Add(role);
                }
            }

            // Read claims and convert to permissions (compatibility with v2)
            user.Permissions = user.Roles.SelectMany(x => x.Permissions).Select(x => x.Name).Distinct().ToArray();

            // Read associated logins
            var logins = await base.GetLoginsAsync(user);
            user.Logins = logins.Select(x => new ApplicationUserLogin() { LoginProvider = x.LoginProvider, ProviderKey = x.ProviderKey }).ToArray();
        }

        /// <summary>
        /// Finds existing user and loads its details
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns null, if no user found, otherwise user with details.</returns>
        protected virtual async Task<ApplicationUser> LoadExistingUser(ApplicationUser user)
        {
            ApplicationUser result = null;

            if (!string.IsNullOrEmpty(user.Id))
            {
                //It is important to call base.FindByIdAsync method to avoid of update a cached user.
                result = await base.FindByIdAsync(user.Id);
            }
            if (result == null)
            {
                //It is important to call base.FindByNameAsync method to avoid of update a cached user.
                result = await base.FindByNameAsync(user.UserName);
            }

            if (result != null)
            {
                await LoadUserDetailsAsync(result);
            }

            return result;
        }
    }
}
