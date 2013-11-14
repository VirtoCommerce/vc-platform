using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public interface IItemTypeSelectionStepViewModel : IViewModel
	{
		string SelectedItemType { get; }
	}

	public class ItemTypeSelectionStepViewModel : ViewModelBase, IItemTypeSelectionStepViewModel
	{
		public ItemTypeSelectionStepViewModel(ItemTypeSelectionModel[] allAvailableOptions)
		{
			SearchFilterItemTypes = allAvailableOptions;
			SelectedItemType = SearchFilterItemTypes[0].Value;
			OnPropertyChanged("SelectedItemType");
		}

		public string SelectedItemType { get; set; }
		public ItemTypeSelectionModel[] SearchFilterItemTypes { get; private set; }

	}

	public class ItemTypeSelectionModel
	{

		public ItemTypeSelectionModel(string value, string description = null)
		{
			Value = value;
			Description = description;
		}

		public string Value { get; set; }

		public string Description { get; set; }
	}
}
