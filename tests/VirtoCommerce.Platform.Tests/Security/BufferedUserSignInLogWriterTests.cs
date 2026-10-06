using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Services;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.SignInLog;

namespace VirtoCommerce.Platform.Tests.Security;

public class BufferedUserSignInLogWriterTests
{
    [Fact]
    public async Task Write_ThenFlush_PersistsEveryQueuedRecord()
    {
        var saved = new ConcurrentBag<UserSignInLog>();
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChanges(It.IsAny<IList<UserSignInLog>>(), It.IsAny<CancellationToken>()))
            .Callback<IList<UserSignInLog>, CancellationToken>((batch, _) =>
            {
                foreach (var record in batch)
                {
                    saved.Add(record);
                }
            })
            .Returns(Task.CompletedTask);

        var writer = CreateWriter(service.Object, capacity: 100, batchSize: 10);

        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "b", SignInType = SignInType.Password });

        await writer.Flush(CancellationToken.None);

        saved.Select(x => x.UserName).Should().BeEquivalentTo("a", "b");
    }

    [Fact]
    public void Write_AssignsIdAndCreatedDateWhenMissing()
    {
        var writer = CreateWriter(Mock.Of<IUserSignInLogService>(), capacity: 10, batchSize: 10);

        var record = new UserSignInLog { UserName = "a", SignInType = SignInType.Password };

        writer.Write(record);

        record.Id.Should().NotBeNullOrEmpty();
        record.CreatedDate.Should().NotBe(default);
    }

    [Fact]
    public void Write_WhenBufferIsFull_CountsTheDropExactly()
    {
        var writer = CreateWriter(Mock.Of<IUserSignInLogService>(), capacity: 2, batchSize: 10);

        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "b", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "c", SignInType = SignInType.Password });

        // FullMode.Wait is the only mode where TryWrite reports the rejection to the caller, so the
        // count is exact rather than inferred from queue depth - a silent gap in an audit trail is
        // worse than a visible one.
        writer.DroppedCount.Should().Be(1);
    }

    [Fact]
    public async Task Flush_ConcurrentCalls_AreSerialized()
    {
        var entered = new SemaphoreSlim(0);
        var release = new TaskCompletionSource();
        var inFlight = 0;
        var overlapped = false;

        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChanges(It.IsAny<IList<UserSignInLog>>(), It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                if (Interlocked.Increment(ref inFlight) > 1)
                {
                    overlapped = true;
                }

                entered.Release();
                await release.Task;
                Interlocked.Decrement(ref inFlight);
            });

        var writer = CreateWriter(service.Object, capacity: 100, batchSize: 1);
        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "b", SignInType = SignInType.Password });

        var first = writer.Flush(CancellationToken.None);
        await entered.WaitAsync(TestContext.Current.CancellationToken);

        // Flush is public so a host can drain on demand, and the timer loop calls it too. The channel
        // is SingleReader, so a second caller has to wait rather than read alongside the first.
        var second = writer.Flush(CancellationToken.None);
        second.IsCompleted.Should().BeFalse();

        release.SetResult();
        await Task.WhenAll(first, second);

        overlapped.Should().BeFalse();
    }

    [Fact]
    public async Task Flush_EnrichesEachRecordBeforePersisting()
    {
        var saved = new List<UserSignInLog>();
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChanges(It.IsAny<IList<UserSignInLog>>(), It.IsAny<CancellationToken>()))
            .Callback<IList<UserSignInLog>, CancellationToken>((batch, _) =>
                saved.AddRange(batch.Select(x => new UserSignInLog { UserName = x.UserName, OrganizationName = x.OrganizationName })))
            .Returns(Task.CompletedTask);

        var enricher = new Mock<IUserSignInLogEnricher>();
        enricher.SetupGet(x => x.Priority).Returns(0);
        enricher.Setup(x => x.Enrich(It.IsAny<UserSignInLog>()))
            .Callback<UserSignInLog>(record => record.OrganizationName = "Acme Inc")
            .Returns(Task.CompletedTask);

        var writer = CreateWriter(service.Object, capacity: 100, batchSize: 10, enrichers: [enricher.Object]);
        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });
        writer.Write(new UserSignInLog { UserName = "b", SignInType = SignInType.Password });

        await writer.Flush(CancellationToken.None);

        // Enrichment runs here rather than in the event handler: a module's lookup must not sit on the
        // sign-in request thread, where it would reopen the user-enumeration timing gap.
        saved.Should().OnlyContain(x => x.OrganizationName == "Acme Inc");
    }

    [Fact]
    public async Task Flush_WhenEnricherThrows_StillPersistsTheRecord()
    {
        var saved = new ConcurrentBag<UserSignInLog>();
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChanges(It.IsAny<IList<UserSignInLog>>(), It.IsAny<CancellationToken>()))
            .Callback<IList<UserSignInLog>, CancellationToken>((batch, _) =>
            {
                foreach (var record in batch)
                {
                    saved.Add(record);
                }
            })
            .Returns(Task.CompletedTask);

        var enricher = new Mock<IUserSignInLogEnricher>();
        enricher.SetupGet(x => x.Priority).Returns(0);
        enricher.Setup(x => x.Enrich(It.IsAny<UserSignInLog>())).ThrowsAsync(new Exception("module down"));

        var writer = CreateWriter(service.Object, capacity: 10, batchSize: 10, enrichers: [enricher.Object]);
        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });

        await writer.Flush(CancellationToken.None);

        saved.Should().ContainSingle();
    }

    [Fact]
    public async Task Flush_RunsEnrichersInPriorityOrder()
    {
        var order = new List<int>();

        var second = new Mock<IUserSignInLogEnricher>();
        second.SetupGet(x => x.Priority).Returns(10);
        second.Setup(x => x.Enrich(It.IsAny<UserSignInLog>()))
            .Callback(() => order.Add(10)).Returns(Task.CompletedTask);

        var first = new Mock<IUserSignInLogEnricher>();
        first.SetupGet(x => x.Priority).Returns(1);
        first.Setup(x => x.Enrich(It.IsAny<UserSignInLog>()))
            .Callback(() => order.Add(1)).Returns(Task.CompletedTask);

        var writer = CreateWriter(Mock.Of<IUserSignInLogService>(), capacity: 10, batchSize: 10,
            enrichers: [second.Object, first.Object]);
        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });

        await writer.Flush(CancellationToken.None);

        order.Should().Equal(1, 10);
    }

    [Fact]
    public async Task FlushAsync_WhenPersistenceThrows_DoesNotPropagate()
    {
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChanges(It.IsAny<IList<UserSignInLog>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("db down"));

        var writer = CreateWriter(service.Object, capacity: 10, batchSize: 10);
        writer.Write(new UserSignInLog { UserName = "a", SignInType = SignInType.Password });

        var act = async () => await writer.Flush(CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task FlushAsync_MoreRecordsThanBatchSize_PersistsInBatches()
    {
        var batches = new List<int>();
        var service = new Mock<IUserSignInLogService>();
        service.Setup(x => x.SaveChanges(It.IsAny<IList<UserSignInLog>>(), It.IsAny<CancellationToken>()))
            .Callback<IList<UserSignInLog>, CancellationToken>((batch, _) => batches.Add(batch.Count))
            .Returns(Task.CompletedTask);

        var writer = CreateWriter(service.Object, capacity: 100, batchSize: 2);

        for (var i = 0; i < 5; i++)
        {
            writer.Write(new UserSignInLog { UserName = $"u{i}", SignInType = SignInType.Password });
        }

        await writer.Flush(CancellationToken.None);

        batches.Should().BeEquivalentTo([2, 2, 1]);
    }

    private static BufferedUserSignInLogWriter CreateWriter(
        IUserSignInLogService service,
        int capacity,
        int batchSize,
        IEnumerable<IUserSignInLogEnricher> enrichers = null)
    {
        // Enrichers are resolved from a scope per flushed batch, so the test supplies a real container.
        var services = new ServiceCollection();
        foreach (var enricher in enrichers ?? [])
        {
            services.AddSingleton(enricher);
        }

        return new BufferedUserSignInLogWriter(
            service,
            services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>(),
            NullLogger<BufferedUserSignInLogWriter>.Instance,
            Options.Create(new SignInLogOptions { BufferCapacity = capacity, BatchSize = batchSize }));
    }
}
