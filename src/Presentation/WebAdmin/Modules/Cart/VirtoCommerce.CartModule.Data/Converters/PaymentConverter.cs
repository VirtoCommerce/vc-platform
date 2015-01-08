using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class PaymentConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Payment source, Payment target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			if (source.Amount != null)
				target.Amount = source.Amount;
			if (source.PaymentGatewayCode != null)
				target.PaymentGatewayCode = source.PaymentGatewayCode;

			if (source.BillingAddress != null)
				target.BillingAddress = source.BillingAddress;
		}

	}

	public class PaymentComparer : IEqualityComparer<Payment>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(Payment x, Payment y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(Payment obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
