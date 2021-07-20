using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class PlatformRepositoryStub : IPlatformRepository, IUnitOfWork
    {
        [Obsolete("Use IAssetsRepository.AssetEntries from VirtoCommerce.AssetsModule.Data instead")]
        public IQueryable<AssetEntryEntity> AssetEntries { get; } = Enumerable.Empty<AssetEntryEntity>().AsQueryable();

        public IQueryable<SettingEntity> Settings { get; } = Enumerable.Empty<SettingEntity>().AsQueryable();

        public IQueryable<DynamicPropertyEntity> DynamicProperties { get; } = Enumerable.Empty<DynamicPropertyEntity>().AsQueryable();

        public IQueryable<DynamicPropertyDictionaryItemEntity> DynamicPropertyDictionaryItems { get; } =  Enumerable.Empty<DynamicPropertyDictionaryItemEntity>().AsQueryable();

        public IQueryable<OperationLogEntity> OperationLogs { get; } = Enumerable.Empty<OperationLogEntity>().AsQueryable();

        public IUnitOfWork UnitOfWork => this;

        public void Add<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Attach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }
        [Obsolete("Use IAssetsRepository.GetAssetsByIdsAsync from VirtoCommerce.AssetsModule.Data instead")]
        public Task<AssetEntryEntity[]> GetAssetsByIdsAsync(string[] ids)
        {
            return Task.FromResult(Array.Empty<AssetEntryEntity>());

        }

        public Task<DynamicPropertyEntity[]> GetDynamicPropertiesByIdsAsync(string[] ids)
        {
            return Task.FromResult(Array.Empty<DynamicPropertyEntity>());
        }

        public Task<DynamicPropertyEntity[]> GetDynamicPropertiesForTypesAsync(string[] objectTypes)
        {
            return Task.FromResult(Array.Empty<DynamicPropertyEntity>());
        }

        public Task<DynamicPropertyDictionaryItemEntity[]> GetDynamicPropertyDictionaryItemByIdsAsync(string[] ids)
        {
            return Task.FromResult(Array.Empty<DynamicPropertyDictionaryItemEntity>());
        }

        public Task<DynamicPropertyEntity[]> GetObjectDynamicPropertiesAsync(string[] objectTypes)
        {
            return Task.FromResult(Array.Empty<DynamicPropertyEntity>());
        }

        public Task<SettingEntity[]> GetObjectSettingsByNamesAsync(string[] names, string objectType, string objectId)
        {
            return Task.FromResult(Array.Empty<SettingEntity>());
        }

        public Task<OperationLogEntity[]> GetOperationLogsByIdsAsync(string[] ids)
        {
            return Task.FromResult(Array.Empty<OperationLogEntity>());
        }

        public void Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public int Commit()
        {
            throw new NotImplementedException();
        }

        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();
        }
    }
}
