using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ExportImport
{
    public static class JsonSerializerExtensions
    {
        public static async Task SerializeJsonArrayWithPagingAsync<T>(this JsonTextWriter writer, JsonSerializer serializer, int pageSize, Func<int, int, Task<GenericSearchResult<T>>> pagedDataLoader, Action<int, int> progressCallback, ICancellationToken cancellationToken)
        {
            //Evaluate total items counts
            var result = await pagedDataLoader(0, 1);

            var totalCount = result.TotalCount;

            await writer.WriteStartArrayAsync();
            for (var i = 0; i < totalCount; i += pageSize)
            {
                var nextPage = await pagedDataLoader(i, pageSize);
                foreach (var data in nextPage.Results)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    serializer.Serialize(writer, data);
                }
                writer.Flush();
                progressCallback(Math.Min(totalCount, i + pageSize), totalCount);
            }
            await writer.WriteEndArrayAsync();
        }

        public static async Task DeserializeJsonArrayWithPagingAsync<T>(this JsonTextReader reader, JsonSerializer serializer, int pageSize, Func<IEnumerable<T>, Task> action, Action<int> progressCallback, ICancellationToken cancellationToken)
        {
            reader.Read();
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();

                var items = new List<T>();
                var processedCount = 0;
                while (reader.TokenType != JsonToken.EndArray)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var itemType = AbstractTypeFactory<T>.TryCreateInstance().GetType();
                    var item = serializer.Deserialize(reader, itemType);
                    items.Add((T)item);
                    processedCount++;
                    reader.Read();
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
