using System;
using VirtoCommerce.ManagementClient.Asset.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel
{
    public abstract class AssetEntitySearchViewModelBase : ViewModelBase
    {
	    protected AssetEntitySearchViewModelBase(AssetEntitySearchViewModelBase parent)
        {
            Parent = parent;
        }

        public virtual string Size { get { return ""; } }
        public virtual DateTime? Modified { get { return null; } }
        public virtual string DisplayedType { get { return Type.ToString(); } }
        public abstract AssetType Type { get; }

        public abstract string InnerItemID { get; }
        public AssetEntitySearchViewModelBase Parent { get; private set; }
    }

}
