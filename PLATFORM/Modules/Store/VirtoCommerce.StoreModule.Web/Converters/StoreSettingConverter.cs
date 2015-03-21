using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class StoreSettingConverter
	{
		public static webModel.StoreSetting ToWebModel(this coreModel.StoreSetting setting)
		{
			var retVal = new webModel.StoreSetting();
			retVal.InjectFrom(setting);

			retVal.ValueType = setting.ValueType;
			retVal.Value = setting.Value != null ? setting.Value.ToString() : null;

			return retVal;
		}

		public static coreModel.StoreSetting ToCoreModel(this webModel.StoreSetting setting)
		{
			var retVal = new coreModel.StoreSetting();
			retVal.InjectFrom(setting);

			retVal.ValueType = setting.ValueType;
			retVal.Value = setting.Value;

			return retVal;
		}


	}
}