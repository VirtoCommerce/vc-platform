using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using Omu.ValueInjecter;
using cart = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class PaymentInConverter
	{
		public static PaymentIn ToCoreModel(this PaymentInEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new PaymentIn();
			retVal.InjectFrom(entity);
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);

			if (entity.Addresses != null && entity.Addresses.Any())
			{
				retVal.BillingAddress = entity.Addresses.First().ToCoreModel();
			}
			return retVal;
		}

		public static PaymentIn ToCoreModel(this cart.Payment payment)
		{
			if (payment == null)
				throw new ArgumentNullException("payment");

			var retVal = new PaymentIn();
			retVal.InjectFrom(payment);
			retVal.Currency = payment.Currency;
			retVal.GatewayCode = payment.PaymentGatewayCode;
			retVal.Sum = payment.Amount;

			if(payment.BillingAddress != null)
			{
				retVal.BillingAddress = payment.BillingAddress.ToCoreModel();
			}

			return retVal;
		}

		public static PaymentInEntity ToEntity(this PaymentIn paymentIn)
		{
			if (paymentIn == null)
				throw new ArgumentNullException("paymentIn");

			var retVal = new PaymentInEntity();
			retVal.InjectFrom(paymentIn);

			retVal.Currency = paymentIn.Currency.ToString();

			if (paymentIn.BillingAddress != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(new AddressEntity[] { paymentIn.BillingAddress.ToEntity() });
			}
			return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this PaymentInEntity source, PaymentInEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			source.Patch((OperationEntity)target);

			var patchInjectionPolicy = new PatchInjection<PaymentInEntity>(x => x.CustomerId, x => x.OrganizationId, x => x.GatewayCode, x => x.Purpose);
			target.InjectFrom(patchInjectionPolicy, source);

			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}
		}
	}


}
