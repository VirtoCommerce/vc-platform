using Omu.ValueInjecter;
using webModel = VirtoCommerce.Platform.Web.Model.Settings;
using moduleModel = VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Converters.Settings
{
    public static class SettingConverter
    {
        public static webModel.Setting ToWebModel(this moduleModel.SettingEntry setting)
        {
			var retVal = new webModel.Setting();
			retVal.InjectFrom(setting);
            return retVal;
        }

		public static moduleModel.SettingEntry ToModuleModel(this webModel.Setting setting)
        {
			var retVal = new moduleModel.SettingEntry();
			retVal.InjectFrom(setting);
			return retVal;
        }
    }
}
