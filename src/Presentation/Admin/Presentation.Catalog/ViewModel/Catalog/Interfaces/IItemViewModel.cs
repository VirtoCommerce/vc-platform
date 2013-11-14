using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface IItemViewModel : IViewModelDetailBase
	{
		Item InnerItem { get; }
		ICatalogRepository ItemRepository { get; }

		IEnumerable<PropertySet> AllAvailableItemTypes { get; }
		Packaging[] AvailablePackageTypes { get; }
		AvailabilityRule[] AvailableAvailabilityRules { get; }

		CollectionChangeGeneral<ItemRelation> ItemRelations { get; }

		DelegateCommand<object> PropertyValueEditCommand { get; }
		DelegateCommand<object> PropertyValueDeleteCommand { get; }

		DelegateCommand<object> AssociationGroupDeleteCommand { get; }
	}
}
