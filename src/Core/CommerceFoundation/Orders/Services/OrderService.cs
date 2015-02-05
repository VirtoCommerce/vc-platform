using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Search;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Services;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;

namespace VirtoCommerce.Foundation.Orders.Services
{
    using System.Diagnostics;

    [UnityInstanceProviderServiceBehaviorAttribute]
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly ISearchService _searchService;
		private readonly IWorkflowService _workflowService;
		private readonly IPaymentMethodRepository _paymentMethodRepository;
		private readonly IStoreRepository _storeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
		public OrderService()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        /// <param name="searchService">The search service.</param>
        /// <param name="workflowService">The workflow service.</param>
        /// <param name="paymentMethodRepository">The payment method repository.</param>
        /// <param name="storeRepository">The store repository.</param>
		public OrderService(IOrderRepository orderRepository, ISearchService searchService, IWorkflowService workflowService, IPaymentMethodRepository paymentMethodRepository, IStoreRepository storeRepository)
			: this()
		{
			_orderRepository = orderRepository;
			_searchService = searchService;
			_workflowService = workflowService;
			_paymentMethodRepository = paymentMethodRepository;
			_storeRepository = storeRepository;
		}

		#region IOrderService Members
		public OrderSearchResults SearchOrders(string scope, ISearchCriteria criteria)
		{
			var results = _searchService.Search(scope, criteria) as SearchResults;

			if (results != null)
			{
				var resultIds = results.GetKeyFieldValues<string>();

				var orders = _orderRepository.Orders.Where(x => resultIds.Contains(x.OrderGroupId)).ToArray();

				var retVal = new OrderSearchResults(criteria, orders, results);
				return retVal;
			}
			return null;
		}

		public OrderWorkflowResult ExecuteWorkflow(string workflowName, OrderGroup orderGroup)
		{
			var parameters = new Dictionary<string, object>();
			parameters["OrderGroupArgument"] = orderGroup;

			var results = _workflowService.RunWorkflow(workflowName, parameters, new object[] { ServiceLocator.Current });

			var r = new OrderWorkflowResult(results) { OrderGroup = orderGroup };
			return r;
		}

		public CreatePaymentResult CreatePayment(Payment payment)
		{
			var result = new CreatePaymentResult { TransactionId = payment.AuthorizationCode };

			if (string.IsNullOrEmpty(payment.TransactionType))
			{
				result.Message = "Transaction type is required";
				return result;
			}

			var orderForm = _orderRepository.Orders.SelectMany(o => o.OrderForms)
				.ExpandAll().Expand("OrderGroup/OrderAddresses")
				.First(of => of.OrderFormId == payment.OrderFormId);

			var transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), payment.TransactionType);

			if (transactionType == TransactionType.Credit)
			{
				var totalSales = orderForm.Payments.Where(
					p => p.Status == PaymentStatus.Completed.ToString() &&
                         (p.TransactionType == TransactionType.Sale.ToString() 
                         || p.TransactionType == TransactionType.Capture.ToString())).Sum(p => p.Amount);

				if (payment.Amount > totalSales)
				{
					result.Message = string.Format("Payment amount exceeds total sales amount {0} {1}", totalSales,
					                               orderForm.OrderGroup.BillingCurrency);
					return result;
				}
			}

			var paymentMethod = _paymentMethodRepository.PaymentMethods
									.Expand("PaymentGateway")
									.Expand("PaymentMethodPropertyValues")
									.ExpandAll()
									.FirstOrDefault(p => p.PaymentMethodId.Equals(payment.PaymentMethodId));

			if (paymentMethod == null)
			{
				result.Message = String.Format("Specified payment method \"{0}\" has not been defined.", payment.PaymentMethodId);
				return result;
			}


			if ((((TransactionType)paymentMethod.PaymentGateway.SupportedTransactionTypes) & transactionType) != transactionType)
			{
				result.Message = String.Format("Transaction type {0} is not supported by payment gateway", payment.TransactionType);
				return result;
			}

			Debug.WriteLine(String.Format("Getting the type \"{0}\".", paymentMethod.PaymentGateway.ClassType));
			var type = Type.GetType(paymentMethod.PaymentGateway.ClassType);
			if (type == null)
			{
				result.Message = String.Format("Specified payment method class \"{0}\" can not be created.", paymentMethod.PaymentGateway.ClassType);
				return result;
			}

			var provider = (IPaymentGateway)Activator.CreateInstance(type);
			provider.Settings = CreateSettings(paymentMethod);

			payment.Status = PaymentStatus.Pending.ToString();

			var message = "";
            Debug.WriteLine(String.Format("Processing the payment."));

			try
			{
				//Save changes before process
				
				payment.OrderForm = null;
				orderForm.Payments.Add(payment);
				_orderRepository.UnitOfWork.Commit();

				payment.OrderForm = orderForm;

				result.IsSuccess = provider.ProcessPayment(payment, ref message);
				result.Message = message;
				result.TransactionId = payment.AuthorizationCode;

                Debug.WriteLine(String.Format("Payment processed."));
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
				payment.Status = PaymentStatus.Failed.ToString();
				Trace.TraceError(ex.Message);
			}
			finally
			{
				PostProcessPayment(payment);
				_orderRepository.Update(payment);
				_orderRepository.UnitOfWork.Commit();
			}
			return result;
		}

	
		#endregion

		#region Private Helpers
		private Dictionary<string, string> CreateSettings(PaymentMethod method)
		{
			var settings = method.PaymentMethodPropertyValues.ToDictionary(property => property.Name, property => property.ToString());
			settings["Gateway"] = method.PaymentGateway.GatewayId;
			return settings;
		}

		/// <summary>
		/// Post process the payment. Decrypts the data if needed.
		/// </summary>
		private void PostProcessPayment(Payment payment)
		{
			// We only care about credit cards here, all other payment types should be encrypted by default
			var cardPayment = payment as CreditCardPayment;
			var store = _storeRepository.Stores.FirstOrDefault(s => s.StoreId == payment.OrderForm.OrderGroup.StoreId);
			if (cardPayment != null)
			{
				if (store != null)
				{
					switch ((CreditCardSavePolicy)store.CreditCardSavePolicy)
					{
						case CreditCardSavePolicy.Full:
							//leave full number
							break;
						case CreditCardSavePolicy.LastFourDigits:
							var ccNumber = cardPayment.CreditCardNumber;
							if (!String.IsNullOrEmpty(ccNumber) && ccNumber.Length > 4)
							{
								ccNumber = ccNumber.Substring(ccNumber.Length - 4);
								cardPayment.CreditCardNumber = ccNumber;
							}
							break;
						case CreditCardSavePolicy.None:
							cardPayment.CreditCardNumber = string.Empty;
							break;
					}
				}
				else
				{
					cardPayment.CreditCardNumber = string.Empty;
				}

				// Always remove pin
				cardPayment.CreditCardSecurityCode = String.Empty;
			}
		}
		#endregion
	}
}
