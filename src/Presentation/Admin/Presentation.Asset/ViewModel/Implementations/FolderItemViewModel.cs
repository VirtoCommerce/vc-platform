using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Services;
using System;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations
{
    public class FolderItemViewModel : AssetEntityViewModelBase, IFolderItemViewModel
    {
        internal const string _imageSource = "/VirtoCommerce.ManagementClient.Asset;component/Resources/images/file.png";

        public FolderItemViewModel(IAssetService repository, FolderItem folderItem)
			: base(repository)
        {
            CurrentFolderItem = folderItem;

            // OpenItemCommand = new DelegateCommand(() => DoOpenAsset());
        }

        #region IFolderItemViewModel
        public FolderItem CurrentFolderItem
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
                return CurrentFolderItem.Name;
            }
        }

        public override string Size
        {
            get
            {
                return CurrentFolderItem.ContentLength.ToString();
            }
        }

        public override DateTime Created
        {
            get
            {
                return CurrentFolderItem.LastModified ?? DateTime.UtcNow;
            }
        }
        
        public override DateTime? Modified
        {
            get { return CurrentFolderItem.LastModified; }
        }

        #endregion


    }
}
