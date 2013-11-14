using System;
using VirtoCommerce.ManagementClient.Asset.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.Foundation.Assets.Model;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations
{
    public class FileSearchViewModel : AssetEntitySearchViewModelBase, IFileSearchViewModel
    {
        internal const string _imageSource = "/VirtoCommerce.ManagementClient.Asset;component/Resources/images/file.png";

        public FileSearchViewModel(FolderItem folder)
            : base(null)
        {
            InnerItem = folder;
        }

        #region IFolderSearchViewModel
        public FolderItem InnerItem { get; private set; }
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
                return InnerItem.Name;
            }
        }

        public override string Size
        {
            get { return InnerItem.ContentLength.ToString(); }
        }

        public override DateTime? Modified
        {
            get { return InnerItem.LastModified; }
        }

        public override AssetType Type
        {
            get { return AssetType.File; }
        }

        public override string InnerItemID
        {
            get { return InnerItem.FolderItemId; }
        }
        #endregion

    }
}
