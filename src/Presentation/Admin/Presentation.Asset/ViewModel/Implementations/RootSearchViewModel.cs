using System;
using VirtoCommerce.ManagementClient.Asset.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations
{
    
    public class RootSearchViewModel : AssetEntitySearchViewModelBase, IRootSearchViewModel
    {
        internal const string _imageSource = "/VirtoCommerce.ManagementClient.Asset;component/Resources/images/arrow_up.png";

        public RootSearchViewModel(AssetEntitySearchViewModelBase parentItem)
            : base(parentItem)
        {
        }

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
            get { return ".."; }
        }

        public override DateTime? Modified
        {
            get { return null; }
        }

        public override string DisplayedType { get { return string.Empty; } }

        public override AssetType Type
        {
            get { return AssetType.Parent; }
        }

        public override string InnerItemID
        {
            get { return Parent == null ? null : Parent.InnerItemID; }
        }

        #endregion
    }
}
