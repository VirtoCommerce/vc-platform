using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces
{
    public interface IPriceListAssignmentHomeViewModel : IViewModel
    {
        string SearchFilterKeyword { get; }
        string SearchFilterName { get;}

        DelegateCommand ClearFiltersCommand { get; }

    }
}
