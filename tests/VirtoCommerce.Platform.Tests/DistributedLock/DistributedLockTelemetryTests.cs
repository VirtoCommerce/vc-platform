using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

public class DistributedLockTelemetryTests
{
    private static CancellationToken Token => TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("cart:recalc:42", "cart:recalc")]
    [InlineData("loyalty-balance:user-1", "loyalty-balance")]
    [InlineData("catalog:reindex", "catalog")]
    [InlineData("a:b:c:d:42", "a:b")]
    [InlineData("order-42", DistributedLockBase.OtherResourceFamily)]
    [InlineData(":42", DistributedLockBase.OtherResourceFamily)]
    public void GetResourceFamily_KeepsAtMostTwoLeadingSegments(string resource, string expected)
    {
        FamilyProbe.Family(resource).Should().Be(expected);
    }

    [Fact]
    public async Task Metrics_RecordAcquisitionsWaitHoldAndLossByFamily()
    {
        var family = $"lock-telemetry:{Guid.NewGuid():N}";
        using var recorder = new MetricRecorder(family);
        using var lost = new CancellationTokenSource();
        var acquired = new ScriptedLock(resource => new Handle(resource, lost.Token));
        var busy = new ScriptedLock(_ => null);
        var unavailable = new ScriptedLock(resource => throw new DistributedLockUnavailableException(resource, "test"));

        var handle = await acquired.TryAcquireAsync($"{family}:1", cancellationToken: Token);
        await lost.CancelAsync();
        await handle.DisposeAsync();
        (await busy.TryAcquireAsync($"{family}:2", cancellationToken: Token)).Should().BeNull();
        await FluentActions.Awaiting(() => unavailable.TryAcquireAsync($"{family}:3", cancellationToken: Token))
            .Should().ThrowAsync<DistributedLockUnavailableException>();

        recorder.Values("vc.lock.acquisitions").Select(x => x.Outcome).Should().BeEquivalentTo(
            [DistributedLockBase.OutcomeAcquired, DistributedLockBase.OutcomeTimeout, DistributedLockBase.OutcomeUnavailable]);
        recorder.Values("vc.lock.wait.duration").Should().HaveCount(3).And.OnlyContain(x => x.Value >= 0);
        recorder.Values("vc.lock.hold.duration").Should().ContainSingle().Which.Outcome.Should().BeNull("hold time is tagged by family only");
        recorder.Values("vc.lock.lost").Should().ContainSingle().Which.Value.Should().Be(1);
    }

    [Fact]
    public void DurationHistograms_UseSecondsBuckets()
    {
        var histograms = new ConcurrentDictionary<string, Histogram<double>>();
        using var listener = new MeterListener
        {
            InstrumentPublished = (instrument, _) =>
            {
                if (instrument.Meter.Name == DistributedLockBase.ActivitySourceName && instrument is Histogram<double> histogram)
                {
                    histograms[instrument.Name] = histogram;
                }
            },
        };
        listener.Start();

        histograms.Keys.Should().BeEquivalentTo(["vc.lock.wait.duration", "vc.lock.hold.duration"]);
        histograms.Values.Should().OnlyContain(x => x.Unit == "s"
            && x.Advice != null
            && x.Advice.HistogramBucketBoundaries.First() == 0.005
            && x.Advice.HistogramBucketBoundaries.Last() == 300);
    }

    [Fact]
    public void OptionsValidator_AcceptsDefaults()
    {
        new DistributedLockOptionsValidator().Validate(null, new DistributedLockOptions()).Succeeded.Should().BeTrue();
    }

    [Theory]
    [InlineData(nameof(DistributedLockOptions.RetryInterval), "00:00:00")]
    [InlineData(nameof(DistributedLockOptions.RetryInterval), "-00:00:01")]
    [InlineData(nameof(DistributedLockOptions.MaxRetryInterval), "00:00:00.05")]
    [InlineData(nameof(DistributedLockOptions.Expiry), "00:00:00")]
    [InlineData(nameof(DistributedLockOptions.StartupExpiry), "00:00:00")]
    [InlineData(nameof(DistributedLockOptions.DefaultTimeout), "-00:00:02")]
    public void OptionsValidator_RejectsValuesThatBreakLocking(string option, string value)
    {
        var options = new DistributedLockOptions();
        typeof(DistributedLockOptions).GetProperty(option)!.SetValue(options, TimeSpan.Parse(value));

        var result = new DistributedLockOptionsValidator().Validate(null, options);

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain(option);
    }

    [Fact]
    public void OptionsValidator_AcceptsInfiniteDefaultTimeoutAndRejectsNegativeWaitTime()
    {
        var validator = new DistributedLockOptionsValidator();

        validator.Validate(null, new DistributedLockOptions { DefaultTimeout = Timeout.InfiniteTimeSpan }).Succeeded.Should().BeTrue();
        validator.Validate(null, new DistributedLockOptions { WaitTime = -1 }).Failed.Should().BeTrue();
    }

    private sealed class FamilyProbe : DistributedLockBase
    {
        private FamilyProbe()
            : base(Options.Create(new DistributedLockOptions()))
        {
        }

        public static string Family(string resource)
        {
            return GetResourceFamily(resource);
        }

        protected override Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class ScriptedLock(Func<string, IDistributedLockHandle> acquire) : DistributedLockBase(Options.Create(new DistributedLockOptions()))
    {
        protected override Task<IDistributedLockHandle> TryAcquireCoreAsync(string resource, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return Task.FromResult(acquire(resource));
        }
    }

    private sealed class Handle(string resource, CancellationToken lostToken) : IDistributedLockHandle
    {
        public string Resource { get; } = resource;

        public CancellationToken HandleLostToken { get; } = lostToken;

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }

    // Records lock measurements for one resource family, so tests running in parallel do not interfere.
    private sealed class MetricRecorder : IDisposable
    {
        private readonly string _family;
        private readonly MeterListener _listener = new();
        private readonly ConcurrentQueue<(string Instrument, double Value, string Outcome)> _measurements = new();

        public MetricRecorder(string family)
        {
            _family = family;
            _listener.InstrumentPublished = (instrument, listener) =>
            {
                if (instrument.Meter.Name == DistributedLockBase.ActivitySourceName)
                {
                    listener.EnableMeasurementEvents(instrument);
                }
            };
            _listener.SetMeasurementEventCallback<double>((instrument, value, tags, _) => Record(instrument, value, tags));
            _listener.SetMeasurementEventCallback<long>((instrument, value, tags, _) => Record(instrument, value, tags));
            _listener.Start();
        }

        public IReadOnlyList<(double Value, string Outcome)> Values(string instrument)
        {
            return _measurements.Where(x => x.Instrument == instrument).Select(x => (x.Value, x.Outcome)).ToList();
        }

        public void Dispose()
        {
            _listener.Dispose();
        }

        private void Record(Instrument instrument, double value, ReadOnlySpan<KeyValuePair<string, object>> tags)
        {
            string family = null;
            string outcome = null;
            foreach (var tag in tags)
            {
                if (tag.Key == "vc.lock.resource_family")
                {
                    family = (string)tag.Value;
                }
                else if (tag.Key == "vc.lock.outcome")
                {
                    outcome = (string)tag.Value;
                }
            }

            if (family == _family)
            {
                _measurements.Enqueue((instrument.Name, value, outcome));
            }
        }
    }
}
