using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    public class BackgroundJobsRunner
    {
        private readonly ISettingsManager _settingsManager;

        public BackgroundJobsRunner(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task ConfigureProcessSubscriptionJob()
        {
            var processJobEnable = await _settingsManager.GetValueAsync(PlatformConstants.Settings.Security.EnablePruneExpiredTokensJob.Name, true);
            if (processJobEnable)
            {
                var cronExpression = _settingsManager.GetValue(PlatformConstants.Settings.Security.CronPruneExpiredTokensJob.Name, PlatformConstants.Settings.Security.CronPruneExpiredTokensJob.DefaultValue.ToString());
                RecurringJob.AddOrUpdate<PruneExpiredTokensJob>("PruneExpiredTokensJob", x => x.Process(), cronExpression);
            }
            else
            {
                RecurringJob.RemoveIfExists("PruneExpiredTokensJob");
            }
        }

    }
}
