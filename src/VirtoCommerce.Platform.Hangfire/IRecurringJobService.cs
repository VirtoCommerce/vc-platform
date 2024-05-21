using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Hangfire;

public interface IRecurringJobService
{
    void WatchJobSetting<T>(
        SettingDescriptor enablerSetting,
        SettingDescriptor cronSetting,
        Expression<Func<T, Task>> methodCall,
        string jobId,
        TimeZoneInfo timeZoneInfo,
        string queue);

    void WatchJobSetting(SettingCronJob settingCronJob);

    Task WatchJobSettingAsync(SettingCronJob settingCronJob);
}
