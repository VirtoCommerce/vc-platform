using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;

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
			if (entity.Currency != null)
			{
				retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			}
			if(entity.BillingAddress != null)
			{
				retVal.BillingAddress = entity.BillingAddress.ToCoreModel();
			}
			return retVal;
		}

		public static PaymentInEntity ToEntity(this PaymentIn paymentIn)
		{
			if (paymentIn == null)
				throw new ArgumentNullException("paymentIn");

			var retVal = new PaymentInEntity();
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
		public static void Patch(this PaymentInEntity source, PaymentInEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			source.Patch((OperationEntity)target);
		}
	}


}
