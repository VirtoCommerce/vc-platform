using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Framework.Web.Asset;

namespace VirtoCommerce.CoreModule.Web.Assets
{
    public class AssetsProviderManager : IAssetsProviderManager, IBlobStorageProvider, IAssetRepository, IAssetUrlResolver, IUnitOfWork
    {
        private readonly IAssetsConnection _connection;
        private readonly ConcurrentDictionary<string, Func<string, IBlobStorageProvider>> _factories;
        private IBlobStorageProvider _currentProvider;

        public AssetsProviderManager(IAssetsConnection connection)
        {
            _connection = connection;
            _factories = new ConcurrentDictionary<string, Func<string, IBlobStorageProvider>>(StringComparer.OrdinalIgnoreCase);
        }

        #region IAssetsProviderManager Members

        public void RegisterProvider(string name, Func<string, IBlobStorageProvider> factory)
        {
            _factories.AddOrUpdate(name, factory, (key, oldValue) => factory);
        }

        #endregion

        #region IBlobStorageProvider Members

        public string Upload(UploadStreamInfo info)
        {
            return CurrentBlobStorageProvider.Upload(info);
        }

        public Stream OpenReadOnly(string blobKey)
        {
            return CurrentBlobStorageProvider.OpenReadOnly(blobKey);
        }

        public bool Exists(string blobKey)
        {
            return CurrentBlobStorageProvider.Exists(blobKey);
        }

        public byte[] GetImagePreview(string blobKey)
        {
            return CurrentBlobStorageProvider.GetImagePreview(blobKey);
        }

        #endregion

        #region IAssetRepository Members

        public Folder GetFolderById(string folderId)
        {
            return CurrentAssetRepository.GetFolderById(folderId);
        }

        public FolderItem GetFolderItemById(string itemId)
        {
            return CurrentAssetRepository.GetFolderItemById(itemId);
        }

        public Folder[] GetChildrenFolders(string folderId)
        {
            return CurrentAssetRepository.GetChildrenFolders(folderId);
        }

        public FolderItem[] GetChildrenFolderItems(string folderId)
        {
            return CurrentAssetRepository.GetChildrenFolderItems(folderId);
        }

        public Folder CreateFolder(string folderName, string parentId = null)
        {
            return CurrentAssetRepository.CreateFolder(folderName, parentId);
        }

        public void Delete(string id)
        {
            CurrentAssetRepository.Delete(id);
        }

        public void Rename(string id, string name)
        {
            CurrentAssetRepository.Rename(id, name);
        }

        #endregion

        #region IRepository Members

        public IUnitOfWork UnitOfWork
        {
            get { return CurrentAssetRepository.UnitOfWork; }
        }

        public void Attach<T>(T item) where T : class
        {
            CurrentAssetRepository.Attach(item);
        }

        public bool IsAttachedTo<T>(T item) where T : class
        {
            return CurrentAssetRepository.IsAttachedTo(item);
        }

        public void Add<T>(T item) where T : class
        {
            CurrentAssetRepository.Add(item);
        }

        public void Update<T>(T item) where T : class
        {
            CurrentAssetRepository.Update(item);
        }

        public void Remove<T>(T item) where T : class
        {
            CurrentAssetRepository.Remove(item);
        }

        public IQueryable<T> GetAsQueryable<T>() where T : class
        {
            return CurrentAssetRepository.GetAsQueryable<T>();
        }

        public void Refresh(IEnumerable collection)
        {
            CurrentAssetRepository.Refresh(collection);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                var assetRepository = _currentProvider as IAssetRepository;
                if (assetRepository != null)
                {
                    assetRepository.Dispose();
                }
            }
        }

        #endregion

        #region IAssetUrl Members

        public string GetAbsoluteUrl(string assetId, bool thumb = false)
        {
            return CurrentAssetUrlResolver.GetAbsoluteUrl(assetId, thumb);
        }

        public string GetRelativeUrl(string absoluteUrl)
        {
            return CurrentAssetUrlResolver.GetRelativeUrl(absoluteUrl);
        }

        #endregion

        #region IUnitOfWork Members

        public int Commit()
        {
            return CurrentUnitOfWork.Commit();
        }

        public void CommitAndRefreshChanges()
        {
            CurrentUnitOfWork.CommitAndRefreshChanges();
        }

        public void RollbackChanges()
        {
            CurrentUnitOfWork.RollbackChanges();
        }

        #endregion

        private IBlobStorageProvider CurrentBlobStorageProvider
        {
            get { return _currentProvider ?? (_currentProvider = CreateProvider()); }
        }

        private IAssetRepository CurrentAssetRepository
        {
            get { return CurrentBlobStorageProvider as IAssetRepository; }
        }

        private IAssetUrlResolver CurrentAssetUrlResolver
        {
            get { return CurrentBlobStorageProvider as IAssetUrlResolver; }
        }

        private IUnitOfWork CurrentUnitOfWork
        {
            get { return CurrentBlobStorageProvider as IUnitOfWork; }
        }


        private IBlobStorageProvider CreateProvider()
        {
            IBlobStorageProvider result = null;

            Func<string, IBlobStorageProvider> factory;
            if (_factories.TryGetValue(_connection.Provider, out factory))
            {
                result = factory(_connection.OriginalConnectionString);
            }

            return result;
        }
    }
}
