using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
	public class LineItemAddViewModel : ViewModelBase, ILineItemAddViewModel, IMultiSelectControlCommands
	{
		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
		private readonly IViewModelsFactory<ILineItemViewModel> _lineItemVmFactory;

		private bool _searched;
		private int _totalCount;

		public List<Foundation.Catalogs.Model.Catalog> AvailableCatalogs { get; private set; }
		public string SelectedCatalogId { get; set; }

		public string SearchSKUText { get; set; }
		public DelegateCommand SearchLineItemCommand { get; private set; }

		#region constructor
#if DESIGN
		public LineItemAddViewModel()
		{
			Initialize();
		}
#endif

		public LineItemAddViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<ILineItemViewModel> lineItemVmFactory)
		{
			_repositoryFactory = repositoryFactory;
			_lineItemVmFactory = lineItemVmFactory;

			ItemDetailsConfirmRequest = new InteractionRequest<Confirmation>();
			SearchLineItemCommand = new DelegateCommand(RaiseSearchLineItem);

			Initialize();
		}

		#endregion

		#region ILineItemAddViewModel Members
		public InteractionRequest<Confirmation> ItemDetailsConfirmRequest { get; private set; }

		public ObservableCollection<Item> AvailableItems { get; private set; }
		public ObservableCollection<ILineItemViewModel> SelectedItemsToAdd { get; private set; }

		public string AvailableTitle
		{
			get
			{
				var result = "Available items".Localize();
				if (_searched)
				{
					result += string.Format(" ({0} of {1} total)".Localize(), AvailableItems.Count, _totalCount);
				}
				return result;
			}
		}

		#endregion

		#region IMultiSelectControlCommands Members
		public void SelectItem(object selectedObj)
		{
			var itemVM = _lineItemVmFactory.GetViewModelInstance();
			itemVM.ItemToAdd = selectedObj as Item;

			var confirmation = new ConditionalConfirmation();
			confirmation.Title = "Specify item details".Localize();
			confirmation.Content = itemVM;
			ItemDetailsConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					// ReturnBuilder.AddExchangeItem(itemVM.ItemToAdd, itemVM.Quantity, itemVM.SelectedColor);
					// OrderBuilder.AddLineItem(...)
					SelectedItemsToAdd.Add(itemVM);
				}
			});
			//OnPropertyChanged("ReturnTotal");
		}

		public void UnSelectItem(object selectedObj)
		{
			var selectedItem = selectedObj as ILineItemViewModel;
			//ReturnBuilder.RemoveReturnItem(selectedItem, ...);
			SelectedItemsToAdd.Remove(selectedItem);
			//OnPropertyChanged("ReturnTotal");
		}

		public void SelectAllItems(System.ComponentModel.ICollectionView availableItemsCollectionView)
		{
		}

		public void UnSelectAllItems(System.Collections.IList currentListItems)
		{
			if (null != currentListItems)
			{
				currentListItems.Clear();
			}
		}

		#endregion

		#region private members
		private void Initialize()
		{
			AvailableItems = new ObservableCollection<Item>();
			SelectedItemsToAdd = new ObservableCollection<ILineItemViewModel>();

			using (var _catalogRepository = _repositoryFactory.GetRepositoryInstance())
			{
				AvailableCatalogs = _catalogRepository.Catalogs.OfType<Foundation.Catalogs.Model.Catalog>().OrderBy(x => x.Name).ToList();
				SelectedCatalogId = AvailableCatalogs[0].CatalogId;
			}

			SearchSKUText = string.Empty;
		}

		private void RaiseSearchLineItem()
		{
			using (var _catalogRepository = _repositoryFactory.GetRepositoryInstance())
			{
				var query = _catalogRepository.Items
					.Where(x => x.CatalogId == SelectedCatalogId);

				if (!string.IsNullOrEmpty(SearchSKUText))
				{
					query = query.Where(x => x.Name.Contains(SearchSKUText) || x.ItemId.Contains(SearchSKUText) || x.Code.Contains(SearchSKUText));
				}

				_totalCount = query.Count();
				var products = query.OrderBy(x => x.Name).Take(15);
				AvailableItems.SetItems(products);
			}

			_searched = true;
			OnPropertyChanged("AvailableTitle");
		}

		#endregion

	}
}
