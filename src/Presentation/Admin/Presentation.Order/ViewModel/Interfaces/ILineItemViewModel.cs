using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
	public interface ILineItemViewModel : IViewModel
	{
		Item ItemToAdd { get; set; }
		decimal Quantity { get; set; }
		void Initialize(ShipmentItem selectedItem);
	}
}
