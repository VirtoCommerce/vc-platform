using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.Caching
{
    public class InMemoryContentStore : IContentStore
    {
        #region Fields

        private readonly Dictionary<PrimaryCacheKey, InMemoryCacheEntry> _responseCache =
            new Dictionary<PrimaryCacheKey, InMemoryCacheEntry>();

        private readonly object syncRoot = new object();

        #endregion

        #region Public Methods and Operators

        public async Task<CacheContent> GetContentAsync(CacheEntry cacheEntry, string secondaryKey)
        {
            var inMemoryCacheEntry = _responseCache[cacheEntry.Key];
            if (inMemoryCacheEntry.Responses.ContainsKey(secondaryKey))
            {
                return await CloneAsync(inMemoryCacheEntry.Responses[secondaryKey]);
            }

            return null;
        }

        public Task<CacheEntry> GetEntryAsync(PrimaryCacheKey cacheKey)
        {
            // NB: Task.FromResult doesn't exist in MS.BCL.Async
            var ret = new TaskCompletionSource<CacheEntry>();

            if (_responseCache.ContainsKey(cacheKey))
            {
                ret.SetResult(_responseCache[cacheKey].CacheEntry);
            }
            else
            {
                ret.SetResult(null);
            }

            return ret.Task;
        }

        public async Task UpdateEntryAsync(CacheContent content)
        {
            var entry = content.CacheEntry;

            InMemoryCacheEntry inMemoryCacheEntry = null;

            if (!_responseCache.ContainsKey(entry.Key))
            {
                inMemoryCacheEntry = new InMemoryCacheEntry(entry);
                lock (syncRoot)
                {
                    _responseCache[entry.Key] = inMemoryCacheEntry;
                }
            }
            else
            {
                inMemoryCacheEntry = _responseCache[entry.Key];
            }

            var newContent = await CloneAsync(content);
            lock (syncRoot)
            {
                inMemoryCacheEntry.Responses[content.Key] = newContent;
            }
        }

        #endregion

        #region Methods

        private async Task<CacheContent> CloneAsync(CacheContent cacheContent)
        {
            var newResponse = new HttpResponseMessage(cacheContent.Response.StatusCode);
            var ms = new MemoryStream();

            foreach (var v in cacheContent.Response.Headers)
            {
                newResponse.Headers.TryAddWithoutValidation(v.Key, v.Value);
            }

            if (cacheContent.Response.Content != null)
            {
                var objectContent = cacheContent.Response.Content as ObjectContent;
                if (objectContent != null && objectContent.Value != null)
                {
                    newResponse.Content = new ObjectContent(objectContent.Value.GetType(), objectContent.Value, new JsonMediaTypeFormatter());
                }
                else
                {
                    await cacheContent.Response.Content.CopyToAsync(ms).ConfigureAwait(false);
                    ms.Position = 0;
                    newResponse.Content = new StreamContent(ms);
                    foreach (var v in cacheContent.Response.Content.Headers)
                    {
                        newResponse.Content.Headers.TryAddWithoutValidation(v.Key, v.Value);
                    }
                }
            }

            var newContent = new CacheContent()
            {
                CacheEntry = cacheContent.CacheEntry,
                Key = cacheContent.Key,
                Expires = cacheContent.Expires,
                HasValidator = cacheContent.HasValidator,
                CacheControl = cacheContent.CacheControl,
                Response = newResponse
            };

            return newContent;
        }

        #endregion
    }

    public class InMemoryCacheEntry
    {
        #region Constructors and Destructors

        public InMemoryCacheEntry(CacheEntry cacheEntry)
        {
            CacheEntry = cacheEntry;
            Responses = new Dictionary<string, CacheContent>();
        }

        #endregion

        #region Public Properties

        public CacheEntry CacheEntry { get; set; }
        public Dictionary<string, CacheContent> Responses { get; set; }

        #endregion
    }
}
