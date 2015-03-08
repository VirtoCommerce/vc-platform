using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
            var inMemoryCacheEntry = this._responseCache[cacheEntry.Key];
            if (inMemoryCacheEntry.Responses.ContainsKey(secondaryKey))
            {
                return await this.CloneAsync(inMemoryCacheEntry.Responses[secondaryKey]);
            }

            return null;
        }

        public Task<CacheEntry> GetEntryAsync(PrimaryCacheKey cacheKey)
        {
            // NB: Task.FromResult doesn't exist in MS.BCL.Async
            var ret = new TaskCompletionSource<CacheEntry>();

            if (this._responseCache.ContainsKey(cacheKey))
            {
                ret.SetResult(this._responseCache[cacheKey].CacheEntry);
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

            if (!this._responseCache.ContainsKey(entry.Key))
            {
                inMemoryCacheEntry = new InMemoryCacheEntry(entry);
                lock (this.syncRoot)
                {
                    this._responseCache[entry.Key] = inMemoryCacheEntry;
                }
            }
            else
            {
                inMemoryCacheEntry = this._responseCache[entry.Key];
            }

            var newContent = await this.CloneAsync(content);
            lock (this.syncRoot)
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
                await cacheContent.Response.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                newResponse.Content = new StreamContent(ms);
                foreach (var v in cacheContent.Response.Content.Headers)
                {
                    newResponse.Content.Headers.TryAddWithoutValidation(v.Key, v.Value);
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
            this.CacheEntry = cacheEntry;
            this.Responses = new Dictionary<string, CacheContent>();
        }

        #endregion

        #region Public Properties

        public CacheEntry CacheEntry { get; set; }
        public Dictionary<string, CacheContent> Responses { get; set; }

        #endregion
    }
}
