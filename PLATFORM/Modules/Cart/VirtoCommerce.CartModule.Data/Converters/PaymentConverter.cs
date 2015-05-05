using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

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
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);

			if (entity.Addresses != null && entity.Addresses.Any())
			{
				retVal.BillingAddress = entity.Addresses.First().ToCoreModel();
			}
			return retVal;
		}

		public static PaymentEntity ToDataModel(this Payment payment)
		{
			if (payment == null)
				throw new ArgumentNullException("payment");

			var retVal = new PaymentEntity();
			retVal.InjectFrom(payment);

			retVal.Currency = payment.Currency.ToString();

			if (payment.BillingAddress != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(new AddressEntity[] { payment.BillingAddress.ToDataModel() });
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

			var patchInjection = new PatchInjection<PaymentEntity>(x => x.Amount, x => x.GatewayCode);
			target.InjectFrom(patchInjection, source);

			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}
		}

	}

}
