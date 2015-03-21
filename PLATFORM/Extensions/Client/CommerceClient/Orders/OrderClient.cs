using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.StateMachines;

namespace VirtoCommerce.Client
{
	public class OrderClient
	{
		#region Private Variables

		private readonly ICacheRepository _cacheRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderStateController _stateController;

		#endregion

		private CacheHelper _cacheHelper;

		public OrderClient(IOrderStateController stateController, IOrderRepository orderRepository,
		                   ICacheRepository cacheRepository)
		{
			_stateController = stateController;
			_orderRepository = orderRepository;
			_cacheRepository = cacheRepository;
		}

		public IOrderStateController StateController
		{
			get { return _stateController; }
		}

		public CacheHelper Helper
		{
			get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
		}

		public bool CanChangeStatus(Order order, string newStatus)
		{
			return CanChangeStatus(order.Status, newStatus, _stateController.OrderStateMachine);
		}

		public bool CanChangeStatus(RmaRequest request, string newStatus)
		{
			return CanChangeStatus(request.Status, newStatus, _stateController.RmaStateMachine);
		}

		public bool CanChangeStatus(Shipment shipment, string newStatus)
		{
			return CanChangeStatus(shipment.Status, newStatus, _stateController.ShipmentStateMachine);
		}

		private bool CanChangeStatus(string oldStatus, string newStatus, IStateMachine<string> stateMachine)
		{
			var retVal = string.IsNullOrEmpty(oldStatus)
			             || (oldStatus != newStatus
			                 && stateMachine.IsTransitionAvailable(oldStatus, newStatus));

			return retVal;
		}

		public static OrderAddress FindAddressById(OrderGroup group, string addressId)
		{
			return String.IsNullOrEmpty(addressId)
				       ? null
				       : @group.OrderAddresses.FirstOrDefault(
					       address => address.OrderAddressId.Equals(addressId, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		///     Finds the name of the address by.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static OrderAddress FindAddressByName(OrderGroup group, string name)
		{
			return string.IsNullOrEmpty(name)
				       ? null
				       : @group.OrderAddresses.FirstOrDefault(
					       address => address.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}

		public static Address ConvertToCustomerAddress(OrderAddress address)
		{
			var addr = new Address();
			addr.InjectFrom(address);
			return addr;
		}

		public static OrderAddress ConvertToOrderAddress(Address address)
		{
			var addr = new OrderAddress();
			addr.InjectFrom(address);
			return addr;
		}

        public IQueryable<Order> GetAllCustomerOrders(string customerId, string storeId, int? limit = null)
		{
			var orders = _orderRepository.Orders.Where(x => (x.StoreId == storeId) && (x.CustomerId == customerId));
            if (limit.HasValue)
            {
                orders = orders.Take(limit.Value);
            }

            return (!orders.Any()) ? null : orders;
		}

		public void SaveChanges()
		{
			_orderRepository.UnitOfWork.Commit();
		}

		public Order GetCustomerOrder(string customerId, string orderId)
		{
			var order = _orderRepository.Orders
			                            .ExpandAll()
			                            .FirstOrDefault(x => x.OrderGroupId == orderId && x.CustomerId == customerId);
			return order;
		}

        /// <summary>
        /// Gets the order by authentication code. Used by Paypal.
        /// </summary>
        /// <param name="authCode">The authentication code.</param>
        /// <returns></returns>
	    public OrderGroup GetOrderByAuthCode(string authCode)
	    {
            var payment = _orderRepository.Payments.Where(x => x.AuthorizationCode == authCode)
                .Expand(p => p.OrderForm.OrderGroup.OrderAddresses)
                .Expand(p=>p.OrderForm.LineItems)
                .Expand("OrderForm.Shipments.ShipmentItems").FirstOrDefault();
            return payment != null ? payment.OrderForm.OrderGroup : null;
	    }
	}
}