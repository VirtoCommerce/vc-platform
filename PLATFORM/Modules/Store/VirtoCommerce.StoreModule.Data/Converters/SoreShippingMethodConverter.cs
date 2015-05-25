using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Shipping.Model;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.StoreModule.Data.Converters
{
	public static class StoreShippingMethodConverter
	{
		
		public static dataModel.StoreShippingMethod ToDataModel(this coreModel.ShippingMethod shippingMethod)
		{
			if (shippingMethod == null)
				throw new ArgumentNullException("shippingMethod");

			var retVal = new dataModel.StoreShippingMethod();

			retVal.InjectFrom(shippingMethod);

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.StoreShippingMethod source, dataModel.StoreShippingMethod target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjectionPolicy = new PatchInjection<dataModel.StoreShippingMethod>(x => x.LogoUrl, x => x.Name,
																		   x => x.Description, x => x.Priority,
																		   x => x.IsActive);
			target.InjectFrom(patchInjectionPolicy, source);
		}


	}
}
