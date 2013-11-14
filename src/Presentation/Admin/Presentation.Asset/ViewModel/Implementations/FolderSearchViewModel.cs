using System;
using VirtoCommerce.ManagementClient.Asset.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.Foundation.Assets.Model;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations
{
   
    public class FolderSearchViewModel : AssetEntitySearchViewModelBase, IFolderSearchViewModel
    {
        public FolderSearchViewModel(Folder folder, AssetEntitySearchViewModelBase parent)
            : base(parent)
        {
            InnerItem = folder;
            _Type = (Parent is RootSearchViewModel) ? AssetType.Container : AssetType.Folder;
        }

        #region IFolderSearchViewModel
        public Folder InnerItem { get; private set; }
        #endregion

        #region base overrides
        public override string IconSource
        {
            get
            {
                string result = (Type == AssetType.Folder) ? "folder" : "container";
                result = string.Format("/VirtoCommerce.ManagementClient.Asset;component/Resources/images/{0}.png", result);
                return result;
            }
        }

        public override string DisplayName
        {
            get
            {
                return InnerItem.Name;
            }
        }

        public override DateTime? Modified
        {
            get { return InnerItem.LastModified; }
        }

        AssetType _Type;
        public override AssetType Type
        {
            get { return _Type; }
        }

        public override string InnerItemID
        {
            get { return InnerItem.FolderId; }
        }

        #endregion

    }
}
