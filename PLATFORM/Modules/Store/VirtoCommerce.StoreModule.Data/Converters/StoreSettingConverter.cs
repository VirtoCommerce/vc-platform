using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.StoreModule.Data.Converters
{
	public static class StoreSetting
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.StoreSetting ToCoreModel(this foundationModel.StoreSetting dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.StoreSetting();
			retVal.InjectFrom(dbEntity);
			retVal.ValueType = (coreModel.SettingValueType)Enum.Parse(typeof(coreModel.SettingValueType), dbEntity.ValueType, true);
			retVal.Value = dbEntity.RawValue();
			return retVal;

		}


		public static foundationModel.StoreSetting ToFoundation(this coreModel.StoreSetting setting)
		{
			if (setting == null)
				throw new ArgumentNullException("setting");

			var retVal = new foundationModel.StoreSetting();
			retVal.InjectFrom(setting);
			retVal.ValueType = setting.ValueType.ToString();
			SetPropertyValue(retVal, setting);
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.StoreSetting source, foundationModel.StoreSetting target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<foundationModel.StoreSetting>(x => x.BooleanValue, x => x.DateTimeValue,
																					   x => x.DecimalValue, x => x.IntegerValue,
																					   x => x.Locale, x => x.LongTextValue, x => x.ShortTextValue,
																					   x => x.ValueType);
			target.InjectFrom(patchInjectionPolicy, source);

		}

		private static void SetPropertyValue(foundationModel.StoreSetting retVal, coreModel.StoreSetting setting)
		{
			switch (setting.ValueType)
			{
				case coreModel.SettingValueType.Boolean:
					retVal.BooleanValue = Convert.ToBoolean(setting.Value);
					break;
				case coreModel.SettingValueType.DateTime:
					retVal.DateTimeValue = Convert.ToDateTime(setting.Value);
					break;
				case coreModel.SettingValueType.Decimal:
					retVal.DecimalValue = Convert.ToDecimal(setting.Value);
					break;
				case coreModel.SettingValueType.Integer:
					retVal.IntegerValue = Convert.ToInt32(setting.Value);
					break;
				case coreModel.SettingValueType.LongText:
					retVal.LongTextValue = Convert.ToString(setting.Value);
					break;
				case coreModel.SettingValueType.ShortText:
					retVal.ShortTextValue = Convert.ToString(setting.Value);
					break;
			}
		}
	}
}
