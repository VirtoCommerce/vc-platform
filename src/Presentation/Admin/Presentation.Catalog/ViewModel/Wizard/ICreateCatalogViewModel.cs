using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public interface ICreateCatalogViewModel : IViewModel
    {
    }

    public interface ICatalogOverviewStepViewModel : IWizardStep
    {
        ObservableCollection<CatalogLanguageDisplay> AllAvailableLanguages { get; }
    }
}
