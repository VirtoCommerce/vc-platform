using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class ItemRelationViewModel : ViewModelBase, IItemRelationViewModel
	{
		private readonly IViewModelsFactory<ISearchItemViewModel> _vmFactory;
		private readonly IItemViewModel Parent;

		public ItemRelationViewModel(IViewModelsFactory<ISearchItemViewModel> vmFactory, ItemRelation item, IItemViewModel parent)
		{
			_vmFactory = vmFactory;
			InnerItem = item;
			Parent = parent;

			ItemPickCommand = new DelegateCommand(RaiseItemPickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public DelegateCommand ItemPickCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IItemRelationViewModel

		public ItemRelation InnerItem { get; private set; }

		public bool Validate()
		{
			return InnerItem.Validate();
		}

		#endregion

		#region ViewModelBase overrides
		public override string DisplayName
		{
			get
			{
				return InnerItem.ChildItem == null ? string.Empty : InnerItem.ChildItem.Name;
			}
		}
		#endregion

		private void RaiseItemPickInteractionRequest()
		{
			var itemVM = _vmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("catalogInfo", Parent.InnerItem.Catalog)
				, new KeyValuePair<string, object>("searchType", string.Empty)
				);
			itemVM.ExcludeItemId = Parent.InnerItem.ItemId;
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select an item".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						var selectedItem = itemVM.SelectedItem;
						if (Parent.ItemRelations.InnerItems.Any(x1 => x1.ChildItemId == selectedItem.ItemId))
						{
							selectedItem = Parent.ItemRelations.InnerItems.First(x1 => x1.ChildItemId == selectedItem.ItemId).ChildItem;
						}
						InnerItem.ChildItem = selectedItem;
						InnerItem.ChildItemId = selectedItem.ItemId;
						OnPropertyChanged("DisplayName");
					}
				});
		}
	}
}
