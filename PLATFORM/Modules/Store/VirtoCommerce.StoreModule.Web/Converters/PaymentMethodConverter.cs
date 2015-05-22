using Omu.ValueInjecter;
using webModel = VirtoCommerce.StoreModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Payment2.Model;
using System.Linq;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class PaymentMethodConverter
	{
		public static webModel.PaymentMethod ToWebModel(this coreModel.PaymentMethod method)
		{
			var retVal = new webModel.PaymentMethod();
			retVal.InjectFrom(method);
			if(method.Settings != null)
				retVal.Settings = method.Settings.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.PaymentMethod ToCoreModel(this webModel.PaymentMethod webMethod, coreModel.PaymentMethod paymentMethod)
		{
			var retVal = paymentMethod;
			retVal.InjectFrom(webMethod);
			if (webMethod.Settings != null)
				retVal.Settings = webMethod.Settings.Select(x => x.ToCoreModel()).ToList();
			return retVal;
		}
	}
}
