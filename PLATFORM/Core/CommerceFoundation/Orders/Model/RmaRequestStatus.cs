
namespace VirtoCommerce.Foundation.Orders.Model
{
	public enum RmaRequestStatus
	{
		Complete = 1,
		Canceled = 2,
		AwaitingStockReturn = 4,
		AwaitingCompletion = 8
	}

}
