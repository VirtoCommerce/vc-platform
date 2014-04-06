using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
	class RoleHomeViewModel : ViewModelHomeEditableBase<Role>, IRoleHomeViewModel, IVirtualListLoader<IRoleViewModel>, ISupportDelayInitialization
	{
		#region Private Dependencies

		private readonly IViewModelsFactory<IRoleViewModel> _itemVmFactory;
		private readonly IViewModelsFactory<ICreateRoleViewModel> _wizardVmFactory;
		private readonly IRepositoryFactory<ISecurityRepository> _repositoryFactory;

		#endregion

		public RoleHomeViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, IViewModelsFactory<IRoleViewModel> itemVmFactory, IViewModelsFactory<ICreateRoleViewModel> wizardVmFactory)
		{
			_itemVmFactory = itemVmFactory;
			_wizardVmFactory = wizardVmFactory;
			_repositoryFactory = repositoryFactory;
		}

		#region ViewModelHomeEditableBase

		protected override bool CanItemAddExecute()
		{
			return true;
		}

		protected override bool CanItemDeleteExecute(IList x)
		{
			return x != null && x.Count > 0;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = new Role();

			var itemVM = _wizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new Confirmation { Title = "Create role".Localize(), Content = itemVM };
			ItemAdd(confirmation);
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IRoleViewModel>>();
			ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
		}

		#endregion


		#region IVirtualListLoader<ICustomerServiceDetailViewModel> Members

		public bool CanSort
		{
			get { return true; }
		}

		public IList<IRoleViewModel> LoadRange(int startIndex, int count, System.ComponentModel.SortDescriptionCollection sortDescriptions, out int overallCount)
		{

			var retVal = new List<IRoleViewModel>();

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var query = repository.Roles;

				if (!string.IsNullOrEmpty(SearchKeyword))
				{
					query = query.Where(c => c.Name.Contains(SearchKeyword));
				}

				overallCount = query.Count();

				var results = query.OrderBy(c => c.RoleId).Skip(startIndex).Take(count).ToList();

				retVal.AddRange(results.Select(item => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item))));
			}

			return retVal;
		}

		#endregion


		#region IRoleHomeViewModel

		private string _searchKeyword = string.Empty;
		public string SearchKeyword
		{
			get { return _searchKeyword; }
			set { _searchKeyword = value; }

		}

		private ICollectionView _selectedItems;
		public ICollectionView SelectedItems
		{
			get
			{
				return _selectedItems;
			}
			private set
			{
				_selectedItems = value;
				OnPropertyChanged();
			}
		}

		#endregion


		#region ISupportdelayInitialization

		public void InitializeForOpen()
		{
			if (ListItemsSource == null)
			{
				OnUIThread(() => ListItemsSource = new VirtualList<IRoleViewModel>(this, 20, SynchronizationContext.Current));
			}
		}

		#endregion

	}
}
