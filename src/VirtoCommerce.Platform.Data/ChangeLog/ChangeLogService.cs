using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public class ChangeLogService : IChangeLogService, ILastModifiedDateTime
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public ChangeLogService(
            Func<IPlatformRepository> platformRepositoryFactory
            , IPlatformMemoryCache memoryCache)
        {
            _repositoryFactory = platformRepositoryFactory;
            _memoryCache = memoryCache;
        }
        #region ILastModifiedDateTime Members

        public DateTimeOffset LastModified
        {
            get
            {
                var cacheKey = CacheKey.With(GetType(), "LastModifiedDateTime");
                return _memoryCache.GetOrCreateExclusive(cacheKey, (cacheEntry) =>
                {
                    cacheEntry.AddExpirationToken(ChangeLogCacheRegion.CreateChangeToken());
                    return DateTimeOffset.Now;
                });
            }
        }

        public void Reset()
        {
            ChangeLogCacheRegion.ExpireRegion();
        }

        #endregion
        #region IChangeLogService Members

        public async Task<OperationLog[]> GetByIdsAsync(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                repository.DisableChangesTracking();

                var existEntities = await repository.GetOperationLogsByIdsAsync(ids);
                return existEntities.Select(x => x.ToModel(AbstractTypeFactory<OperationLog>.TryCreateInstance())).ToArray();
            }
        }

        public virtual async Task SaveChangesAsync(params OperationLog[] operationLogs)
        {
            if (operationLogs == null)
            {
                throw new ArgumentNullException(nameof(operationLogs));
            }
            var pkMap = new PrimaryKeyResolvingMap();

            using (var repository = _repositoryFactory())
            {
                var ids = operationLogs.Where(x => !x.IsTransient()).Select(x => x.Id).Distinct().ToArray();
                var existEntities = await repository.GetOperationLogsByIdsAsync(ids);
                foreach (var operation in operationLogs)
                {
                    var existsEntity = existEntities.FirstOrDefault(x => x.Id == operation.Id);
                    var modifiedEntity = AbstractTypeFactory<OperationLogEntity>.TryCreateInstance().FromModel(operation, pkMap);
                    if (existsEntity != null)
                    {
                        modifiedEntity.Patch(existsEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                ChangeLogCacheRegion.ExpireRegion();
            }
        }

        public virtual async Task DeleteAsync(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var existEntities = await repository.GetOperationLogsByIdsAsync(ids);
                foreach (var entity in existEntities)
                {

                    repository.Remove(entity);
                }
                await repository.UnitOfWork.CommitAsync();
                ChangeLogCacheRegion.ExpireRegion();
            }
        }

        #endregion
    }
}
