using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Assets;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Model.Exceptions;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Foundation.Data.Asset
{
    using VirtoCommerce.Foundation.Data.Infrastructure;

    public class FileSystemBlobAssetRepository : IAssetRepository, IBlobStorageProvider, IUnitOfWork
	{
		private List<Action> _delayedActions = new List<Action>();

		private readonly IAssetEntityFactory _entityFactory;
		private readonly string _storagePath;

		public ObservableChangeTracker ChangeTracker { get; private set; }

		[InjectionConstructor]
		public FileSystemBlobAssetRepository(IAssetEntityFactory assetEntityFactory)
            : this(ConnectionHelper.GetConnectionString(AssetConfiguration.Instance.Connection.StorageConnectionStringName), assetEntityFactory)
		{
		}

		public FileSystemBlobAssetRepository(string storagePath, IAssetEntityFactory assetEntityFactory)
		{
			_storagePath = storagePath;
			_entityFactory = assetEntityFactory;
			ChangeTracker = CreateChangeTracker();
		}

		#region IBlobStorageProvider Members
		public virtual string Upload(UploadStreamInfo info)
		{
			var filePath = Absolute(info.FileName);

			using (var memoryStream = new MemoryStream())
			{
				memoryStream.SetLength(info.Length);
				info.FileByteStream.CopyTo(memoryStream);
				memoryStream.Position = 0;

				using (var file = File.OpenWrite(filePath))
				{
					memoryStream.WriteTo(file);
				}

				// generate thumbnail
				memoryStream.Position = 0;
				filePath = GenerateThumbnailPath(filePath);
				GenerateThumbnail(filePath, memoryStream);
			}

			return info.FileName;
		}

		private static bool GenerateThumbnail(string filePath, Stream sourceStream)
		{
			try
			{
				var image = Image.FromStream(sourceStream);
				GenerateNewImage(image, 100, 100, filePath);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static string GenerateThumbnailPath(string filePath)
		{
			filePath = Path.ChangeExtension(filePath, "thumb") + Path.GetExtension(filePath);
			return filePath;
		}

		public virtual Stream OpenReadOnly(string filePath)
		{
			filePath = Absolute(filePath);

			var stream = new MemoryStream();
			using (var fileStream = File.OpenRead(filePath))
			{
				stream.SetLength(fileStream.Length);
				fileStream.Read(stream.GetBuffer(), 0, (int)fileStream.Length);
			}
			if (stream.CanSeek)
				stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}

		public bool Exists(string blobKey)
		{
			return File.Exists(blobKey);
		}

		public byte[] GetImagePreview(string filePath)
		{
			byte[] result = null;
			bool thumbnailExists;

			var thumbnailPathRelative = GenerateThumbnailPath(filePath);
			var thumbnailPath = Absolute(thumbnailPathRelative);
			
			if (File.Exists(thumbnailPath))
			{
				thumbnailExists = true;
			}
			else
			{
				using (var dataStream = OpenReadOnly(filePath))
				{
					thumbnailExists = GenerateThumbnail(thumbnailPath, dataStream);
				}
			}

			if (thumbnailExists)
			{
				using (var dataStream = OpenReadOnly(thumbnailPathRelative))
				{
					// 1 MB is the file length limit for preview
					if (dataStream.Length <= 1048576)
					{
						var memoryStream = (MemoryStream)dataStream;
						memoryStream.SetLength(dataStream.Length);
						dataStream.CopyTo(memoryStream);
						result = memoryStream.ToArray();
					}
				}
			}

			return result ?? new byte[1];
		}

		#endregion

		#region IAssetRepository Members

		public virtual Folder GetFolderById(string folderId)
		{
			folderId = Absolute(folderId);
			Folder retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<Folder>().FirstOrDefault(x => x.FolderId == folderId);

			if (retVal == null)
			{
				var folderTypeName = _entityFactory.GetEntityTypeStringName(typeof(Folder));
				retVal = _entityFactory.CreateEntityForType(folderTypeName) as Folder;

				MapFileSystemBlobDirectory2Folder(folderId, retVal);

				if (retVal != null)
				{
					ChangeTracker.Attach(retVal);
				}
			}

			return retVal;
		}

		public virtual FolderItem GetFolderItemById(string itemId)
		{
			itemId = Absolute(itemId);
			FolderItem retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<FolderItem>().FirstOrDefault(x => x.FolderItemId == itemId);

			if (retVal == null)
			{
				if (File.Exists(itemId))
				{
					var folderItemTypeName = _entityFactory.GetEntityTypeStringName(typeof(FolderItem));
					retVal = _entityFactory.CreateEntityForType(folderItemTypeName) as FolderItem;
					MapFileSystemBlob2FolderItem(itemId, retVal, true);
				}

				if (retVal != null)
				{
					ChangeTracker.Attach(retVal);
				}
			}

			return retVal;
		}

		/// <summary>
		/// Gets the children folders. If folderid is null or an empty string, containers are returned as folders.
		/// </summary>
		/// <param name="folderId">The folder id.</param>
		/// <returns></returns>
		public virtual Folder[] GetChildrenFolders(string folderId)
		{
			if (string.IsNullOrEmpty(folderId))
			{
				//HttpContext
				folderId = RootFolder;
			}
			else
			{
				folderId = Absolute(folderId);
			}

			List<Folder> retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<Folder>().Where(x => x.ParentFolderId == folderId).ToList();
			if (!retVal.Any())
			{
				var folderTypeName = _entityFactory.GetEntityTypeStringName(typeof(Folder));

				if (String.IsNullOrEmpty(folderId))
				{
					var folder = _entityFactory.CreateEntityForType(folderTypeName) as Folder;
					MapFileSystemBlobDirectory2Folder(folderId, folder);
					retVal.Add(folder);

					ChangeTracker.Attach(folder);
				}
				else
				{
					var directories = Directory.GetDirectories(folderId);
					foreach (var subDirectory in directories)
					{
						var folder = _entityFactory.CreateEntityForType(folderTypeName) as Folder;
						MapFileSystemBlobDirectory2Folder(subDirectory, folder);
						retVal.Add(folder);

						ChangeTracker.Attach(folder);
					}
				}
			}
			return retVal.ToArray();
		}

		/// <summary>
		/// Gets the children folder items.
		/// </summary>
		/// <param name="folderId">The folder id.</param>
		/// <returns></returns>
		public virtual FolderItem[] GetChildrenFolderItems(string folderId)
		{
			folderId = Absolute(folderId);
			List<FolderItem> retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<FolderItem>().Where(x => x.FolderId == folderId).ToList();
			if (!retVal.Any())
			{
				var items = Directory.GetFiles(folderId);
				foreach (var localBlob in items)
				{
					var fileExt = Path.GetExtension(localBlob);
					if (!(localBlob.EndsWith(".thumb" + fileExt, StringComparison.OrdinalIgnoreCase) && items.Any(x => x == localBlob.Remove(localBlob.LastIndexOf(".thumb", StringComparison.OrdinalIgnoreCase), ".thumb".Length))))
					{
						var folderItemTypeName = _entityFactory.GetEntityTypeStringName(typeof(FolderItem));
						var folderItem = _entityFactory.CreateEntityForType(folderItemTypeName) as FolderItem;
						MapFileSystemBlob2FolderItem(localBlob, folderItem, false);
						retVal.Add(folderItem);

						ChangeTracker.Attach(folderItem);
					}
				}
			}
			return retVal.ToArray();
		}

	    public Folder CreateFolder(string folderName, string parentId = null)
	    {
	        var fullName = Absolute(Path.Combine(parentId ?? "", folderName));
	        var folder = new Folder {Name = folderName};
	        if (!Directory.Exists(fullName))
	        {
	            Directory.CreateDirectory(fullName);   
	        }

            MapFileSystemBlobDirectory2Folder(fullName, folder);
            ChangeTracker.Attach(folder);
	        return folder;
	    }

	    public void Delete(string id)
	    {
            var itemPath = Absolute(id);
	        if (File.Exists(itemPath))
	        {
                File.Delete(itemPath);
                var thumbFile = GenerateThumbnailPath(itemPath);
                if (File.Exists(thumbFile))
                {
                    File.Delete(thumbFile);
                }

                return;
	        }

	        if (Directory.Exists(itemPath))
	        {
                Directory.Delete(itemPath, true);
	        }
	    }

	    public void Rename(string id, string name)
	    {
            var oldItemPath = Absolute(id);

            if (File.Exists(oldItemPath))
            {
                var newItemPath = Path.Combine(Path.GetDirectoryName(oldItemPath) ?? String.Empty, name);
                File.Move(oldItemPath, newItemPath);
                var thumbFile = GenerateThumbnailPath(oldItemPath);
                if (File.Exists(thumbFile))
                {
                    var newthumbFile = GenerateThumbnailPath(newItemPath);
                    File.Move(thumbFile, newthumbFile);
                }
                return;
            }

            if (Directory.Exists(oldItemPath))
            {
                var oldFolderPath = Absolute(id);
                var newFolderPath = Path.Combine(Path.GetDirectoryName(oldFolderPath) ?? String.Empty, name);
                Directory.Move(oldFolderPath, newFolderPath);
            }
	    }

	    public IUnitOfWork UnitOfWork
		{
			get { return this; }
		}

		public void Attach<T>(T item) where T : class
		{
			ChangeTracker.Attach(item);
		}

		public bool IsAttachedTo<T>(T item) where T : class
		{
			return ChangeTracker.IsAttached(item);
		}

		public void Add<T>(T item) where T : class
		{
			ChangeTracker.Add(item);
		}

		public void Update<T>(T item) where T : class
		{
			ChangeTracker.Update(item);
		}

		public void Remove<T>(T item) where T : class
		{
			ChangeTracker.Remove(item);
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
                catch (Exception e)
                {
                    throw new AssetStorageException(e.Message, e);
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
		protected virtual void MapFileSystemBlobDirectory2Folder(string directory, Folder folder)
		{
			var dInfo = new DirectoryInfo(directory);
            
			//Copy properties
			folder.LastModified = dInfo.LastWriteTimeUtc;
			//Convert folder uri to Name, and ParentId properties
			folder.FolderId = Relative(dInfo.FullName);
			folder.Name = dInfo.Name;
			folder.ParentFolderId = dInfo.Parent != null ? Relative(dInfo.Parent.FullName) : "";
		}

		protected virtual async void MapFileSystemBlob2FolderItem(string localBlob, FolderItem folderItem, bool fillSmallData)
		{
			var fInfo = new FileInfo(localBlob);

			//Copy properties
			folderItem.FolderItemId = Relative(fInfo.FullName);
			folderItem.FolderId = fInfo.DirectoryName;
			folderItem.Name = fInfo.Name;
			folderItem.LastModified = fInfo.LastWriteTimeUtc;
			folderItem.ContentLength = fInfo.Length;

			if (fillSmallData)
			{
				//Read small data
			    using (var memoryStream = new MemoryStream())
			    {
			        try
			        {
			            using (FileStream fileStream = File.OpenRead(localBlob))
			            {
			                memoryStream.SetLength(fileStream.Length);
			                await fileStream.CopyToAsync(memoryStream);
                            
			                //fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
			            }
			            folderItem.SmallData = memoryStream.ToArray();
			        }
			        catch
			        {
			            //TODO exception
			        }
			    }
			}
		}

		#endregion

		#region Private methods

		private string _RootFolder = String.Empty;
		private string RootFolder
		{
			get
			{
				if (!String.IsNullOrEmpty(_RootFolder))
				{
					return _RootFolder;
				}

				// check if current folder uses relative path
				if (_storagePath != null && _storagePath.StartsWith("~"))
				{
					if (HttpContext.Current != null)
					{
						var path = HttpContext.Current.Server.MapPath(_storagePath);
						_RootFolder = path;
						return _RootFolder;
					}
				}

				_RootFolder = _storagePath;
				return _RootFolder;
			}
		}

		/// <summary>
		/// Absolutes the specified relative url.
		/// </summary>
		/// <param name="relative">The relative.</param>
		/// <returns></returns>
		private string Absolute(string relative)
		{
			var filePath = HttpContext.Current.Request.MapPath(relative, _storagePath, false);
			return filePath;
		}

		private string Relative(string fullName)
		{
			var folder = RootFolder;
			var ret = fullName.Replace(RootFolder, String.Empty);

			if (ret.StartsWith("\\"))
			{
				ret = ret.Substring(1);
			}

			return ret;
		}

		private ObservableChangeTracker CreateChangeTracker()
		{
			var retVal = new ObservableChangeTracker
			{
                AddAction = (x) =>
                {
                    var entry = this.ChangeTracker.GetTrackingEntry(x);
                    this._delayedActions.Add(() => this.SaveEntryChanges(entry));
                },
                UpdateAction = (x) =>
                {
                    var entry = this.ChangeTracker.GetTrackingEntry(x);
                    entry.EntryState = EntryState.Detached;
                    var clonedEntry = new TrackingEntry
                    {
                        Entity = x.DeepClone(new AssetEntityFactory()),
                        EntryState = EntryState.Modified
                    };
                    this._delayedActions.Add(() => this.SaveEntryChanges(clonedEntry));
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
			                ((Folder) target).ParentFolderId = folder.FolderId;
			            }
			        }
			        else if (folder != null && property == "FolderItems")
			        {
			            if (folder.FolderId != null)
			            {
			                ((FolderItem) target).FolderId = folder.FolderId;
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

	    private void SaveEntryChanges(TrackingEntry entry)
	    {
            if (entry.EntryState == EntryState.Added)
            {
            }

            if ((entry.EntryState & (EntryState.Unchanged | EntryState.Detached)) != entry.EntryState)
            {
                if (entry.Entity is Folder)
                {
                    var folder = entry.Entity as Folder;
 
                    switch (entry.EntryState)
                    {
                        case EntryState.Modified:
                            var oldFolderPath = Absolute(folder.FolderId);
                            var newFolderPath = Path.Combine(Path.GetDirectoryName(oldFolderPath)??String.Empty, folder.Name);
                            Directory.Move(oldFolderPath, newFolderPath);
                            break;
                        case EntryState.Added:
                            var newFolder = Path.Combine(Absolute(folder.ParentFolderId), folder.Name);
                            Directory.CreateDirectory(newFolder);
                            break;
                        case EntryState.Deleted:
                            var folderPathToRemove = Absolute(folder.FolderId);
                            Directory.Delete(folderPathToRemove, true);
                            break;
                    }
                }
                else if (entry.Entity is FolderItem)
                {
                    var folderItem = entry.Entity as FolderItem;

                    switch (entry.EntryState)
                    {
                        case EntryState.Added:
                        case EntryState.Modified:

                            break;
                        case EntryState.Deleted:
                            var item = Absolute(folderItem.FolderItemId);
                            File.Delete(item);
                            var thumbFile = GenerateThumbnailPath(item);
                            if (File.Exists(thumbFile))
                            {
                                File.Delete(thumbFile);
                            }
                            break;
                    }
                }
            }
	    }

	    private static void GenerateNewImage(Image source, int width, int height, string fileName)
		{
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
				target.Save(fileName, encoderJpeg, parameters);
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
