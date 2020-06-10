using System;

namespace VirtoCommerce.Platform.Core.Settings
{
    public class SettingCronJob
    {
        public string RecurringJobId { get; set; }
        public SettingDescriptor EnableSetting { get; set; }
        public SettingDescriptor CronSetting { get; set; }
        public Hangfire.Common.Job Job { get; set; }
        public string Queue { get; set; } = Hangfire.States.EnqueuedState.DefaultQueue;
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;
    }
}
