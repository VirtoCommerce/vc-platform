using System.Collections;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;

namespace VirtoCommerce.Foundation.Data.Azure.Asset
{
    using Microsoft.Practices.Unity;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using VirtoCommerce.Foundation.Assets;
    using VirtoCommerce.Foundation.Assets.Factories;
    using VirtoCommerce.Foundation.Assets.Model;
    using VirtoCommerce.Foundation.Assets.Model.Exceptions;
    using VirtoCommerce.Foundation.Assets.Repositories;
    using VirtoCommerce.Foundation.Assets.Services;
    using VirtoCommerce.Foundation.Data.Common;
    using VirtoCommerce.Foundation.Data.Infrastructure;
    using VirtoCommerce.Foundation.Frameworks;
    using VirtoCommerce.Foundation.Frameworks.Extensions;

    public class AzureBlobAssetRepository : IAssetRepository, IBlobStorageProvider, IAssetUrl, IUnitOfWork
    {
        public const string DefaultBlobContainerName = "default-container";
        private const string ThumbMetadataKey = "ThumbImage";
        private const string DirectoryPlaceHolder = "placeholder.$$$";

        private readonly string _connectionString;
        private readonly List<Action> _delayedActions = new List<Action>();
        private readonly IAssetEntityFactory _entityFactory;
        private CloudBlobClient _cloudBlobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobAssetRepository"/> class.
        /// </summary>
        /// <param name="assetEntityFactory">The asset entity factory.</param>
        [InjectionConstructor]
        public AzureBlobAssetRepository(IAssetEntityFactory assetEntityFactory)
            : this(ConnectionHelper.GetConnectionString(AssetConfiguration.Instance.Connection.StorageConnectionStringName), assetEntityFactory)
        {
        }

		
        /// <summary>
        /// Prevents a default instance of the <see cref="AzureBlobAssetRepository"/> class from being created.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="assetEntityFactory">The asset entity factory.</param>
        public AzureBlobAssetRepository(string connectionString, IAssetEntityFactory assetEntityFactory)
        {
            this._entityFactory = assetEntityFactory;
            this._connectionString = connectionString;
            this.ChangeTracker = this.CreateChangeTracker();
        }

        #region IBlobStorageProvider Members

        public virtual string Upload(UploadStreamInfo info)
        {
            string result = null;
            // container name is up to the first occurrence of '/'
            var separatorIndex = info.FileName.IndexOf('/', 1);
            var containerName = info.FileName.Substring(0, separatorIndex);

            var container = this.CurrentCloudBlobClient.GetContainerReference(containerName);
            if (container.Exists())
            {
                var blobName = info.FileName.Substring(separatorIndex + 1);
                var blob = container.GetBlockBlobReference(blobName);
                blob.Properties.ContentType = ResolveContentType(blobName);

                using (var memoryStream = new MemoryStream())
                {
                    // upload to MemoryStream
                    memoryStream.SetLength(info.Length);
                    info.FileByteStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    // fill blob
                    blob.UploadFromStream(memoryStream);

                    // generate thumbnail
                    memoryStream.Position = 0;
                    var thumbBytes = GenerateThumb(memoryStream, 100, 100);
                    SaveThumb(thumbBytes, blob);
                }

                result = blob.Uri.ToString();
            }

            return result;
        }

        public virtual Stream OpenReadOnly(string blobKey)
        {
            if (Exists(blobKey))
            {
                //var container = this.CurrentCloudBlobClient.GetContainerReference(DefaultBlobContainerName);
                //var cloudBlob = container.GetBlobReferenceFromServer(blobKey);
                var cloudBlob = CurrentCloudBlobClient.GetBlobReferenceFromServer(GetUri(blobKey));

                var stream = new MemoryStream();
                cloudBlob.DownloadToStream(stream);
                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            else
            {
                return null;
            }
        }

        public bool Exists(string blobKey)
        {
            //	CloudBlobContainer container = CurrentCloudBlobClient.GetContainerReference(DefaultBlobContainerName);
            //	ICloudBlob cloudBlob = container.GetBlobReferenceFromServer(blobKey);
            var cloudBlob = CurrentCloudBlobClient.GetBlobReferenceFromServer(GetUri(blobKey));
            return cloudBlob.Exists();
        }

        public byte[] GetImagePreview(string blobKey)
        {
            byte[] result;
            var cloudBlob = CurrentCloudBlobClient.GetBlobReferenceFromServer(GetUri(blobKey));
            cloudBlob.FetchAttributes();

            if (cloudBlob.Metadata.ContainsKey(ThumbMetadataKey))
            {
                var imageAsString = cloudBlob.Metadata[ThumbMetadataKey];
                result = Convert.FromBase64String(imageAsString);
            }
            else
            {
                using (var dataStream = new MemoryStream())
                {
                    cloudBlob.DownloadToStream(dataStream);
                    result = GenerateThumb(dataStream, 100, 100);
                    SaveThumb(result, cloudBlob);
                }
            }

            // don't return null
            return result ?? new byte[1];
        }

        private Uri GetUri(string blobKey)
        {
            return new Uri(CurrentCloudBlobClient.BaseUri, blobKey);
        }

        #endregion

        #region IAssetRepository Members

        public virtual Folder GetFolderById(string folderId)
        {
            var retVal = this.ChangeTracker.TrackingEntries.Select(x => x.Entity)
                             .OfType<Folder>()
                             .FirstOrDefault(x => x.FolderId == folderId);

            if (retVal == null)
            {
                string directoryUri = folderId;
                var directory =
                    this.CurrentCloudBlobClient.ListBlobs(directoryUri, false, BlobListingDetails.None).FirstOrDefault() as
                    CloudBlobDirectory;

                string folderTypeName = this._entityFactory.GetEntityTypeStringName(typeof(Folder));
                retVal = this._entityFactory.CreateEntityForType(folderTypeName) as Folder;

                this.MapCloudBlobDirectory2Folder(directory, retVal);

                if (retVal != null)
                {
                    this.ChangeTracker.Attach(retVal);
                }
            }

            return retVal;
        }

        public virtual FolderItem GetFolderItemById(string itemId)
        {
            FolderItem retVal =
                this.ChangeTracker.TrackingEntries.Select(x => x.Entity)
                             .OfType<FolderItem>()
                             .FirstOrDefault(x => x.FolderItemId == itemId);
            if (retVal == null)
            {
                var cloudBlob = CurrentCloudBlobClient.GetBlobReferenceFromServer(GetUri(itemId)) as CloudBlockBlob;
                if (cloudBlob != null && cloudBlob.Exists())
                {
                    string folderItemTypeName = this._entityFactory.GetEntityTypeStringName(typeof(FolderItem));
                    retVal = this._entityFactory.CreateEntityForType(folderItemTypeName) as FolderItem;
                    this.MapCloudBlob2FolderItem(cloudBlob, retVal);
                }

                if (retVal != null)
                {
                    this.ChangeTracker.Attach(retVal);
                }
            }
            return retVal;
        }

        /// <summary>
        ///     Gets the children folders. If folderid is null or an empty string, containers are returned as folders.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <returns></returns>
        public virtual Folder[] GetChildrenFolders(string folderId)
        {
            //if(folderId == null)
            //    throw new ArgumentNullException("folderId");

            //todo: created, deleted or modified folders are not reflected to changetracker
            List<Folder> retVal = new List<Folder>();
            //    this.ChangeTracker.TrackingEntries.Select(x => x.Entity)
            //                 .OfType<Folder>()
            //                 .Where(x => x.ParentFolderId == folderId)
            //                 .ToList();
            //if (!retVal.Any())
            {
                string folderTypeName = this._entityFactory.GetEntityTypeStringName(typeof(Folder));

                if (String.IsNullOrEmpty(folderId))
                {
                    // use the prefix vc (virtocommerce)
                    //var containersSegment = CurrentCloudBlobClient.ListContainers();
                    IEnumerable<CloudBlobContainer> containers = this.CurrentCloudBlobClient.ListContainers();
                    //containersSegment.Results;

                    foreach (CloudBlobContainer container in containers)
                    {
                        var folder = this._entityFactory.CreateEntityForType(folderTypeName) as Folder;
                        this.MapCloudBlobContainer2Folder(container, folder);
                        retVal.Add(folder);

                        this.ChangeTracker.Attach(folder);
                    }
                }
                else
                {
                    if (!folderId.EndsWith(this.CurrentCloudBlobClient.DefaultDelimiter))
                        folderId += this.CurrentCloudBlobClient.DefaultDelimiter;

                    IEnumerable<CloudBlobDirectory> directories =
                        this.CurrentCloudBlobClient.ListBlobs(folderId, false, BlobListingDetails.None)
                                              .OfType<CloudBlobDirectory>();
                    foreach (CloudBlobDirectory subDirectory in directories)
                    {
                        var folder = this._entityFactory.CreateEntityForType(folderTypeName) as Folder;
                        this.MapCloudBlobDirectory2Folder(subDirectory, folder);
                        retVal.Add(folder);

                        this.ChangeTracker.Attach(folder);
                    }
                }
            }
            return retVal.ToArray();
        }

        /// <summary>
        ///     Gets the children folder items.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <returns></returns>
        public virtual FolderItem[] GetChildrenFolderItems(string folderId)
        {
            List<FolderItem> retVal =
                this.ChangeTracker.TrackingEntries.Select(x => x.Entity)
                             .OfType<FolderItem>()
                             .Where(x => x.FolderId == folderId && x.Name != DirectoryPlaceHolder)
                             .ToList();
            if (!retVal.Any())
            {
                if (!folderId.EndsWith(this.CurrentCloudBlobClient.DefaultDelimiter))
                    folderId += this.CurrentCloudBlobClient.DefaultDelimiter;

                IEnumerable<CloudBlockBlob> items =
                    this.CurrentCloudBlobClient.ListBlobs(folderId, false, BlobListingDetails.Metadata)
                                          .OfType<CloudBlockBlob>();
                foreach (CloudBlockBlob cloudBlob in items)
                {
                    string folderItemTypeName = this._entityFactory.GetEntityTypeStringName(typeof(FolderItem));
                    var folderItem = this._entityFactory.CreateEntityForType(folderItemTypeName) as FolderItem;
                    this.MapCloudBlob2FolderItem(cloudBlob, folderItem);
                    if (folderItem != null && folderItem.Name != DirectoryPlaceHolder)
                    {
                        retVal.Add(folderItem);
                        this.ChangeTracker.Attach(folderItem);
                    }
                }
            }
            return retVal.ToArray();
        }

        public Folder CreateFolder(string folderName, string parentId = null)
        {
            string folderTypeName = this._entityFactory.GetEntityTypeStringName(typeof(Folder));
            var folder = this._entityFactory.CreateEntityForType(folderTypeName) as Folder;
            CloudBlobContainer container;
            if (string.IsNullOrWhiteSpace(parentId))
            {
                container = CurrentCloudBlobClient.GetContainerReference(folderName.ToLower());
                container.CreateIfNotExists();
                MapCloudBlobContainer2Folder(container, folder);
            }
            else
            {
                var path = Combine(parentId, folderName);
                string containerName = GetContainer(path);
                string prefix = GetPrefix(path);

                container = CurrentCloudBlobClient.GetContainerReference(containerName);
                
                if (!container.ListBlobs(prefix).Any())
                {
                    container.GetBlockBlobReference(Combine(prefix, DirectoryPlaceHolder)).UploadText(string.Empty);
                }
                var directory = container.GetDirectoryReference(prefix);

                MapCloudBlobDirectory2Folder(directory, folder);
            }

            ChangeTracker.Attach(folder);
            return folder;
        }

        public void Delete(string id)
        {
            var container = CurrentCloudBlobClient.GetContainerReference(GetContainer(id));
            var listOfBlobs = container.ListBlobs(GetPrefix(id));

            Parallel.ForEach(listOfBlobs, item =>
            {
                var blob = CurrentCloudBlobClient.GetBlobReferenceFromServer(item.StorageUri);
                blob.Delete();
            });
        }

        public void Rename(string id, string name)
        {
            var container = CurrentCloudBlobClient.GetContainerReference(GetContainer(id));
            var prefix = GetPrefix(id);
            int index = prefix.LastIndexOf(CurrentCloudBlobClient.DefaultDelimiter, 1, StringComparison.Ordinal);
            var names = prefix.Split(new []{CurrentCloudBlobClient.DefaultDelimiter}, StringSplitOptions.RemoveEmptyEntries);
            names[names.Length - 1] = name;
            var newPrefix = names.JoinStrings(CurrentCloudBlobClient.DefaultDelimiter);
            if (prefix.EndsWith(CurrentCloudBlobClient.DefaultDelimiter))
            {
                newPrefix += CurrentCloudBlobClient.DefaultDelimiter;
            }
            var listOfBlobs = container.ListBlobs(prefix);

            Parallel.ForEach(listOfBlobs, item =>
            {
                var oldBlob = CurrentCloudBlobClient.GetBlobReferenceFromServer(item.StorageUri);
                var newBlob = container.GetBlockBlobReference(oldBlob.Name.Replace(prefix, newPrefix));
                newBlob.StartCopyFromBlob(item.Uri);
                oldBlob.Delete();
            });
        }

        public IUnitOfWork UnitOfWork
        {
            get { return this; }
        }

        public void Attach<T>(T item) where T : class
        {
            this.ChangeTracker.Attach(item);
        }

        public bool IsAttachedTo<T>(T item) where T : class
        {
            return this.ChangeTracker.IsAttached(item);
        }

        public void Add<T>(T item) where T : class
        {
            this.ChangeTracker.Add(item);
        }

        public void Update<T>(T item) where T : class
        {
            this.ChangeTracker.Update(item);
        }

        public void Remove<T>(T item) where T : class
        {
            this.ChangeTracker.Remove(item);
        }

        public IQueryable<T> GetAsQueryable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Refresh(IEnumerable collection)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUnitOfWork Members

        public int Commit()
        {
            foreach (Action delayedAction in this._delayedActions)
            {
                try
                {
                    delayedAction();
                }
                catch (StorageException e)
                {
                    throw new AssetStorageException(e.Message,
                                                    (AssetErrorCode)
                                                    e.RequestInformation.ExtendedErrorInformation.GetHashCode(), e);
                }
            }
            this._delayedActions.Clear();

            this.ChangeTracker.MarkAllUnchanged();
            return 0;
        }

        public void CommitAndRefreshChanges()
        {
            this.Commit();

            this.ChangeTracker.Dispose();

            this.ChangeTracker = this.CreateChangeTracker();
        }

        public void RollbackChanges()
        {
            this.ChangeTracker.Dispose();

            this.ChangeTracker = this.CreateChangeTracker();
        }

        #endregion

        #region Protected methods

        [CLSCompliant(false)]
        protected virtual void MapFolder2CloudBlobDirectory(Folder folder, CloudBlobDirectory directory)
        {
            //Nothing todo;		
        }

        [CLSCompliant(false)]
        protected virtual void MapCloudBlobContainer2Folder(CloudBlobContainer directory, Folder folder)
        {
            //Copy properties
            folder.LastModified = directory.Properties.LastModified.Value.DateTime;
            //Convert folder uri to Name, and ParentId properties
            folder.FolderId = directory.Name; //directory.Uri.ToString();
            string trimedFolderId = folder.FolderId; //.TrimEnd(folder.FolderId[folder.FolderId.Length - 1]);
            folder.Name =
                trimedFolderId.Substring(trimedFolderId.LastIndexOf(this.CurrentCloudBlobClient.DefaultDelimiter) + 1);
            folder.ParentFolderId = folder.FolderId.Contains(this.CurrentCloudBlobClient.DefaultDelimiter)
                                        ? folder.FolderId.Substring(0,
                                                                    trimedFolderId.LastIndexOf(
                                                                        this.CurrentCloudBlobClient.DefaultDelimiter))
                                        : "";
            //folder.ParentFolderId = folder.FolderId.Substring(0, trimedFolderId.LastIndexOf(CurrentCloudBlobClient.DefaultDelimiter));
        }

        [CLSCompliant(false)]
        protected virtual void MapCloudBlobDirectory2Folder(CloudBlobDirectory directory, Folder folder)
        {
            //Convert folder uri to Name, and ParentId properties
            folder.FolderId = String.Format("{0}{1}{2}", directory.Container.Name,
                                            this.CurrentCloudBlobClient.DefaultDelimiter, directory.Prefix);
            string trimedFolderId = folder.FolderId.TrimEnd(folder.FolderId[folder.FolderId.Length - 1]);
            folder.Name =
                trimedFolderId.Substring(trimedFolderId.LastIndexOf(this.CurrentCloudBlobClient.DefaultDelimiter) + 1);
            string parentPath = String.Format("{0}{1}{2}", directory.Container.Name,
                                              this.CurrentCloudBlobClient.DefaultDelimiter,
                                              directory.Parent != null ? directory.Parent.Prefix : "");
            folder.ParentFolderId = parentPath;
        }

        [CLSCompliant(false)]
        protected virtual void MapCloudBlob2FolderItem(CloudBlockBlob cloudBlob, FolderItem folderItem)
        {
            /*
            try
            {
                cloudBlob.FetchAttributes();
            }
            catch (StorageException e)
            {
                throw new AssetStorageException(e.Message, (AssetErrorCode)(int)e.RequestInformation.ExtendedErrorInformation.GetHashCode(), e); // TODO: error might not be correct here
            }
             * */

            //Copy properties

            folderItem.FolderItemId = cloudBlob.Uri.LocalPath.Substring(1);
            //folderItem.FolderItemId = String.Format("{0}{1}{2}{3}", cloudBlob.Container.Name, CurrentCloudBlobClient.DefaultDelimiter, cloudBlob.Parent != null ? cloudBlob.Parent.Prefix : "", cloudBlob.Name);
            folderItem.FolderId = String.Format("{0}{1}{2}", cloudBlob.Container.Name,
                                                this.CurrentCloudBlobClient.DefaultDelimiter,
                                                cloudBlob.Parent != null ? cloudBlob.Parent.Prefix : "");
            folderItem.Name =
                folderItem.FolderItemId.Substring(
                    folderItem.FolderItemId.LastIndexOf(this.CurrentCloudBlobClient.DefaultDelimiter) + 1);
            folderItem.ContentType = cloudBlob.Properties.ContentType;
            folderItem.ContentLanguage = cloudBlob.Properties.ContentLanguage;
            folderItem.ContentEncoding = cloudBlob.Properties.ContentEncoding;
            folderItem.LastModified = cloudBlob.Properties.LastModified.Value.DateTime;
            folderItem.ContentLength = cloudBlob.Properties.Length;

            //Read small data
            //var memoryStream = new MemoryStream();
            //cloudBlob.DownloadToStream(memoryStream);
            //folderItem.SmallData = memoryStream.ToArray();

            //Meta
            foreach (string metaKey in cloudBlob.Metadata.Keys)
            {
                PropertyInfo propertyInfo = folderItem.GetType().GetProperty(metaKey);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(folderItem, cloudBlob.Metadata[metaKey], null);
                }
            }
        }

        [CLSCompliant(false)]
        protected virtual void MapFolderItem2CloudBlob(FolderItem folderItem, ICloudBlob cloudBlob)
        {
            //Copy properties
            cloudBlob.Properties.ContentType = folderItem.ContentType;
            cloudBlob.Properties.ContentLanguage = folderItem.ContentLanguage;
            cloudBlob.Properties.ContentEncoding = folderItem.ContentEncoding;

            //Save small data
            if (folderItem.SmallData != null)
            {
                cloudBlob.UploadFromStream(new MemoryStream(folderItem.SmallData));
            }

            //Meta
            foreach (
                PropertyInfo propertyInfo in
                    folderItem.GetType()
                              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                              .Where(x => x.PropertyType == typeof(string) && x.Name != "Item"))
            {
                var value = propertyInfo.GetValue(folderItem, null) as String;
                if (value != null)
                {
                    cloudBlob.Metadata[propertyInfo.Name] = value;
                }
            }
        }

        #endregion

        #region Private methods

        private ObservableChangeTracker CreateChangeTracker()
        {
            var retVal = new ObservableChangeTracker
                {
                    AddAction = (x) =>
                        {
                            var entry = this.ChangeTracker.GetTrackingEntry(x);
                            this._delayedActions.Add(() => this.SaveEntryChanges(entry));
                        },
                    RemoveAction = (x) =>
                        {
                            var entry = this.ChangeTracker.GetTrackingEntry(x);
                            entry.EntryState = EntryState.Detached;
                            var clonedEntry = new TrackingEntry
                                {
                                    Entity = x.DeepClone(new AssetEntityFactory()),
                                    EntryState = EntryState.Deleted
                                };
                            this._delayedActions.Add(() => this.SaveEntryChanges(clonedEntry));
                        },
                    AddNewOneToManyRelationAction = (source, property, target) =>
                        {
                            var folder = source as Folder;
                            if (folder != null && property == "SubFolders")
                            {
                                if (folder.FolderId != null)
                                {
                                    ((Folder)target).ParentFolderId = folder.FolderId;
                                }
                            }
                            else if (folder != null && property == "FolderItems")
                            {
                                if (folder.FolderId != null)
                                {
                                    ((FolderItem)target).FolderId = folder.FolderId;
                                }
                            }
                        },
                    PropertyChangedAction = (source, property, target) =>
                        {
                            var folder = source as Folder;
                            if (folder != null && property == "FolderId")
                            {
                                foreach (var subFolder in folder.Subfolders)
                                {
                                    subFolder.ParentFolderId = folder.FolderId;
                                }
                                foreach (var folderItem in folder.FolderItems)
                                {
                                    folderItem.FolderId = folder.FolderId;
                                }
                            }
                        }
                };

            return retVal;
        }

        private void GenereateEntityKey(StorageEntity entity)
        {
            if (entity is Folder)
            {
                var folder = entity as Folder;

                var directoryUri = GetBlobDirectoryUri(folder);
                if (folder.ParentFolderId != null)
                {
                    var directory =
                        this.CurrentCloudBlobClient.ListBlobs(folder.ParentFolderId)
                                              .FirstOrDefault() as CloudBlobDirectory;
                    //var directory = CurrentCloudBlobClient.GetBlobDirectoryReference(folder.ParentFolderId);
                    directory = directory.GetSubdirectoryReference(folder.Name);
                    folder.FolderId = directory.Uri.ToString();
                }
                else
                {
                    var directory =
                        this.CurrentCloudBlobClient.ListBlobs(directoryUri).FirstOrDefault()
                        as CloudBlobDirectory;
                    //var directory = CurrentCloudBlobClient.GetBlobDirectoryReference(directoryUri);
                    folder.FolderId = directory.Uri.ToString();
                }
            }
            else if (entity is FolderItem)
            {
                var folderItem = entity as FolderItem;

                var cloudBlobUri = GetCloudBlobUri(folderItem);
                var directoryUri = folderItem.FolderId ?? DefaultBlobContainerName;
                var directory =
                    this.CurrentCloudBlobClient.ListBlobs(directoryUri).FirstOrDefault() as
                    CloudBlobDirectory;
                //var directory = CurrentCloudBlobClient.GetBlobDirectoryReference(directoryUri);
                var cloudBlob = directory.GetBlockBlobReference(cloudBlobUri);
                folderItem.FolderItemId = cloudBlob.Uri.ToString();
            }
        }

        private void SaveEntryChanges(TrackingEntry entry)
        {
            if ((entry.EntryState & (EntryState.Unchanged | EntryState.Detached)) != entry.EntryState)
            {
                if (entry.Entity is Folder)
                {
                    var folder = entry.Entity as Folder;

                    switch (entry.EntryState)
                    {
                        case EntryState.Modified:
                            break;
                        case EntryState.Added:
                            //CloudBlobDirectory directory;
                            //directory.Container.CreateIfNotExists();
                            //cloudBlob.UploadFromStream(new MemoryStream(new byte[] { }));
                            //cloudBlob.SetProperties();
                            //cloudBlob.SetMetadata();
                            break;
                        case EntryState.Deleted:
                            //directory.Container.Delete();
                            //cloudBlob.Delete();
                            break;
                    }
                }
                else if (entry.Entity is FolderItem)
                {
                    var folderItem = entry.Entity as FolderItem;

                    var directory =
                        this.CurrentCloudBlobClient.ListBlobs(folderItem.FolderId, false, BlobListingDetails.None)
                                              .FirstOrDefault() as CloudBlobDirectory;
                    //var directory = CurrentCloudBlobClient.GetBlobDirectoryReference(folderItem.FolderId);
                    var cloudBlob = directory.GetBlockBlobReference(folderItem.FolderItemId);

                    this.MapFolderItem2CloudBlob(folderItem, cloudBlob);

                    switch (entry.EntryState)
                    {
                        case EntryState.Added:
                        case EntryState.Modified:
                            if (cloudBlob.Properties.Length == 0)
                            {
                                cloudBlob.UploadFromStream(new MemoryStream(new byte[] { }));
                                //cloudBlob.UploadByteArray(new byte[] { });
                            }
                            cloudBlob.SetProperties();
                            cloudBlob.SetMetadata();
                            break;
                        case EntryState.Deleted:
                            cloudBlob.Delete();
                            break;
                    }
                }
            }
        }

        private static string GetBlobDirectoryUri(Folder folder)
        {
            var retval = folder.FolderId;
            if (String.IsNullOrEmpty(retval))
            {
                retval = folder.Name;
            }

            if (!AzureBlobStorageHelper.IsBlobContainerNameValid(retval))
            {
                throw new InvalidDataException("invalid container name");
            }

            return retval;
        }

        private static string GetCloudBlobUri(FolderItem folderItem)
        {
            var retval = folderItem.FolderItemId;
            if (String.IsNullOrEmpty(retval))
            {
                retval = folderItem.Name;
            }
            if (String.IsNullOrEmpty(retval))
            {
                retval = Guid.NewGuid().ToBase64();
            }
            return retval;
        }

        private static string ResolveContentType(string fileName)
        {
            string result;
            var mapping = new Dictionary<string, string>();
            mapping.Add("pdf", "application/pdf");
            mapping.Add("zip", "application/zip");
            mapping.Add("gz", "application/x-gzip");
            mapping.Add("gzip", "application/x-gzip");
            mapping.Add("m4a", "audio/mp4");
            mapping.Add("gif", "image/gif");
            mapping.Add("jpg", "image/jpeg");
            mapping.Add("jpeg", "image/jpeg");
            mapping.Add("png", "image/png");
            mapping.Add("svg", "image/svg+xml");
            mapping.Add("tif", "image/tiff");
            mapping.Add("tiff", "image/tiff");
            mapping.Add("csv", "text/csv");
            mapping.Add("html", "text/html");
            mapping.Add("mpg", "video/mpeg");
            mapping.Add("mpeg", "video/mpeg");
            mapping.Add("mp4", "video/mp4");
            mapping.Add("ogg", "video/ogg");
            mapping.Add("qt", "video/quicktime");
            mapping.Add("mov", "video/quicktime");

            var ext = Path.GetExtension(fileName).Substring(1).ToLower();
            if (mapping.ContainsKey(ext))
            {
                result = mapping[ext];
            }
            else
            {
                result = "application/octet-stream";
            }

            return result;
        }

        private static byte[] GenerateThumb(MemoryStream sourceStream, int width, int height)
        {
            byte[] result = null;
            try
            {
                var source = Image.FromStream(sourceStream);
                var newSize = new SizeF(width, (float)height * source.Height / source.Width);
                var target = new Bitmap((int)newSize.Width, (int)newSize.Height);

                using (var graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    // doesn't increase quality
                    // graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(source, 0, 0, newSize.Width, newSize.Height);

                    // Compress and save
                    var parameters = new EncoderParameters(1);
                    parameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
                    var encoderJpeg = ImageCodecInfo.GetImageEncoders().First(x => x.MimeType == "image/jpeg");

                    using (var memoryStream = new MemoryStream())
                    {
                        target.Save(memoryStream, encoderJpeg, parameters);
                        var thumbBytes = memoryStream.ToArray();
                        // return original bytes if generated thumbnail is bigger than original image.
                        if (thumbBytes.Length < sourceStream.Length)
                        {
                            result = thumbBytes;
                        }
                        else
                        {
                            sourceStream.Position = 0;
                            result = sourceStream.ToArray();
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private static void SaveThumb(byte[] thumbData, ICloudBlob blob)
        {
            if (thumbData != null)
            {
                var imageAsString = Convert.ToBase64String(thumbData);

                if (blob.Metadata.ContainsKey(ThumbMetadataKey))
                {
                    blob.Metadata[ThumbMetadataKey] = imageAsString;
                }
                else
                {
                    blob.Metadata.Add(ThumbMetadataKey, imageAsString);
                }

                blob.SetMetadata();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        public ObservableChangeTracker ChangeTracker { get; private set; }

        /// <summary>
        /// Gets the current cloud BLOB client.
        /// </summary>
        /// <value>
        /// The current cloud BLOB client.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Failed to get valid connection string</exception>
        [CLSCompliant(false)]
        protected CloudBlobClient CurrentCloudBlobClient
        {
            get
            {
                if (this._cloudBlobClient == null)
                {
                    CloudStorageAccount storageAcount;
                    if (!CloudStorageAccount.TryParse(this._connectionString, out storageAcount))
                    {
                        throw new InvalidOperationException("Failed to get valid connection string");
                    }
                    this._cloudBlobClient = storageAcount.CreateCloudBlobClient();
                }
                return this._cloudBlobClient;
            }
        }

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="thumb">is thumbnail</param>
        /// <returns>formatted string url including path to a storage server</returns>
        public string ResolveUrl(string assetId, bool thumb)
        {
            if (thumb)
            {
                if (!assetId.Contains(".thumb"))
                {
                    var extIdx = assetId.LastIndexOf(".", StringComparison.Ordinal);
                    if (extIdx != -1)
                    {
                        assetId = string.Format("{0}thumb{1}", assetId.Substring(0, extIdx + 1),
                            assetId.Substring(extIdx));
                    }
                }
            }
            else
            {
                assetId = assetId.Replace(".thumb", "");
            }
            var root = AzureConfiguration.Instance.AzureStorageAccount.BlobEndpoint.AbsoluteUri;

            return String.Format("{0}{1}", root.EndsWith("/") ? root : root + "/", assetId);
        }

        public string Combine(string path1, string path2)
        {
            if (string.IsNullOrWhiteSpace(path1))
            {
                path1 = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(path2))
            {
                path2 = string.Empty;
            }

            if (path2.StartsWith(CurrentCloudBlobClient.DefaultDelimiter))
            {
                path2 = path2.Substring(1);
            }

            return path1.EndsWith(CurrentCloudBlobClient.DefaultDelimiter)
                ? path1 + path2
                : path1 + CurrentCloudBlobClient.DefaultDelimiter + path2;
        }

        public string GetContainer(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            string container = path;
            int index = path.TrimStart().IndexOf(CurrentCloudBlobClient.DefaultDelimiter, 1, StringComparison.Ordinal);
            if (index >= 0)
            {
                container = path.Substring(0, index);
            }

            return container;
        }

        public string GetPrefix(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            path = path.TrimStart();
            string prefix = string.Empty;
            int index = path.TrimStart().IndexOf(CurrentCloudBlobClient.DefaultDelimiter, 1, StringComparison.Ordinal);
            if (index >= 0 && index < path.Length)
            {
                prefix = path.Substring(index + 1);
            }

            return prefix;
        }
    }
}