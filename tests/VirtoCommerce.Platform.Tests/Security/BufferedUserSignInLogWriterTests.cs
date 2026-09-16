using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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

        // DropWrite reports the eviction to the caller, so the count is exact rather than inferred
        // from queue depth - a silent gap in an audit trail is worse than a visible one.
        writer.DroppedCount.Should().Be(1);
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

    private static BufferedUserSignInLogWriter CreateWriter(IUserSignInLogService service, int capacity, int batchSize)
        => new(service,
               NullLogger<BufferedUserSignInLogWriter>.Instance,
               Options.Create(new SignInLogOptions { BufferCapacity = capacity, BatchSize = batchSize }));
}
