using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Payment.Model;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.StoreModule.Data.Converters
{
	public static class StorePaymentMethodConverter
	{
		
		public static dataModel.StorePaymentMethod ToDataModel(this coreModel.PaymentMethod paymentMethod)
		{
			if (paymentMethod == null)
				throw new ArgumentNullException("paymentMethod");

			var retVal = new dataModel.StorePaymentMethod();

			retVal.InjectFrom(paymentMethod);

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.StorePaymentMethod source, dataModel.StorePaymentMethod target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjectionPolicy = new PatchInjection<dataModel.StorePaymentMethod>(x => x.LogoUrl, x => x.Name,
																		   x => x.Description, x => x.Priority,
																		   x => x.IsActive);
			target.InjectFrom(patchInjectionPolicy, source);
		}


	}
}
