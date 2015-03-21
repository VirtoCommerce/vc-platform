using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;

namespace VirtoCommerce.OrderWorkflow
{
    using System.Diagnostics;

    public class ProcessShipmentActivity : OrderActivityBase
	{
		public ProcessShipmentActivity()
		{
		}

		public ProcessShipmentActivity(IShippingRepository shippingRepository)
			: this()
		{
			_shippingRepository = shippingRepository;
		}

		IShippingRepository _shippingRepository;
		protected IShippingRepository ShippingRepository
		{
			get { return _shippingRepository ?? (_shippingRepository = ServiceLocator.GetInstance<IShippingRepository>()); }
			set
			{
				_shippingRepository = value;
			}
		}

		protected override void Execute(System.Activities.CodeActivityContext context)
		{
			base.Execute(context);
			ProcessShipments();
		}


		/// <summary>
		/// Processes the shipments.
		/// </summary>
		private void ProcessShipments()
		{
			var options = ShippingRepository.ShippingOptions.ExpandAll()
				.Expand("ShippingGateway") //remove this when fixed
				.ToArray();
			var methods = (from o in options from m in o.ShippingMethods select m).ToArray();

			var order = CurrentOrderGroup;

			// request rates, make sure we request rates not bound to selected delivery method
			foreach (var form in order.OrderForms)
			{
				foreach (var shipment in form.Shipments)
				{
					var method = (from s in methods where s.ShippingMethodId == shipment.ShippingMethodId select s).FirstOrDefault();

					// If shipping method is not found, set it to 0 and continue
					if (method == null)
					{
						Trace.TraceInformation(String.Format("Total shipment is 0 so skip shipment calculations."));
						shipment.ShippingCost = 0;
						continue;
					}

					var classType = method.ShippingOption.ShippingGateway.ClassType;
					// Check if package contains shippable items, if it does not use the default shipping method instead of the one specified
					Debug.WriteLine(String.Format("Getting the type \"{0}\".", classType));
					var type = Type.GetType(classType);
					if (type == null)
					{
						throw new TypeInitializationException(classType, null);
					}
					var message = String.Empty;
                    Debug.WriteLine(String.Format("Creating instance of \"{0}\".", type.Name));
					var provider = (IShippingGateway)Activator.CreateInstance(type);

					var items = GetLineItems(shipment);

                    Debug.WriteLine(String.Format("Calculating the rates."));
					var rate = provider.GetRate(method.ShippingMethodId, items.ToArray(), ref message);
					if (rate != null)
					{
                        Debug.WriteLine(String.Format("Rates calculated."));
						shipment.ShippingCost = rate.Price;
					}
					else
                        Debug.WriteLine(String.Format("No rates has been found."));
				}
			}
		}

		/// <summary>
		/// Gets the line items with quantities that are included in the shipment.
		/// </summary>
		/// <param name="shipment">The shipment.</param>
		/// <returns></returns>
		private IEnumerable<LineItem> GetLineItems(Shipment shipment)
		{
			var lineItems = new List<LineItem>();
			foreach (var shipItem in shipment.ShipmentItems)
			{
				if (shipItem.LineItem == null)
					continue;

				var lineItem = shipItem.LineItem.DeepClone(new OrderEntityFactory());
				lineItem.Quantity = shipItem.Quantity;
				lineItem.ExtendedPrice = lineItem.ExtendedPrice / lineItem.Quantity * shipItem.Quantity;
				lineItems.Add(lineItem);
			}

			return lineItems.ToArray();
		}
	}
}
