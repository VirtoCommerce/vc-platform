using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections;
using VirtoCommerce.ManagementClient.Asset.Model;

namespace VirtoCommerce.ManagementClient.Asset.Repositories
{
    public class MockAssetRepository : IAssetRepository
    {
        private IList[] MockLists;

        private List<FolderExt> MockFolderList;
        private List<FolderItemExt> MockFolderItemList;

        public MockAssetRepository()
        {
            MockFolderList = new List<FolderExt>();
            MockFolderList.Add(new FolderExt {FolderId = "folder", Name = "main"});
            MockFolderList.Add(new FolderExt {FolderId = "backup", Name = "backups"});

            MockFolderList.Add(new FolderExt {FolderId = "subfolder", Name = "goods", ParentFolderId = "folder"});
            MockFolderList.Add(new FolderExt {FolderId = "1", Name = "Customers", ParentFolderId = "subfolder"});
            MockFolderList.Add(new FolderExt {FolderId = "a", Name = "items", ParentFolderId = "subfolder"});
            MockFolderList.Add(new FolderExt {FolderId = "t", Name = "sales", ParentFolderId = "1"});
            MockFolderList.Add(new FolderExt
            {
                FolderId = "i",
                Name = "marketing and advertising",
                ParentFolderId = "folder"
            });

            foreach (var folder in MockFolderList.ToArray())
            {
                for (int i = 0; i < folder.Name.Length; i++)
                {
                    MockFolderList.Add(new FolderExt
                    {
                        FolderId = folder.Name + i,
                        Name = folder.Name + "\\" + i,
                        ParentFolderId = folder.FolderId
                    });
                }
            }

            MockFolderItemList = new List<FolderItemExt>();
            var item = new FolderItemExt();
            //item.SmallData = new byte[] { 1, 2, 3, 4, 5, 6 };
            item.Name = "image.jpg";
            item.ContentType = "image";
            item.FolderId = "subfolder";

            MockFolderItemList.Add(item);
            var folderitem = new FolderItemExt
            {
                Name = "imageqw.jpg",
                ContentType = "image",
                FolderId = "folder",
                FolderItemId = "441584123544353"
            };

            MockFolderItemList.Add(folderitem);

            MockLists = new IList[] {MockFolderList, MockFolderItemList};
        }

        #region IAssetRepository

        public VirtoCommerce.Foundation.Assets.Model.Folder GetFolderById(string folderId)
        {
            return MockFolderList.FirstOrDefault(x => x.FolderId == folderId);
        }

        public VirtoCommerce.Foundation.Assets.Model.FolderItem GetFolderItemById(string itemId)
        {
            throw new NotImplementedException();
        }

        public Folder[] GetChildrenFolders(string folderId)
        {
            return MockFolderList.Where(x => (x.ParentFolderId == folderId)).ToArray();
        }

        public FolderItem[] GetChildrenFolderItems(string folderId)
        {
            return MockFolderItemList.Where(x => (x.FolderId == folderId)).ToArray();
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

        #endregion
        
		#region IRepository Members

		public VirtoCommerce.Foundation.Frameworks.IUnitOfWork UnitOfWork
		{
			get { throw new NotImplementedException(); }
		}

        public bool IsAttachTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }


		public void Attach<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

		public void Add<T>(T item) where T : class
		{
            var list = MockLists.OfType<List<T>>().First();

            if (!list.Contains(item))
                list.Add(item);
		}

		public void Update<T>(T item) where T : class
		{
			throw new NotImplementedException();
		}

		public void Remove<T>(T item) where T : class
		{
			throw new NotImplementedException();
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

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
