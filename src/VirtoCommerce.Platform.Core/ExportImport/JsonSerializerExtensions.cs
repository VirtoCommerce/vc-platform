using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public static class JsonSerializerExtensions
    {
        public const int DefaultPageSize = 50;

        [Obsolete("Use the cancellation-aware overload instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public static async Task SerializeArrayWithPagingAsync<T>(this JsonTextWriter writer, JsonSerializer serializer, int pageSize, Func<int, int, Task<GenericSearchResult<T>>> pagedDataLoader, Action<int, int> progressCallback, ICancellationToken cancellationToken)
        {
            //Evaluate total items counts
            var result = await pagedDataLoader(0, 1);

            var totalCount = result.TotalCount;

            await writer.WriteStartArrayAsync();

            // Prevent infinity loop
            if (pageSize <= 0)
            {
                pageSize = DefaultPageSize;
            }

            for (var i = 0; i < totalCount; i += pageSize)
            {
                var nextPage = await pagedDataLoader(i, pageSize);
                foreach (var data in nextPage.Results)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    serializer.Serialize(writer, data);
                }
                await writer.FlushAsync();
                progressCallback(Math.Min(totalCount, i + pageSize), totalCount);
            }
            await writer.WriteEndArrayAsync();
        }

        [Obsolete("Use the cancellation-aware overload instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public static async Task DeserializeArrayWithPagingAsync<T>(this JsonTextReader reader, JsonSerializer serializer, int pageSize, Func<IList<T>, Task> action, Action<int> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.ReadAsync();
            if (reader.TokenType == JsonToken.StartArray)
            {
                await reader.ReadAsync();

                var items = new List<T>();
                var processedCount = 0;
                while (reader.TokenType != JsonToken.EndArray)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var item = serializer.Deserialize<T>(reader);
                    items.Add(item);
                    processedCount++;
                    await reader.ReadAsync();
                    if (processedCount % pageSize == 0 || reader.TokenType == JsonToken.EndArray)
                    {
                        await action(items);
                        items.Clear();
                        progressCallback(processedCount);
                    }
                }
            }
        }

        public static async Task SerializeArrayWithPagingAsync<T>(this JsonTextWriter writer, JsonSerializer serializer, int pageSize, Func<int, int, Task<GenericSearchResult<T>>> pagedDataLoader, Action<int, int> progressCallback, CancellationToken cancellationToken)
        {
            var result = await pagedDataLoader(0, 1);

            var totalCount = result.TotalCount;

            await writer.WriteStartArrayAsync(cancellationToken);

            // Prevent infinity loop
            if (pageSize <= 0)
            {
                pageSize = DefaultPageSize;
            }

            for (var i = 0; i < totalCount; i += pageSize)
            {
                var nextPage = await pagedDataLoader(i, pageSize);
                foreach (var data in nextPage.Results)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    serializer.Serialize(writer, data);
                }
                await writer.FlushAsync(cancellationToken);
                progressCallback(Math.Min(totalCount, i + pageSize), totalCount);
            }
            await writer.WriteEndArrayAsync(cancellationToken);
        }

        public static async Task DeserializeArrayWithPagingAsync<T>(this JsonTextReader reader, JsonSerializer serializer, int pageSize, Func<IList<T>, Task> action, Action<int> progressCallback, CancellationToken cancellationToken)
        {
            await reader.ReadAsync(cancellationToken);
            if (reader.TokenType == JsonToken.StartArray)
            {
                await reader.ReadAsync(cancellationToken);

                var items = new List<T>();
                var processedCount = 0;
                while (reader.TokenType != JsonToken.EndArray)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var item = serializer.Deserialize<T>(reader);
                    items.Add(item);
                    processedCount++;
                    await reader.ReadAsync(cancellationToken);
                    if (processedCount % pageSize == 0 || reader.TokenType == JsonToken.EndArray)
                    {
                        await action(items);
                        items.Clear();
                        progressCallback(processedCount);
                    }
                }
            }
        }
    }
}
