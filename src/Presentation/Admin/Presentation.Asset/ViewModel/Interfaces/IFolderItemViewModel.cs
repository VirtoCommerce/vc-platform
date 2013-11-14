using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Assets.Model;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces
{
    public interface IFolderItemViewModel : IViewModel
    {
        FolderItem CurrentFolderItem { get; }
        IViewModel Parent { get; set; }
    }
}
