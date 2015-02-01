using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Money;
using Omu.ValueInjecter;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class PaymentConverter
	{
		public static Payment ToCoreModel(this PaymentEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new Payment();
			retVal.InjectFrom(entity);
			if (entity.Currency != null)
			{
				retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			}
			if (entity.BillingAddress != null)
			{
				retVal.BillingAddress = entity.BillingAddress.ToCoreModel();
			}
			return retVal;
		}

		public static PaymentEntity ToEntity(this Payment paymentIn)
		{
			if (paymentIn == null)
				throw new ArgumentNullException("payment");

			var retVal = new PaymentEntity();
			retVal.InjectFrom(paymentIn);
			if (retVal.IsTransient())
			{
				retVal.Id = Guid.NewGuid().ToString();
			}
			if (paymentIn.Currency != null)
			{
				retVal.Currency = paymentIn.Currency.ToString();
			}

			if (paymentIn.BillingAddress != null)
			{
				retVal.BillingAddress = paymentIn.BillingAddress.ToEntity();
			}
			return retVal;
		}


		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this PaymentEntity source, PaymentEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			target.Amount = source.Amount;

			if (source.PaymentGatewayCode != null)
				target.PaymentGatewayCode = source.PaymentGatewayCode;

			var addressComparer = new AddressComparer();
			if (source.BillingAddress != null && !addressComparer.Equals(target.BillingAddress, source.BillingAddress))
				target.BillingAddress = source.BillingAddress;
		}

	}

}
