using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces
{
	public interface IPickAssetViewModel : IViewModel
	{
		FolderItem SelectedAsset { get; }
        string RootItemId { get; set; }
        bool AssetPickMode { get; set; }
	    bool Validate();
	}
}
