using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Hangfire;
using VirtoCommerce.Platform.Hangfire.Extensions;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    public class BackgroundJobsRunner
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IRecurringJobManager _recurringJobManager;

        public BackgroundJobsRunner(ISettingsManager settingsManager, IRecurringJobManager recurringJobManager)
        {
            _settingsManager = settingsManager;
            _recurringJobManager = recurringJobManager;
        }

        public async Task PruneExpiredTokensJob()
        {
            await RecurringJobExtensions.WatchJobSettingAsync(_recurringJobManager,
            _settingsManager,
            new SettingCronJobBuilder()
                .SetEnablerSetting(PlatformConstants.Settings.Security.EnablePruneExpiredTokensJob)
                .SetCronSetting(PlatformConstants.Settings.Security.CronPruneExpiredTokensJob)
                .ToJob<PruneExpiredTokensJob>(x => x.Process())
                .Build());
        }

    }
}
