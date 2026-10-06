using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Web.Security.SignInLog
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

        public Task Process() => Process(CancellationToken.None);

        public async Task Process(CancellationToken cancellationToken)
        {
            var retentionDays = await _settingsManager.GetValueAsync<int>(
                PlatformConstants.Settings.Security.SignInLogRetentionDays);

            // Zero or less means keep forever. Reading an unset or cleared value as
            // "delete everything" would destroy the audit trail it exists to protect.
            if (retentionDays <= 0)
            {
                return;
            }

            var cutoff = DateTime.UtcNow.AddDays(-retentionDays);

            int deleted;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                deleted = await _service.DeleteOlderThan(cutoff, BatchSize, cancellationToken);
            }
            while (deleted == BatchSize);
        }

        public Task Execute(SignInLogCleanupJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
            => Process(cancellationToken);
    }
}
