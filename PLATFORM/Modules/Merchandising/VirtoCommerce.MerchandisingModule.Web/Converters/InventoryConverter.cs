using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using Omu.ValueInjecter;
using data = VirtoCommerce.Domain.Inventory.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class InventoryConverter
	{
		public static webModel.Inventory ToWebModel(this data.InventoryInfo inventoryInfo)
		{
			var retVal = new webModel.Inventory();
			retVal.InjectFrom(inventoryInfo);		
			return retVal;
		}
	}
}
