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
                        // A miss is keyed on a string the caller chooses, and it is unlikely to be read
                        // again - a guess has to be novel to be worth making - so the entry is pure
                        // residency. Absolute rather than sliding: sliding is what would let one
                        // repeated guess stay resident indefinitely. Clamped rather than assigned,
                        // because a deployment configuring CacheAbsoluteExpiration below this would
                        // otherwise have its negative entries lengthened by the change.
                        cacheEntry.AbsoluteExpirationRelativeToNow = ShorterOf(cacheEntry.AbsoluteExpirationRelativeToNow, _missingApiKeyExpiration);
                        cacheEntry.SlidingExpiration = null;
                    }

                    return result?.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance());
                }
            });
        }

        private string BuildApiKeyCacheKey(string apiKey)
        {
            // Above the stored column's length no key this platform issues can appear, so the digest
            // branch is unreachable for a legitimate caller. It rejects nothing: the repository is
            // still queried with the candidate as presented, because SQL "=" ignores trailing spaces
            // on some providers and a longer candidate can therefore match a stored row.
            if (apiKey == null || apiKey.Length <= DbContextBase.Length128)
            {
                return CacheKey.With(GetType(), nameof(GetApiKeyByKeyAsync), RawKeyDiscriminator, apiKey);
            }

            // Lowercase hex, not Base64: CacheKey.Normalize lower-cases the whole key before it reaches
            // the cache, and Base64 is case-sensitive, so case-folding it would merge digests that
            // differ only in case. Hex survives normalization unchanged.
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
