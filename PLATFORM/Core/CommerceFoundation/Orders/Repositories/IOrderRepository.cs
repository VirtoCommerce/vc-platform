using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.Foundation.Orders.Repositories
{
    public interface IOrderRepository : IRepository
    {
        IQueryable<Order> Orders { get; }
        IQueryable<ShoppingCart> ShoppingCarts { get; }
        IQueryable<Shipment> Shipments { get; }
		IQueryable<RmaRequest> RmaRequests { get; }
        IQueryable<LineItem> LineItems { get; }
        IQueryable<OrderAddress> OrderAddresses { get; }
        IQueryable<Payment> Payments { get; }
        IQueryable<Jurisdiction> Jurisdictions { get; }
        IQueryable<JurisdictionGroup> JurisdictionGroups { get; }
    }
}
