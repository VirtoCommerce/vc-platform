using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ICategoryViewModel : IViewModel
	{
		IEnumerable<PropertySet> AvailableCategoryTypes { get; }
		DelegateCommand<object> PropertyValueEditCommand { get; }
		DelegateCommand<object> PropertyValueDeleteCommand { get; }
	}
}
