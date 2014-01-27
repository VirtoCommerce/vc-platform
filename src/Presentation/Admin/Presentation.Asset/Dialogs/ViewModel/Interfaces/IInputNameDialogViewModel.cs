using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel.Interfaces
{
    public interface IInputNameDialogViewModel : IViewModel
    {
        string InputText { get; set; }
        string InputLabel { get; set; }
    }
}
