using Omu.ValueInjecter;
using webModel = VirtoCommerce.StoreModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Shipping.Model;
using System.Linq;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class ShippingMethodConverter
	{
		public static webModel.ShippingMethod ToWebModel(this coreModel.ShippingMethod method)
		{
			var retVal = new webModel.ShippingMethod();
			retVal.InjectFrom(method);
			if(method.Settings != null)
				retVal.Settings = method.Settings.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.ShippingMethod ToCoreModel(this webModel.ShippingMethod method, coreModel.ShippingMethod shippingMethod)
		{
			var retVal = shippingMethod;
			retVal.InjectFrom(method);
			if (method.Settings != null)
				retVal.Settings = method.Settings.Select(x => x.ToCoreModel()).ToList();
			return retVal;
		}
	}
}
