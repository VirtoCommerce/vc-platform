using Omu.ValueInjecter;
using webModel = VirtoCommerce.StoreModule.Web.Model;
using coreModel = VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class SettingConverter
	{
		public static webModel.Setting ToWebModel(this coreModel.SettingEntry setting)
		{
			var retVal = new webModel.Setting();
			retVal.InjectFrom(setting);
			return retVal;
		}

		public static coreModel.SettingEntry ToCoreModel(this webModel.Setting setting)
		{
			var retVal = new coreModel.SettingEntry();
			retVal.InjectFrom(setting);
			return retVal;
		}
	}
}
