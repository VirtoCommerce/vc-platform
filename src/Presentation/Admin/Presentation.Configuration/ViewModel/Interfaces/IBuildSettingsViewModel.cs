using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Search.Model;

namespace VirtoCommerce.ManagementClient.Configuration.ViewModel.Interfaces
{
    public interface IBuildSettingsViewModel : IViewModel
    {
        DelegateCommand<BuildSettings> ItemRebuildCommand { get; }
        InteractionRequest<Confirmation> CommonConfirmRequest { get; }

        ObservableCollection<BuildSettings> Items { get; }
		bool IsActive { get; set; }
    }
}
