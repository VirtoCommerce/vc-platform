using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using webModel = VirtoCommerce.InventoryModule.Web.Model;

namespace VirtoCommerce.InventoryModule.Web.Converters
{
	public static class FulfillmentCenterConverter
	{
		public static webModel.FulfillmentCenter ToWebModel(this coreModel.FulfillmentCenter center)
		{
			var retVal = new webModel.FulfillmentCenter();
			retVal.InjectFrom(center);
			return retVal;
		}
	}
}
