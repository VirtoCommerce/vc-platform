using System.Activities;
using System.Linq;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.OrderWorkflow
{
    public class CreateOrderActivity : OrderActivityBase
    {

        IOrderRepository _orderRepository;
        protected IOrderRepository OrderRepository
		{
            get { return _orderRepository ?? (_orderRepository = ServiceLocator.GetInstance<IOrderRepository>()); }
			set
			{
                _orderRepository = value;
			}
		}

		ICustomerSessionService _customerSessionService;
        protected ICustomerSessionService CustomerSessionService
		{
			get
			{
				return _customerSessionService ??
					   (_customerSessionService = ServiceLocator.GetInstance<Foundation.Customers.Services.ICustomerSessionService>());
			}
			set
			{
				_customerSessionService = value;
			}
		}

        private SequencesClient _sequencesClient;
        protected SequencesClient SequencesClient
        {
            get
            {
                return _sequencesClient ??
                       (_sequencesClient = ServiceLocator.GetInstance<SequencesClient>());
            }
            set
            {
                _sequencesClient = value;
            }
        }

		public CreateOrderActivity()
		{
		}

        public CreateOrderActivity(IOrderRepository orderRepository,
            ICustomerSessionService customerService, 
            SequencesClient sequencesClient)
		{
            _orderRepository = orderRepository;
			_customerSessionService = customerService;
            _sequencesClient = sequencesClient;
		}


        protected override void Execute(CodeActivityContext context)
        {
            base.Execute(context);

            if (ServiceLocator == null)
                return;

            if (CurrentOrderGroup == null || CurrentOrderGroup.OrderForms.Count == 0)
                return;

            var customerName = CustomerSessionService.CustomerSession.CustomerName;

            //If user is anonymous take name from billing address
            if (string.IsNullOrEmpty(customerName))
            {
                var billingAddress = CurrentOrderGroup.OrderAddresses.FirstOrDefault(a => a.OrderAddressId == CurrentOrderGroup.AddressId);
                if (billingAddress != null)
                {
                    customerName = string.Format("{0} {1}", billingAddress.FirstName, billingAddress.LastName);
                }
            }

            var order = new Order();
            order.InjectFrom<CloneInjection>(CurrentOrderGroup);
            order.CustomerName = customerName;
            order.Status = "Pending";
            order.Name = "Default";
            order.TrackingNumber = SequencesClient.GenerateNext(typeof(Order).FullName);

            foreach (var newOf in order.OrderForms)
            {
                newOf.Name = "Default";
                if (!string.IsNullOrEmpty(CustomerSessionService.CustomerSession.CsrUsername))
                {
                    //Add order form property CSR username is saved in the order form property called "Purchased By CSR"
                    newOf.OrderFormPropertyValues.Add(new OrderFormPropertyValue() { ShortTextValue = CustomerSessionService.CustomerSession.CsrUsername, Name = "Purchased By CSR" });
                }
            }
            var cart = OrderRepository.ShoppingCarts.FirstOrDefault(c => c.OrderGroupId == CurrentOrderGroup.OrderGroupId);
            if (cart != null)
            {
                OrderRepository.Remove(cart);
            }
            OrderRepository.Add(order);
            OrderRepository.UnitOfWork.Commit();
        }
	}
}
