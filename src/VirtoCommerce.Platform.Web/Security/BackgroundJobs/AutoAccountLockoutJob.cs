using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    public class AutoAccountLockoutJob
    {
        private readonly IUserSearchService _userSearchService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LockoutOptionsExtended _lockoutOptions;

        public AutoAccountLockoutJob(
            IUserSearchService userSearchService,
            SignInManager<ApplicationUser> signInManager,
            IOptions<LockoutOptionsExtended> lockoutOptions)
        {
            _userSearchService = userSearchService;
            _signInManager = signInManager;
            _lockoutOptions = lockoutOptions.Value;
        }

        public async Task Process()
        {
            var lockoutMaximumDaysFromLastLogin = _lockoutOptions.LockoutMaximumDaysFromLastLogin;
            var criteria = new UserSearchCriteria { OnlyUnlocked = true, LasLoginDate = DateTime.UtcNow.AddDays(-lockoutMaximumDaysFromLastLogin) };
            var usersResult = await _userSearchService.SearchUsersAsync(criteria);

            foreach (var user in usersResult.Results)
            {
                await _signInManager.UserManager.SetLockoutEndDateAsync(user, DateTime.MaxValue.ToUniversalTime());
            }
        }
    }
}
