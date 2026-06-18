#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>Default <see cref="IRecurringJobScheduleBuilder"/>; builds a <see cref="RecurringJobRegistration"/>.</summary>
internal sealed class RecurringJobScheduleBuilder : IRecurringJobScheduleBuilder
{
    private string? _id;
    private string? _cronExpression;
    private SettingDescriptor? _enablerSetting;
    private SettingDescriptor? _cronSetting;
    private string? _queue;
    private TimeZoneInfo _timeZone = TimeZoneInfo.Utc;

    public IRecurringJobScheduleBuilder WithId(string recurringJobId)
    {
        _id = recurringJobId;
        return this;
    }

    public IRecurringJobScheduleBuilder WithCron(string cronExpression)
    {
        _cronExpression = cronExpression;
        return this;
    }

    public IRecurringJobScheduleBuilder FromSettings(SettingDescriptor enablerSetting, SettingDescriptor cronSetting)
    {
        _enablerSetting = enablerSetting;
        _cronSetting = cronSetting;
        return this;
    }

    public IRecurringJobScheduleBuilder WithQueue(string queue)
    {
        _queue = queue;
        return this;
    }

    public IRecurringJobScheduleBuilder WithTimeZone(TimeZoneInfo timeZone)
    {
        _timeZone = timeZone ?? TimeZoneInfo.Utc;
        return this;
    }

    /// <summary>
    /// Builds the registration. <paramref name="enqueue"/> enqueues the (typed) payload — supplied by
    /// <see cref="BackgroundJobsServiceCollectionExtensions.AddRecurringJob{TPayload, THandler}"/> — and is wrapped
    /// with the configured queue.
    /// </summary>
    internal RecurringJobRegistration Build(Func<IBackgroundJob, EnqueueOptions?, CancellationToken, Task> enqueue)
    {
        if (string.IsNullOrEmpty(_id))
        {
            throw new InvalidOperationException("A recurring job requires an id — call WithId(...).");
        }

        if (string.IsNullOrEmpty(_cronExpression) && _cronSetting is null)
        {
            throw new InvalidOperationException(
                $"Recurring job '{_id}' requires a schedule — call WithCron(...) or FromSettings(...).");
        }

        var options = _queue is null ? null : new EnqueueOptions { Queue = _queue };

        return new RecurringJobRegistration
        {
            Id = _id,
            CronExpression = _cronExpression,
            EnablerSetting = _enablerSetting,
            CronSetting = _cronSetting,
            TimeZone = _timeZone,
            Trigger = (jobs, cancellationToken) => enqueue(jobs, options, cancellationToken),
        };
    }
}
