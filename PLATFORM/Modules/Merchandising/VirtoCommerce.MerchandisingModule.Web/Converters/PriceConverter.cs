using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class PriceConverter
	{
		#region Public Methods and Operators

		public static webModel.Price ToWebModel(this coreModel.Price price)
		{
			var retVal = new webModel.Price();
			retVal.InjectFrom(price);
			retVal.ProductId = price.ProductId;
			return retVal;
		}

		#endregion
	}
}