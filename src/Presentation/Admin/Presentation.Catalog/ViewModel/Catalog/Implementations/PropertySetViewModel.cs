using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class PropertySetViewModel : ViewModelBase, IPropertySetViewModel, IMultiSelectControlCommands
	{
		private ICatalogEntityFactory _entityFactory;

		public PropertySetViewModel(ICatalogEntityFactory entityFactory, PropertySet item, ObservableCollection<Property> properties)
		{
			_entityFactory = entityFactory;
			InnerItem = item;
			InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
			InnerItem.PropertyChanged += InnerItem_PropertyChanged;

			ItemsCollection = new CollectionChangeGeneral<PropertySetProperty>(InnerItem.PropertySetProperties);
			AllAvailableProperties = new ObservableCollection<Property>(properties);
			var view = CollectionViewSource.GetDefaultView(AllAvailableProperties);
			view.Filter = FilterProperties;

			TargetTypes = (Enum.GetValues(typeof(PropertyTargetType)).OfType<PropertyTargetType>().Select(x => x.ToString())).ToList();

			// data sorting in list
			var collView2 = CollectionViewSource.GetDefaultView(ItemsCollection.InnerItems);
			collView2.SortDescriptions.Add(new System.ComponentModel.SortDescription("Priority", System.ComponentModel.ListSortDirection.Ascending));
		}

		public CollectionChangeGeneral<PropertySetProperty> ItemsCollection { get; private set; }
		public ObservableCollection<Property> AllAvailableProperties { get; private set; }
		public IEnumerable<string> TargetTypes { get; private set; }

		#region IPropertySetViewModel

		public PropertySet InnerItem { get; private set; }

		public bool Validate()
		{
			InnerItem.Validate();

			ValidateItemTargetType();

			return InnerItem.Errors.Count == 0;
		}

		/// <summary>
		/// Gets the items collection. Implicitly converts CollectionChangeGeneral to ICollectionChange
		/// </summary>
		/// <returns></returns>
		public ICollectionChange<PropertySetProperty> GetItemsCollection()
		{
			return ItemsCollection;
		}

		#endregion

		#region IMultiSelectControlCommands
		public void SelectItem(object selectedObj)
		{
			SelectItem((Property)selectedObj);
		}

		public void SelectAllItems(System.ComponentModel.ICollectionView availableItemsCollectionView)
		{
			IList<Property> itemsList = new List<Property>(availableItemsCollectionView.Cast<Property>());
			foreach (Property obj in itemsList)
			{
				SelectItem(obj);
			}
		}

		public void UnSelectItem(object selectedObj)
		{
			var selectedItem = (PropertySetProperty)selectedObj;
			ItemsCollection.Remove(selectedItem);
		}

		public void UnSelectAllItems(System.Collections.IList currentListItems)
		{
			ItemsCollection.InnerItems.ToList().ForEach(x => UnSelectItem(x));
		}

		private void SelectItem(Property selectedItem)
		{
			var item = (PropertySetProperty)_entityFactory.CreateEntityForType("PropertySetProperty");
			item.PropertyId = selectedItem.PropertyId;
			item.Property = selectedItem;
			item.PropertySetId = InnerItem.PropertySetId;

			var maxPriority = ItemsCollection.InnerItems.Count == 0 ? 0 : ItemsCollection.InnerItems.Max(x => x.Priority);
			item.Priority = maxPriority + 1;

			ItemsCollection.Add(item);
		}

		#endregion

		#region private

		private void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "TargetType")
				ValidateItemTargetType();
		}

		private void ValidateItemTargetType()
		{
			if (!TargetTypes.Contains(InnerItem.TargetType))
				InnerItem.SetError("TargetType", "TargetType must be selected".Localize(), true);
			else
				InnerItem.ClearError("TargetType");
		}

		private bool FilterProperties(object item)
		{
			var filterItem = (Property)item;
			var result = ItemsCollection.InnerItems.All(x => x.PropertyId != filterItem.PropertyId);
			return result;
		}

		#endregion
	}
}
