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
	public static class ShipmentConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Shipment source, Shipment target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			source.Patch((Operation)target);
		
			if (source.InPayments != null)
			{
				if (target.InPayments == null)
					target.InPayments = new ObservableCollection<PaymentIn>();

				source.InPayments.Patch(target.InPayments, new OperationComparer(), (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}
		}
	}
}
