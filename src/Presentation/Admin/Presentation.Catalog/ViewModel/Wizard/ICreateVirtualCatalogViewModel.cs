using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public interface ICreateVirtualCatalogViewModel : IViewModel
    {
    }
    
    public interface IVirtualCatalogOverviewStepViewModel : IWizardStep
    {
        ObservableCollection<CatalogLanguageDisplay> AllAvailableLanguages { get; }
    }
}
