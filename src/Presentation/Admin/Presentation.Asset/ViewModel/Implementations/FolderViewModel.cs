using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Services;
using System;
using VirtoCommerce.ManagementClient.Asset.Model;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations
{
	public class FolderViewModel : AssetEntityViewModelBase, IFolderViewModel, IHierarchy
	{
		internal const string _imageSource = "/VirtoCommerce.ManagementClient.Asset;component/Resources/images/folder.png";

		public FolderViewModel(IAssetService repository, Folder folder)
			: base(repository)
		{
			CurrentFolder = folder;
			EmbeddedHierarchyEntry = this;

			OpenItemCommand = new DelegateCommand(DoOpenFolder);
		}

		#region IFolderViewModel
		public Folder CurrentFolder
		{
			get;
			protected set;
		}

		#endregion

		#region base overrides
		public override string IconSource
		{
			get
			{
				return _imageSource;
			}
		}

		public override string DisplayName
		{
			get
			{
				return CurrentFolder.Name;
			}
		}

		public override DateTime Created
		{
			get { return CurrentFolder.LastModified ?? DateTime.UtcNow; }
		}

		public override DateTime? Modified
		{
			get { return CurrentFolder.LastModified; }
		}

		protected override IViewModel CreateChildrenModel(object children)
		{
			var folderChild = children as FolderExt;
			return folderChild != null ? folderChild.ViewModel : children as IViewModel;
		}

		#endregion

		#region IHierarchy
		public void AddChild(object parent, object child)
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(object item)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<object> GetChildren(object item, int startIndex, int endIndex)
		{
			return Repository.GetChildrenFolders(CurrentFolder.FolderId);
		}

		public IEnumerable<object> GetChildren(object item)
		{
			return GetChildren(item, 0, -1);
		}

		public object GetParent(object child)
		{
			return Parent;
		}

		public bool IsLeaf(object item)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(object child)
		{
			throw new System.NotImplementedException();
		}

		public void SetLeaf(object item, bool leaf)
		{
			throw new System.NotImplementedException();
		}

		public void SetParent(object child, object parent)
		{
			throw new System.NotImplementedException();
		}

		public object Root
		{
			get { throw new System.NotImplementedException(); }
		}

		public object Item
		{
			get { return this; }
		}
		#endregion

		#region private members
		private void DoOpenFolder()
		{
			IsExpanded = true;
			IsSelected = true; // needed if it was expanded already
		}

		#endregion
	}
}
