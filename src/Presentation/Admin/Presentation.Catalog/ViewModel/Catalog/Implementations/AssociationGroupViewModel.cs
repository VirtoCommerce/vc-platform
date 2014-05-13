using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class AssociationGroupViewModel : ViewModelBase, IAssociationGroupViewModel
	{
		#region Dependencies

		private readonly ICatalogEntityFactory _entityFactory;
		private readonly IItemViewModel _parent;
		private readonly IViewModelsFactory<IAssociationViewModel> _vmFactory;

		#endregion

		public AssociationGroupViewModel(
			IViewModelsFactory<IAssociationViewModel> vmFactory, 
			ICatalogEntityFactory entityFactory, 
			AssociationGroup item, 
			IItemViewModel parent)
		{
			InnerItem = item;
			_vmFactory = vmFactory;
			_entityFactory = entityFactory;
			_parent = parent;

			ItemAddCommand = new DelegateCommand(RaiseItemAddInteractionRequest);
			ItemEditCommand = new DelegateCommand<Association>(RaiseItemEditInteractionRequest, x => x != null);
			ItemRemoveCommand = new DelegateCommand<Association>(RaiseItemRemoveInteractionRequest, x => x != null);

			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<Association> ItemEditCommand { get; private set; }
		public DelegateCommand<Association> ItemRemoveCommand { get; private set; }

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IAssociationGroupViewModel

		public AssociationGroup InnerItem { get; private set; }

		public bool Validate()
		{
			InnerItem.Validate();

			//if (InnerItem.IsEnum && InnerItem.PropertyValues.Count == 0)
			//    InnerItem.SetError("Values", "Dictionary values must be defined", true);
			//else
			//    InnerItem.ClearError("Values");

			return InnerItem.Errors.Count == 0;
		}

		#endregion

		public void RaiseCanExecuteChanged()
		{
			ItemEditCommand.RaiseCanExecuteChanged();
			ItemRemoveCommand.RaiseCanExecuteChanged();
		}

		#region private members
		private void RaiseItemAddInteractionRequest()
		{
			var item = (Association)_entityFactory.CreateEntityForType("Association");
			item.Priority = 1;
			if (RaiseItemEditInteractionRequest(item, "Add association".Localize()))
			{
				item.AssociationGroupId = InnerItem.AssociationGroupId;
				InnerItem.Associations.Add(item);
			}
		}

		private void RaiseItemEditInteractionRequest(Association originalItem)
		{
			var item = originalItem.DeepClone(_entityFactory as CatalogEntityFactory);
			if (RaiseItemEditInteractionRequest(item, "Edit association".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
				// fake assign for UI triggers to display updated values.
				originalItem.ItemId = originalItem.ItemId;
			}
		}

		private bool RaiseItemEditInteractionRequest(StorageEntity item, string title)
		{
			var result = false;
			var itemVM = _vmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", _parent)
				);
			var confirmation = new ConditionalConfirmation(itemVM.Validate) {Title = title, Content = itemVM};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaiseItemRemoveInteractionRequest(Association originalItem)
		{
			InnerItem.Associations.Remove(originalItem);
		}

		#endregion

	}
}
