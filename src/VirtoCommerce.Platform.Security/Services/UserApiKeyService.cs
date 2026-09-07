using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Caching;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services
{
    public class UserApiKeyService : IUserApiKeyService
    {
        // Own key component rather than a prefix on the value: CacheKey.With joins positionally, so
        // "0-<raw>" and "1-<digest>" cannot collide whatever the caller sends.
        private const string RawKeyDiscriminator = "0";
        private const string DigestedKeyDiscriminator = "1";

        private static readonly TimeSpan _missingApiKeyExpiration = TimeSpan.FromSeconds(30);

        private readonly Func<ISecurityRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public UserApiKeyService(Func<ISecurityRepository> repositoryFactory, IPlatformMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
        }

        public async Task<UserApiKey> GetApiKeyByKeyAsync(string apiKey)
        {
            var cacheKey = BuildApiKeyCacheKey(apiKey);
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                //Add cache  expiration token
                cacheEntry.AddExpirationToken(ApiKeyCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var result = await repository.UserApiKeys.Where(x => x.ApiKey == apiKey)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync();
                    if (result == null)
                    {
                        // A miss is keyed on a string the caller chooses, so its entry must not be renewable
                        // from outside. Clamped, not assigned: a deployment configuring CacheAbsoluteExpiration
                        // below this bound must not have its negative entries lengthened. Nulled, not left
                        // alone: the entry arrives sliding-only, and a sliding window shorter than the bound
                        // would drop the entry between probes and send every probe back to the repository.
                        cacheEntry.AbsoluteExpirationRelativeToNow = ShorterOf(cacheEntry.AbsoluteExpirationRelativeToNow, _missingApiKeyExpiration);
                        cacheEntry.SlidingExpiration = null;
                    }

                    return result?.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance());
                }
            });
        }

        private string BuildApiKeyCacheKey(string apiKey)
        {
            // The threshold is the ApiKey column's HasMaxLength; the repository is still queried with the
            // candidate as presented.
            if (apiKey == null || apiKey.Length <= DbContextBase.Length128)
            {
                return CacheKey.With(GetType(), nameof(GetApiKeyByKeyAsync), RawKeyDiscriminator, apiKey);
            }

            // Hex, not Base64: CacheKey.Normalize lower-cases the key downstream.
            var digest = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(apiKey)));

            return CacheKey.With(GetType(), nameof(GetApiKeyByKeyAsync), DigestedKeyDiscriminator, digest);
        }

        private static TimeSpan ShorterOf(TimeSpan? configured, TimeSpan bound)
        {
            return configured.HasValue && configured.Value < bound ? configured.Value : bound;
        }

        public async Task<UserApiKey[]> GetAllUserApiKeysAsync(string userId)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetAllUserApiKeysAsync), userId);
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                //Add cache  expiration token
                cacheEntry.AddExpirationToken(ApiKeyCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var result = await repository.UserApiKeys.Where(x => x.UserId == userId)
                                                        .AsNoTracking()
                                                        .ToArrayAsync();
                    return result.Select(x => x.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task<UserApiKey> GetApiKeyByIdAsync(string id)
        {
            return (await GetApiKeysByIdsAsync(new[] { id })).FirstOrDefault();
        }

        public async Task<UserApiKey[]> GetApiKeysByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetApiKeysByIdsAsync), string.Join("-", ids));
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                //Add cache  expiration token
                cacheEntry.AddExpirationToken(ApiKeyCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var result = await repository.UserApiKeys.Where(x => ids.Contains(x.Id))
                                                             .AsNoTracking()
                                                             .ToArrayAsync();
                    return result.Select(x => x.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task<UserApiKey[]> SaveApiKeysAsync(UserApiKey[] apiKeys)
        {
            if (apiKeys == null)
            {
                throw new ArgumentNullException(nameof(apiKeys));
            }

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                var ids = apiKeys.Where(x => !x.IsTransient()).Select(x => x.Id).Distinct().ToArray();
                var apiKeysEntities = await repository.UserApiKeys.Where(x => ids.Contains(x.Id))
                                                                  .ToArrayAsync();
                foreach (var apiKey in apiKeys)
                {
                    var originalEntity = apiKeysEntities.FirstOrDefault(x => x.Id == apiKey.Id);
                    var modifiedEntity = AbstractTypeFactory<UserApiKeyEntity>.TryCreateInstance().FromModel(apiKey, pkMap);
                    if (originalEntity != null)
                    {
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                repository.UnitOfWork.Commit();
                pkMap.ResolvePrimaryKeys();
                ApiKeyCacheRegion.ExpireRegion();
            }
            return apiKeys;
        }

        public async Task DeleteApiKeysAsync(string[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var apiKey = new UserApiKeyEntity { Id = id };
                    repository.Attach(apiKey);
                    repository.Remove(apiKey);
                }

                await repository.UnitOfWork.CommitAsync();

                ApiKeyCacheRegion.ExpireRegion();
            }
        }
    }
}
