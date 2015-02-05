using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Asset.Factories;
using VirtoCommerce.ManagementClient.Asset.Model;

namespace VirtoCommerce.ManagementClient.Asset.Services
{
	public class FileSystemRepository : IAssetRepository, IUnitOfWork
	{
		private List<Action> _delayedActions = new List<Action>();
		private readonly object _lockObject = new object();

		public ObservableChangeTracker ChangeTracker { get; private set; }
		public FileSystemRepository()
		{
			ChangeTracker = CreateChangeTracker();
		}
		/// <summary>
		/// Returns true if the given file path is a folder.
		/// </summary>
		/// <param name="Path">File path</param>
		/// <returns>True if a folder</returns>
		public static bool IsFolder(string path)
		{
			return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
		}

		#region IAssetRepository
		public Folder GetFolderById(string path)
		{
			var retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<FolderExt>().Where(x => x.FileSystemPath == path).FirstOrDefault();
			if (retVal == null)
			{
				if (Directory.Exists(path))
				{
					retVal = new FolderExt
					{
						Name = System.IO.Path.GetFileName(path),
						FolderId = path
					};
					retVal.FileSystemPath = path;
				}
				ChangeTracker.Attach(retVal);
			}
			return retVal;
		}

		public FolderItem GetFolderItemById(string path)
		{
			var retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<FolderItemExt>().Where(x => x.FileSystemPath == path).FirstOrDefault();
			if (retVal == null)
			{
				retVal = new FolderItemExt
				{
					Name = System.IO.Path.GetFileName(path),
					FolderItemId = path
				};
				retVal.FileSystemPath = path;

				ChangeTracker.Attach(retVal);
			}
			return retVal;
		}

		public Folder[] GetChildrenFolders(string folderId)
		{
			if (string.IsNullOrEmpty(folderId))
				folderId = "d:\\tmp\\tstRep";

			lock (_lockObject)
			{
				List<FolderExt> retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<FolderExt>().Where(x => x.ParentFolderId == folderId).ToList();
				if (!retVal.Any())
				{
					foreach (var path in Directory.GetDirectories(folderId))
					{
						var subFolder = new FolderExt
						{
							Name = System.IO.Path.GetFileName(path),
							FileSystemPath = path,
							FolderId = path,
							ParentFolderId = folderId,
							ParentFolder = GetFolderById(folderId)
						};

						retVal.Add(subFolder);

						ChangeTracker.Attach(subFolder);
					}
				}
				return retVal.ToArray();
			}
		}

		public FolderItem[] GetChildrenFolderItems(string folderId)
		{
			List<FolderItemExt> retVal = ChangeTracker.TrackingEntries.Select(x => x.Entity).OfType<FolderItemExt>().Where(x => x.FolderId == folderId).ToList();
			if (!retVal.Any())
			{
				foreach (var path in Directory.GetFiles(folderId))
				{
					FileInfo fi = new FileInfo(path);
					var folderItem = new FolderItemExt
					{
						FolderItemId = path,
						Name = System.IO.Path.GetFileName(path),
						FileSystemPath = path,
						FolderId = folderId,
						ContentLength = fi.Length
					};

					//// 500 KB
					//if (fi.Length > 512000)
					//{
					//	// blob association ?
					//	// folderItem.BlobKey = 
					//}
					//else
					//{
					//	using (FileStream fs = fi.OpenRead())
					//	{
					//		byte[] fileContent = new byte[fs.Length];
					//		fs.Read(fileContent, 0, fileContent.Length);
					//		folderItem.SmallData = fileContent;
					//	}
					//}

					retVal.Add(folderItem);
					ChangeTracker.Attach(folderItem);
				}
			}
			return retVal.ToArray();
		}


        public Folder CreateFolder(string folderName, string parentId = null)
        {
            return null;
        }

	    public void Delete(string id)
	    {
	        throw new NotImplementedException();
	    }

	    public void Rename(string id, string name)
	    {
	        throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			_delayedActions.ForEach(x => x());
			_delayedActions.Clear();

			ChangeTracker.MarkAllUnchanged();

			return 0;
		}

		public void CommitAndRefreshChanges()
		{
			Commit();
			ChangeTracker.Dispose();
			ChangeTracker = CreateChangeTracker();
		}

		public void RollbackChanges()
		{
			ChangeTracker.Dispose();
			ChangeTracker = CreateChangeTracker();
		}

		#endregion

		private void GenereateEntityKey(StorageEntity entity)
		{
			if (entity is Folder)
			{
				var folder = entity as Folder;

				folder.FolderId = folder.Name;
				if (folder.ParentFolderId != null)
				{
					folder.FolderId = Path.Combine(folder.ParentFolderId, folder.FolderId);
				}

			}
			else if (entity is FolderItem)
			{
				var folderItem = entity as FolderItem;

				folderItem.FolderItemId = folderItem.Name;
				if (folderItem.FolderId != null)
				{
					folderItem.FolderItemId = Path.Combine(folderItem.FolderId, folderItem.FolderItemId);
				}
			}
		}

		private void SaveEntryChanges(TrackingEntry entry)
		{
			if (entry.EntryState == EntryState.Added)
			{
				GenereateEntityKey(entry.Entity as StorageEntity);
			}
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
							Directory.CreateDirectory(folder.FolderId);
							break;
						case EntryState.Deleted:
							try
							{
								Directory.Delete(folder.FolderId, true);
							}
							catch (Exception)
							{
							}
							break;
					}
				}
				else if (entry.Entity is FolderItem)
				{
					var folderItem = entry.Entity as FolderItem;

					switch (entry.EntryState)
					{
						case EntryState.Added:
							using (var fs = File.Create(folderItem.FolderItemId))
								if (folderItem.SmallData != null)
									using (var binaryWriter = new BinaryWriter(fs))
									{
										binaryWriter.Write(folderItem.SmallData);
									}
							break;
						case EntryState.Modified:
							break;
						case EntryState.Deleted:
							try
							{
								File.Delete(folderItem.FolderItemId);
							}
							catch (Exception)
							{
							}
							break;
					}
				}
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion

		private ObservableChangeTracker CreateChangeTracker()
		{
			var retVal = new ObservableChangeTracker();

			retVal.AddAction = (x) =>
			{
				var entry = ChangeTracker.GetTrackingEntry(x);
				_delayedActions.Add(() => SaveEntryChanges(entry));
			};
			retVal.RemoveAction = (x) =>
			{
				var entry = ChangeTracker.GetTrackingEntry(x);
				entry.EntryState = EntryState.Detached;
				var clonedEntry = new TrackingEntry { Entity = x.DeepClone(new AssetEntityFactoryExt()), EntryState = EntryState.Deleted };
				_delayedActions.Add(() => SaveEntryChanges(clonedEntry));
			};

			retVal.AddNewOneToManyRelationAction = (source, property, target) =>
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
			};

			retVal.PropertyChangedAction = (source, property, target) =>
			{
				var folder = source as Folder;
				if (folder != null)
				{
					if (property == "FolderId")
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
    }
}
