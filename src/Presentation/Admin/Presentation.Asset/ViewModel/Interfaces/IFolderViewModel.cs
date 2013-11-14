using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Assets.Model;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces
{
    public interface IFolderViewModel : IViewModel
    {
        Folder CurrentFolder { get; }
        IViewModel Parent { get; set; }
    }
}
