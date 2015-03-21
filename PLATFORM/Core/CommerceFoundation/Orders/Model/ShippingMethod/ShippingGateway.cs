using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Orders.Model.Gateways;

namespace VirtoCommerce.Foundation.Orders.Model.PaymentMethod
{
	[DataContract]
	// [EntitySet("ShippingGateways")]
    [EntitySet("Gateways")]
	public class ShippingGateway : Gateway
	{
	}
}
