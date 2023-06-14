using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    public class AutoAccountLockoutJob
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IUserSearchService _userSearchService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AutoAccountLockoutJob(ISettingsManager settingsManager, IUserSearchService userSearchService, SignInManager<ApplicationUser> signInManager)
        {
            _settingsManager = settingsManager;
            _userSearchService = userSearchService;
            _signInManager = signInManager;
        }

        public async Task Process()
        {
            var lockoutMaximumDaysFromLastLogin = await _settingsManager.GetValueByDescriptorAsync<int>(PlatformConstants.Settings.Security.LockoutMaximumDaysFromLastLogin);
            var criteria = new UserSearchCriteria { OnlyUnlocked = true, LasLoginDate = DateTime.UtcNow.AddDays(-lockoutMaximumDaysFromLastLogin) };
            var usersResult = await _userSearchService.SearchUsersAsync(criteria);

            foreach (var user in usersResult.Results)
            {
                await _signInManager.UserManager.SetLockoutEndDateAsync(user, DateTime.MaxValue.ToUniversalTime());
            }
        }
    }
}
