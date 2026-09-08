#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Jobs;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Jobs;

/// <summary>
/// Exercises the recurring-schedule builder through its public surface: what a schedule declares must reach the
/// <see cref="EnqueueOptions"/> the registration passes to <see cref="IBackgroundJob"/> on each occurrence.
/// </summary>
public class RecurringJobScheduleBuilderTests
{
    private class TestPayload
    {
    }

    private sealed class TestHandler : IBackgroundJobHandler<TestPayload>
    {
        public Task Execute(TestPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }

    /// <summary>Records the options a triggered occurrence enqueues with.</summary>
    private sealed class RecordingBackgroundJob : IBackgroundJob
    {
        public EnqueueOptions? Options { get; private set; }

        public bool Enqueued { get; private set; }

        public Task<string> Enqueue<THandler>(object payload, EnqueueOptions? options = null,
            CancellationToken cancellationToken = default)
            where THandler : class
        {
            Options = options;
            Enqueued = true;
            return Task.FromResult("job-1");
        }
    }

    private static async Task<RecordingBackgroundJob> TriggerOnce(Action<IRecurringJobScheduleBuilder> configure)
    {
        var services = new ServiceCollection();
        services.AddRecurringJob<TestHandler, TestPayload>(configure);

        using var provider = services.BuildServiceProvider();
        var registration = provider.GetServices<RecurringJobRegistration>().Single();

        var jobs = new RecordingBackgroundJob();
        await registration.Trigger(jobs, TestContext.Current.CancellationToken);

        return jobs;
    }

    [Fact]
    public async Task WithMaxRetryAttempts_Reaches_The_Enqueue_Options()
    {
        var jobs = await TriggerOnce(schedule => schedule
            .WithId("test-job")
            .WithCron("0 * * * *")
            .WithMaxRetryAttempts(0));

        Assert.True(jobs.Enqueued);
        Assert.Equal(0, jobs.Options?.MaxRetryAttempts);
    }

    [Fact]
    public async Task WithMaxRetryAttempts_Combines_With_WithQueue()
    {
        var jobs = await TriggerOnce(schedule => schedule
            .WithId("test-job")
            .WithCron("0 * * * *")
            .WithQueue("low")
            .WithMaxRetryAttempts(2));

        Assert.Equal("low", jobs.Options?.Queue);
        Assert.Equal(2, jobs.Options?.MaxRetryAttempts);
    }

    [Fact]
    public async Task Schedule_Without_Queue_Or_Retries_Enqueues_With_No_Options()
    {
        // Null, not an empty options object: the engine must apply its own defaults for both.
        var jobs = await TriggerOnce(schedule => schedule
            .WithId("test-job")
            .WithCron("0 * * * *"));

        Assert.True(jobs.Enqueued);
        Assert.Null(jobs.Options);
    }

    [Fact]
    public void WithMaxRetryAttempts_Rejects_A_Negative_Count()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            services.AddRecurringJob<TestHandler, TestPayload>(schedule => schedule
                .WithId("test-job")
                .WithCron("0 * * * *")
                .WithMaxRetryAttempts(-1)));
    }
}
