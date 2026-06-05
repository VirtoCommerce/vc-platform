using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.ExportImport;

[Trait("Category", "Unit")]
public class JsonSerializerExtensionsTests
{
    private sealed record Item(string Id);

    [Fact]
    public async Task SerializeArrayWithPagingAsync_PreCancelledToken_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        using var sw = new StringWriter();
        await using var writer = new JsonTextWriter(sw);
        var serializer = new JsonSerializer();

        Task<GenericSearchResult<Item>> Loader(int skip, int take) =>
            Task.FromResult(new GenericSearchResult<Item>
            {
                TotalCount = 100,
                Results = Enumerable.Range(skip, take).Select(i => new Item($"id{i}")).ToList(),
            });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            writer.SerializeArrayWithPagingAsync<Item>(serializer, pageSize: 10, Loader, (_, _) => { }, cts.Token));
    }

    [Fact]
    public async Task SerializeArrayWithPagingAsync_CancelMidStream_StopsBeforeRemainingPages()
    {
        const int totalItems = 100;
        const int pageSize = 10;
        var loaderCallCount = 0;

        using var cts = new CancellationTokenSource();

        using var sw = new StringWriter();
        await using var writer = new JsonTextWriter(sw);
        var serializer = new JsonSerializer();

        Task<GenericSearchResult<Item>> Loader(int skip, int take)
        {
            loaderCallCount++;
            if (loaderCallCount == 2)
            {
                cts.Cancel();
            }
            return Task.FromResult(new GenericSearchResult<Item>
            {
                TotalCount = totalItems,
                Results = Enumerable.Range(skip, take).Select(i => new Item($"id{i}")).ToList(),
            });
        }

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            writer.SerializeArrayWithPagingAsync<Item>(serializer, pageSize, Loader, (_, _) => { }, cts.Token));

        Assert.True(loaderCallCount < totalItems / pageSize,
            $"Expected pagination to abort early, but loader ran {loaderCallCount} times.");
    }

    [Fact]
    public async Task DeserializeArrayWithPagingAsync_PreCancelledToken_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var json = "[" + string.Join(",", Enumerable.Range(0, 30).Select(i => $"{{\"Id\":\"id{i}\"}}")) + "]";
        using var sr = new StringReader(json);
        using var reader = new JsonTextReader(sr);
        var serializer = new JsonSerializer();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            reader.DeserializeArrayWithPagingAsync<Item>(serializer, pageSize: 10, _ => Task.CompletedTask, _ => { }, cts.Token));
    }

    [Fact]
    public async Task DeserializeArrayWithPagingAsync_CancelMidStream_StopsBeforeRemainingItems()
    {
        const int totalItems = 50;
        const int pageSize = 10;
        var processedBatches = 0;

        using var cts = new CancellationTokenSource();

        var json = "[" + string.Join(",", Enumerable.Range(0, totalItems).Select(i => $"{{\"Id\":\"id{i}\"}}")) + "]";
        using var sr = new StringReader(json);
        using var reader = new JsonTextReader(sr);
        var serializer = new JsonSerializer();

        Task BatchAction(IList<Item> batch)
        {
            processedBatches++;
            if (processedBatches == 2)
            {
                cts.Cancel();
            }
            return Task.CompletedTask;
        }

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            reader.DeserializeArrayWithPagingAsync<Item>(serializer, pageSize, BatchAction, _ => { }, cts.Token));

        Assert.True(processedBatches < totalItems / pageSize,
            $"Expected pagination to abort early, but {processedBatches} batches were processed.");
    }
}
