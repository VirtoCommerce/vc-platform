using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    public class AutoAccountLockoutJob(
        IUserSearchService userSearchService,
        UserManager<ApplicationUser> userManager,
        IOptions<LockoutOptionsExtended> lockoutOptions,
        ILogger<AutoAccountLockoutJob> logger)
    {
        private const int DefaultPageSize = 100;

        private readonly IUserSearchService _userSearchService = userSearchService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly LockoutOptionsExtended _lockoutOptions = lockoutOptions.Value;
        private readonly ILogger<AutoAccountLockoutJob> _logger = logger;

        [DisableConcurrentExecution(10)]
        public async Task Process(CancellationToken cancellationToken)
        {
            var lastLoginCutoff = DateTime.UtcNow.AddDays(-_lockoutOptions.LockoutMaximumDaysFromLastLogin);
            var batchSize = _lockoutOptions.AutoAccountsLockoutJobBatchSize;
            var unlimited = batchSize <= 0;
            var pageSize = unlimited ? DefaultPageSize : batchSize;

            var matched = 0;
            var locked = 0;
            var failed = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var users = await FetchPageAsync(lastLoginCutoff, skip: failed, take: pageSize);

                if (users.Count == 0)
                {
                    break;
                }

                foreach (var user in users)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    matched++;

                    if (await TryLockUserAsync(user))
                    {
                        locked++;
                    }
                    else
                    {
                        failed++;
                    }
                }

                if (!unlimited)
                {
                    break;
                }
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "AutoAccountLockoutJob completed: matched={Matched}, locked={Locked}, failed={Failed}",
                    matched, locked, failed);
            }
        }

        private async Task<IList<ApplicationUser>> FetchPageAsync(DateTime lastLoginCutoff, int skip, int take)
        {
            var criteria = new UserSearchCriteria
            {
                OnlyUnlocked = true,
                LoginEndDate = lastLoginCutoff,
                Skip = skip,
                Take = take,
            };

            var result = await _userSearchService.SearchUsersAsync(criteria);

            return result.Results;
        }

        private async Task<bool> TryLockUserAsync(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue.ToUniversalTime());

                if (result.Succeeded)
                {
                    return true;
                }

                var errors = result.Errors;

                _logger.LogWarning(
                    "AutoAccountLockoutJob: failed to lock user {UserId} {UserName}: {@Errors}",
                    user.Id, user.UserName, errors);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "AutoAccountLockoutJob: exception locking user {UserId} {UserName}",
                    user.Id, user.UserName);

                return false;
            }
        }
    }
}
