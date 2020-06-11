using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Hangfire
{
    public class SettingCronJobBuilder
    {
        protected SettingCronJob _settingCronJob;

        public SettingCronJobBuilder()
        {
            _settingCronJob = new SettingCronJob();
        }

        public SettingCronJobBuilder(SettingCronJob settingCronJob)
        {
            _settingCronJob = settingCronJob;
        }

        public SettingCronJobBuilder SetJobId(string jobId)
        {
            _settingCronJob.RecurringJobId = jobId;
            return this;
        }

        public SettingCronJobBuilder SetEnablerSetting(SettingDescriptor settingDescriptor)
        {
            _settingCronJob.EnableSetting = settingDescriptor;
            return this;
        }

        public SettingCronJobBuilder SetCronSetting(SettingDescriptor settingDescriptor)
        {
            _settingCronJob.CronSetting = settingDescriptor;
            return this;
        }

        public SettingCronJobBuilder SetQueueName(string queue)
        {
            _settingCronJob.Queue = queue;
            return this;
        }

        public SettingCronJobBuilder SetTimeZoneInfo(TimeZoneInfo timeZoneInfo)
        {
            _settingCronJob.TimeZone = timeZoneInfo;
            return this;
        }

        public SettingCronJobBuilder ToJob<T>(Expression<Func<T, Task>> methodCall)
        {
            _settingCronJob.Job = Job.FromExpression(methodCall);
            _settingCronJob.RecurringJobId ??= $"{_settingCronJob.Job.Type.Name}.{_settingCronJob.Job.Method.Name}";
            return this;
        }

        public SettingCronJob Build()
        {
            return _settingCronJob;
        }
    }
}
