using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Caching;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services
{
    /// <summary>
    /// Stores API keys as hashes. A key's value leaves the platform once, in the result of the save that created
    /// it. Keys from before key hashing are hashed by the AddUserApiKeyHash migration, which clears their plaintext.
    /// </summary>
    public class UserApiKeyService : IUserApiKeyService
    {
        private static readonly TimeSpan _missingApiKeyExpiration = TimeSpan.FromSeconds(30);

        private readonly Func<ISecurityRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;
        private readonly IApiKeyHasher _hasher;
        private readonly IApiKeyGenerator _generator;

        [Obsolete("Use the constructor that takes IApiKeyHasher and IApiKeyGenerator: this one ignores the hasher and generator registered in DI.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public UserApiKeyService(Func<ISecurityRepository> repositoryFactory, IPlatformMemoryCache memoryCache)
            : this(repositoryFactory, memoryCache, new Sha256ApiKeyHasher(), new DefaultApiKeyGenerator())
        {
        }

        public UserApiKeyService(
            Func<ISecurityRepository> repositoryFactory,
            IPlatformMemoryCache memoryCache,
            IApiKeyHasher hasher,
            IApiKeyGenerator generator)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
            _hasher = hasher;
            _generator = generator;
        }

        public async Task<UserApiKey> GetApiKeyByKeyAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return null;
            }

            // Keyed on the hash: the cache holds no key, and a candidate of any length gives a key of one size.
            var apiKeyHash = _hasher.Hash(apiKey);
            var cacheKey = CacheKey.With(GetType(), nameof(GetApiKeyByKeyAsync), apiKeyHash);

            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(ApiKeyCacheRegion.CreateChangeToken());

                using var repository = _repositoryFactory();

                var entity = await repository.UserApiKeys
                    .Where(x => x.ApiKeyHash == apiKeyHash)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    // A miss is keyed on a string the caller chooses, so its entry must not be renewable
                    // from outside. Clamped, not assigned: a deployment configuring CacheAbsoluteExpiration
                    // below this bound must not have its negative entries lengthened. Nulled, not left
                    // alone: the entry arrives sliding-only, and a sliding window shorter than the bound
                    // would drop the entry between probes and send every probe back to the repository.
                    cacheEntry.AbsoluteExpirationRelativeToNow = ShorterOf(cacheEntry.AbsoluteExpirationRelativeToNow, _missingApiKeyExpiration);
                    cacheEntry.SlidingExpiration = null;
                }

                return entity == null ? null : ToModel(entity);
            });
        }

        /// <summary>
        /// A row from before key hashing that is not hashed yet has no stored hint; it gets one for display.
        /// </summary>
        protected virtual UserApiKey ToModel(UserApiKeyEntity entity)
        {
            var apiKey = entity.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance());
            if (apiKey.ApiKeyHint == null && apiKey.ApiKey != null)
            {
                apiKey.ApiKeyHint = _hasher.GetHint(apiKey.ApiKey);
            }

            return apiKey;
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
                    return result.Select(ToModel).ToArray();
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
                    return result.Select(ToModel).ToArray();
                }
            });
        }

        /// <summary>
        /// Saves keys, storing only their hashes. Per key, <see cref="UserApiKey.ApiKey"/> decides:
        /// <list type="bullet">
        /// <item>empty, with <see cref="UserApiKey.ApiKeyHash"/> (a backup restore): that hash is stored;</item>
        /// <item>empty, or the key's own hint, on an existing key: the stored key is kept;</item>
        /// <item>empty, on a new key: the platform generates the key;</item>
        /// <item>any other value: it becomes the key.</item>
        /// </list>
        /// The service accepts any value, so a backup from before key hashing restores whatever its keys are; the
        /// REST API checks what clients send before it gets here.
        /// On return, <see cref="UserApiKey.ApiKey"/> holds the key only where this save set it.
        /// </summary>
        /// <exception cref="ApiKeyConflictException">Two keys in the batch, or a key in the batch and another stored key,
        /// would have the same value. Nothing is saved.</exception>
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

                // All hashes are set before any entity is changed, so an override that throws leaves the batch unsaved.
                foreach (var apiKey in apiKeys)
                {
                    SetKeyHash(apiKey, apiKeysEntities.FirstOrDefault(x => x.Id == apiKey.Id));
                }

                // The unique index would refuse it too, but with the hash in a database error the API would pass on.
                var hashes = apiKeys.Select(x => x.ApiKeyHash).ToArray();
                if (hashes.Distinct().Count() != hashes.Length || await IsAnyHashTakenAsync(repository, hashes, ids))
                {
                    throw new ApiKeyConflictException();
                }

                foreach (var apiKey in apiKeys)
                {
                    var originalEntity = apiKeysEntities.FirstOrDefault(x => x.Id == apiKey.Id);
                    var modifiedEntity = AbstractTypeFactory<UserApiKeyEntity>.TryCreateInstance().FromModel(apiKey, pkMap);

                    // The key itself is never stored; a row from before key hashing loses its plaintext here.
                    modifiedEntity.ApiKey = null;

                    if (originalEntity != null)
                    {
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                try
                {
                    repository.UnitOfWork.Commit();
                }
                catch (DbUpdateException ex)
                {
                    // Another save took one of these values between the check above and this commit.
                    using var checkRepository = _repositoryFactory();
                    if (await IsAnyHashTakenAsync(checkRepository, hashes, ids))
                    {
                        throw new ApiKeyConflictException(ex);
                    }

                    throw;
                }

                pkMap.ResolvePrimaryKeys();
                ApiKeyCacheRegion.ExpireRegion();
            }
            return apiKeys;
        }

        /// <summary>
        /// Whether a key other than the ones being saved already has one of these hashes.
        /// </summary>
        private static Task<bool> IsAnyHashTakenAsync(ISecurityRepository repository, string[] hashes, string[] savedIds)
        {
            return repository.UserApiKeys.AnyAsync(x => hashes.Contains(x.ApiKeyHash) && !savedIds.Contains(x.Id));
        }

        /// <summary>
        /// Sets <see cref="UserApiKey.ApiKeyHash"/> and <see cref="UserApiKey.ApiKeyHint"/> from the key's value,
        /// the stored key, or a new generated key, following the rules of <see cref="SaveApiKeysAsync"/>.
        /// </summary>
        protected virtual void SetKeyHash(UserApiKey apiKey, UserApiKeyEntity originalEntity)
        {
            var value = apiKey.ApiKey;

            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(apiKey.ApiKeyHash))
                {
                    apiKey.ApiKeyHint ??= Sha256ApiKeyHasher.HintSeparator;
                    return;
                }

                // A stored row with neither a hash nor a key has nothing to keep: it gets a key like a new one.
                if (originalEntity != null && HasStoredKey(originalEntity))
                {
                    KeepStoredKey(apiKey, originalEntity);
                    return;
                }

                value = _generator.Generate();
                apiKey.ApiKey = value;
            }
            else if (originalEntity != null && IsShownValueOf(value, originalEntity))
            {
                KeepStoredKey(apiKey, originalEntity);
                return;
            }

            apiKey.ApiKeyHash = _hasher.Hash(value);
            apiKey.ApiKeyHint = _hasher.GetHint(value);
        }

        private static bool HasStoredKey(UserApiKeyEntity entity)
        {
            return !string.IsNullOrEmpty(entity.ApiKeyHash) || !string.IsNullOrEmpty(entity.ApiKey);
        }

        private void KeepStoredKey(UserApiKey apiKey, UserApiKeyEntity originalEntity)
        {
            apiKey.ApiKey = null;
            apiKey.ApiKeyHash = originalEntity.ApiKeyHash ?? _hasher.Hash(originalEntity.ApiKey);
            apiKey.ApiKeyHint = originalEntity.ApiKeyHint ?? _hasher.GetHint(originalEntity.ApiKey);
        }

        /// <summary>
        /// Whether a client sent back what it was shown for this key: its hint, or, for a key from before key
        /// hashing, the key itself, which clients of older versions still hold.
        /// </summary>
        private bool IsShownValueOf(string value, UserApiKeyEntity entity)
        {
            if (value == entity.ApiKeyHint)
            {
                return true;
            }

            return entity.ApiKey != null && (value == entity.ApiKey || value == _hasher.GetHint(entity.ApiKey));
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
