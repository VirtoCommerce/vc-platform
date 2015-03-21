using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Inventory.Model;
using webModel = VirtoCommerce.InventoryModule.Web.Model;

namespace VirtoCommerce.InventoryModule.Web.Converters
{
	public static class InventoryConverter
	{
		public static webModel.InventoryInfo ToWebModel(this coreModel.InventoryInfo inventory)
		{
			var retVal = new webModel.InventoryInfo();
			retVal.InjectFrom(inventory);
			return retVal;
		}

		public static coreModel.InventoryInfo ToCoreModel(this webModel.InventoryInfo inventory)
		{
			var retVal = new coreModel.InventoryInfo();
			retVal.InjectFrom(inventory);
			return retVal;
		}


	}
}
