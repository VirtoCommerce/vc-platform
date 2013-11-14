using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ICatalogViewModel : IViewModel
	{
		ObservableCollection<CatalogLanguageDisplay> AllAvailableLanguages { get; }
	}
}
