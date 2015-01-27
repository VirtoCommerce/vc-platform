using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class CustomerOrderConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this CustomerOrder source, CustomerOrder target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			source.Patch((Operation)target);

			if (source.Shipments != null)
			{
				if (target.Shipments == null)
					target.Shipments = new ObservableCollection<Shipment>();

				source.Shipments.Patch(target.Shipments, new OperationComparer(),
													 (sourceShipment, targetShipment) => sourceShipment.Patch(targetShipment));
			}

			if (source.Items != null)
			{
				if (target.Items == null)
					target.Items = new ObservableCollection<CustomerOrderItem>();
				source.Items.Patch(target.Items, new PositionComparer(), (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			if (source.InPayments != null)
			{
				if (target.InPayments == null)
					target.InPayments = new ObservableCollection<PaymentIn>();

				source.InPayments.Patch(target.InPayments, new OperationComparer(), (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}
		}
	}
}
