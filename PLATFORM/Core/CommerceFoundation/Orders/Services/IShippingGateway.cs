using System.ServiceModel;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Foundation.Orders.Services
{
    [ServiceContract]
    public interface IShippingGateway
    {
        [OperationContract]
        ShippingRate GetRate(string shippingMethod, LineItem[] items, ref string message);
    }
}
