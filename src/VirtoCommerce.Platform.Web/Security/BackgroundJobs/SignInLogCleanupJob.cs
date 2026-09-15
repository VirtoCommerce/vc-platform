using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    /// <summary>
    /// Trims the sign-in audit log to the configured retention window. Not optional: a store at
    /// 50k logins a day produces roughly 18M rows a year.
    /// </summary>
    public class SignInLogCleanupJob : IBackgroundJobHandler<SignInLogCleanupJobPayload>
    {
        private const int BatchSize = 1000;

        private readonly IUserSignInLogService _service;
        private readonly ISettingsManager _settingsManager;

        public SignInLogCleanupJob(IUserSignInLogService service, ISettingsManager settingsManager)
        {
            _service = service;
            _settingsManager = settingsManager;
        }

        public async Task Process()
        {
            var retentionDays = await _settingsManager.GetValueAsync<int>(
                PlatformConstants.Settings.Security.SignInLogRetentionDays);

            // A misconfigured 0 must not be read as "delete everything".
            if (retentionDays <= 0)
            {
                return;
            }

            var cutoff = DateTime.UtcNow.AddDays(-retentionDays);

            int deleted;
            do
            {
                deleted = await _service.DeleteOlderThanAsync(cutoff, BatchSize);
            }
            while (deleted == BatchSize);
        }

        public Task Execute(SignInLogCleanupJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
            => Process();
    }
}
