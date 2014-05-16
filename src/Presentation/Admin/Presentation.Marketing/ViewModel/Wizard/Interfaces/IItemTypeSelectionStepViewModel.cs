using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces
{
	public interface IItemTypeSelectionStepViewModel : IViewModel
	{
		string SelectedItemType { get; }
		ItemTypeSelectionModel[] SearchFilterItemTypes { get; }
	}

	public class ItemTypeSelectionStepViewModel : ViewModelBase, IItemTypeSelectionStepViewModel
	{
		public ItemTypeSelectionStepViewModel()
		{
			SearchFilterItemTypes = new[]
				{
					new ItemTypeSelectionModel("Cart promotion","Cart promotions are used to encourage shoppers to increase their order size by providing incentives, such as free shipping on orders over a certain sum."),
					new ItemTypeSelectionModel("Catalog promotion","Catalog promotions are used to make specific products and categories of products more attractive to shoppers through incentives, such as lowered pricing on a particular brand.")
				};
			SearchFilterItemTypes.ToList().ForEach(x => { x.Value = x.Value.Localize(); x.Description = x.Description.Localize(); });

			SelectedItemType = SearchFilterItemTypes[0].Value;
			OnPropertyChanged("SelectedItemType");
		}

		public string SelectedItemType
		{
			get;
			set;
		}

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
