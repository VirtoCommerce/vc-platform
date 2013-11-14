using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces
{
    public interface IPriceListHomeViewModel : IViewModel
    {
		string SearchFilterKeyword { get; set; }
		string SearchFilterName { get; set; }
		string SearchFilterCurrency { get; set; }

		DelegateCommand ClearFiltersCommand { get; }

	    DelegateCommand NewPriceListNavigate();

    }
}
