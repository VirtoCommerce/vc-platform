using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
	public class LineItemViewModel : ViewModelBase, ILineItemViewModel
	{
		private Item _itemToAdd;

		public LineItemViewModel()
		{
			Quantity = decimal.One;
			//var colors = new string[] { "Black", "Green", "Silver", "Pink", "White" };
			//AvailableColors = new ObservableCollection<string>(colors);
			//SelectedColor = AvailableColors.FirstOrDefault();

			//var sizes = new string[] { "S", "M", "L", "XL", "XXL", "Other" };
			//AvailableSizes = new ObservableCollection<string>(sizes);
			//SelectedSize = AvailableSizes.FirstOrDefault();
		}

		#region ILineItemViewModel Members
		public Item ItemToAdd
		{
			get { return _itemToAdd; }
			set
			{
				_itemToAdd = value;
				ItemDisplayName = _itemToAdd.Name;
				MaxQuantity = _itemToAdd.MaxQuantity;
			}
		}

		public decimal Quantity
		{
			get;
			set;
		}

		public void Initialize(ShipmentItem originalItem)
		{
			ItemDisplayName = originalItem.LineItem.DisplayName;
			MaxQuantity = originalItem.Quantity;
		}

		//public string SelectedColor
		//{
		//    get;
		//    set;
		//}

		//public ObservableCollection<string> AvailableColors
		//{
		//    get;
		//    private set;
		//}

		//public string SelectedSize { get; set; }

		//public ObservableCollection<string> AvailableSizes { get; private set; }

		#endregion

		public string ItemDisplayName { get; private set; }
		public decimal MaxQuantity { get; private set; }
	}
}
