using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using System.Windows.Data;
using System.ComponentModel;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreLinkedStoresStepViewModel : StoreViewModel, IStoreLinkedStoresStepViewModel, IMultiSelectControlCommands
	{
		public StoreLinkedStoresStepViewModel(IStoreEntityFactory entityFactory, Store item,
			IRepositoryFactory<IStoreRepository> repositoryFactory)
			: base(repositoryFactory, entityFactory, item)
		{
		}

		public ObservableCollection<Store> AvailableStoreLinkedStores { get; private set; }

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
				return retval;
			}
		}

		public override bool IsLast
		{
			get
			{
				return true;
			}
		}

		public override string Description
		{
			get
			{
				return string.Empty;
			}
		}
		#endregion

		#region IMultiSelectControlCommands
		public void SelectItem(object selectedObj)
		{
			if (selectedObj is Store)
			{
				var item = EntityFactory.CreateEntity<StoreLinkedStore>();
				// item.Store = InnerItem;
				item.StoreId = InnerItem.StoreId;
				var selectedStore = (Store)selectedObj;
				item.LinkedStore = selectedStore;
				item.LinkedStoreId = selectedStore.StoreId;
				InnerItem.LinkedStores.Add(item);

			}
		}

		public void SelectAllItems(ICollectionView availableItemsCollectionView)
		{
			if (availableItemsCollectionView.SourceCollection is ICollection<Store>)
			{
				availableItemsCollectionView.Cast<Store>().ToList().ForEach(x => SelectItem(x));
			}
		}

		public void UnSelectItem(object selectedObj)
		{
			StoreLinkedStore selectedLinkedStore;

			// prevent removing default language
			if ((selectedLinkedStore = selectedObj as StoreLinkedStore) != null)
			{
				InnerItem.LinkedStores.Where(x => x.LinkedStoreId == selectedLinkedStore.LinkedStoreId).ToList().ForEach(x =>
				{
					InnerItem.LinkedStores.Remove(x);
				});


			}
		}

		public void UnSelectAllItems(System.Collections.IList currentListItems)
		{
			if (currentListItems is IList<StoreLinkedStore>)
			{
				InnerItem.LinkedStores.ToList().ForEach(x => UnSelectItem(x));
			}
		}

		private bool FilterLinkedStores(object item)
		{
			bool result = false;
			if (item is Store)
			{
				var itemTyped = (Store)item;
				result = InnerItem.LinkedStores.All(x => x.LinkedStoreId != itemTyped.StoreId);
			}
			return result;
		}

		#endregion

		protected override void InitializePropertiesForViewing()
		{
			// all Views must be set on UI thread!!
			if (Repository == null)
			{
				GetRepository();
			}

			AvailableStoreLinkedStores =new ObservableCollection<Store>(
				(Repository as IStoreRepository).Stores.Where(x => x.StoreId != InnerItem.StoreId).OrderBy(x => x.Name).ToList());

			OnUIThread(() =>
			{
				var view = CollectionViewSource.GetDefaultView(AvailableStoreLinkedStores);
				view.Filter = FilterLinkedStores;
			});
			OnPropertyChanged("AvailableStoreLinkedStores");
		}
	}
}
