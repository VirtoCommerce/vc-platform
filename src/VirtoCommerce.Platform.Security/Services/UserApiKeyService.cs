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
    public class UserApiKeyService : IUserApiKeyService
    {
        private readonly Func<ISecurityRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public UserApiKeyService(Func<ISecurityRepository> repositoryFactory, IPlatformMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
        }

        public async Task<UserApiKey> GetApiKeyByKeyAsync(string apiKey)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetApiKeyByKeyAsync), apiKey);
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                //Add cache  expiration token
                cacheEntry.AddExpirationToken(ApiKeyCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var result = await repository.UserApiKeys.Where(x => x.ApiKey == apiKey)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync();
                    return result?.ToModel(AbstractTypeFactory<UserApiKey>.TryCreateInstance());
                }
            });
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
