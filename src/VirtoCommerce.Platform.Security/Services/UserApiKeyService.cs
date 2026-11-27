using System;
using System.Linq;
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
    /// <summary>
    /// Stores API keys as hashes. A key's value leaves the platform once, in the result of the save that created
    /// it. Keys from before key hashing are hashed by the AddUserApiKeyHash migration; any the migration could not
    /// hash are hashed on first use.
    /// </summary>
    public class UserApiKeyService : IUserApiKeyService
    {
        /// <summary>
        /// The mask the API keys endpoint used before key hashing.
        /// </summary>
        private const string LegacyMask = "********";

        private static readonly TimeSpan _missingApiKeyExpiration = TimeSpan.FromSeconds(30);

        /// <summary>
        /// The shortest key a client may choose. A UUID, which older admin UIs generate, is 36 characters; keys the
        /// platform generates are 46.
        /// </summary>
        protected virtual int MinimumApiKeyLength => 32;

        private readonly Func<ISecurityRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;
        private readonly IApiKeyHasher _hasher;
        private readonly IApiKeyGenerator _generator;

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

                // Longer than the column: no row from before key hashing can hold it.
                if (entity == null && apiKey.Length <= DbContextBase.Length128)
                {
                    entity = await HashLegacyKeyAsync(repository, apiKey, apiKeyHash);
                }

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
        /// A key that still has its plaintext is found by it, hashed and its plaintext cleared there and then: a key
        /// the migration did not hash (SQL Server hashes only ASCII keys in SQL) or one an older instance saved
        /// during a rolling upgrade.
        /// </summary>
        private async Task<UserApiKeyEntity> HashLegacyKeyAsync(ISecurityRepository repository, string apiKey, string apiKeyHash)
        {
            var entity = await repository.UserApiKeys
                .Where(x => x.ApiKey == apiKey)
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                entity.ApiKeyHash = apiKeyHash;
                entity.ApiKeyHint = _hasher.GetHint(apiKey);
                entity.ApiKey = null;
                await repository.UnitOfWork.CommitAsync();
            }

            return entity;
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
        /// <item>any other value: it becomes the key, unless it looks masked, which is refused.</item>
        /// </list>
        /// On return, <see cref="UserApiKey.ApiKey"/> holds the key only where this save set it.
        /// </summary>
        /// <exception cref="ApiKeyValidationException">A key value that looks masked but is not the key's own hint, or is too short.</exception>
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

                // All keys are checked before any is changed, so a refused key leaves the whole batch unsaved.
                foreach (var apiKey in apiKeys)
                {
                    SetKeyHash(apiKey, apiKeysEntities.FirstOrDefault(x => x.Id == apiKey.Id));
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
                repository.UnitOfWork.Commit();
                pkMap.ResolvePrimaryKeys();
                ApiKeyCacheRegion.ExpireRegion();
            }
            return apiKeys;
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

                if (originalEntity != null)
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
            else if (LooksMasked(value))
            {
                // Saved, the mask would become a real key, readable by everyone who can read the key list.
                throw new ApiKeyValidationException("The API key looks masked. Send an empty value to keep the stored key, or a new key to replace it.");
            }
            else if (value.Length < MinimumApiKeyLength)
            {
                throw new ApiKeyValidationException($"The API key must be at least {MinimumApiKeyLength} characters long. Send an empty value to have the platform generate one.");
            }

            apiKey.ApiKeyHash = _hasher.Hash(value);
            apiKey.ApiKeyHint = _hasher.GetHint(value);
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

        private static bool LooksMasked(string value)
        {
            return value.Contains(Sha256ApiKeyHasher.HintSeparator) || value.Contains(LegacyMask);
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
