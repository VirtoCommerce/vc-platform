using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class ShoppingCartConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShoppingCart source, ShoppingCart target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			//Simply properties patch
			if (source.Name != null)
				target.Name = source.Name;
			if (source.Currency  != null)
				target.Currency = source.Currency;
			if (source.CustomerId != null)
				target.CustomerId = source.CustomerId;
			if (source.CustomerName != null)
				target.CustomerName = source.CustomerName;
			if(source.Dimension != null && !source.Dimension.Equals(target.Dimension))
			{
				target.Dimension = source.Dimension;
			}
			if(source.IsAnonymous != null)
				target.IsAnonymous = source.IsAnonymous;

			if(source.IsRecuring != null)
				target.IsRecuring = source.IsRecuring;
			if (source.LanguageCode != null)
				target.LanguageCode = source.LanguageCode;
			if (source.Note != null)
				target.Note = source.Note;
			if (source.OrganizationId != null)
				target.OrganizationId = source.OrganizationId;
			if (source.VolumetricWeight != null)
				target.VolumetricWeight = source.VolumetricWeight;
		
		
		
			if (source.Discounts != null)
			{
				if (target.Discounts == null)
					target.Discounts = new List<Discount>();

				source.Discounts.Patch(target.Discounts, new DiscountComparer(),
													 (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}

			if(source.Items != null)
			{
				if (target.Items == null)
					target.Items = new List<CartItem>();
				source.Items.Patch(target.Items, new CartItemComparer(), (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			if (source.Payments != null)
			{
				if (target.Payments == null)
					target.Payments = new List<Payment>();

				source.Payments.Patch(target.Payments, new PaymentComparer(), (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}

			if (source.BillingAddresses != null)
			{
				if (target.BillingAddresses == null)
					target.BillingAddresses = new List<Address>();

				source.BillingAddresses.Patch(target.BillingAddresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}

			if (source.ShippingAddresses != null)
			{
				if (target.ShippingAddresses == null)
					target.ShippingAddresses = new List<Address>();

				source.ShippingAddresses.Patch(target.ShippingAddresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}
		}

	}
}
