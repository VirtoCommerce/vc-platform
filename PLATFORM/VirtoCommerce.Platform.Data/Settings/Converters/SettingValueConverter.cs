using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Settings.Converters
{
	public static class SettingValueConverter
	{
		
		public static SettingValueEntity ToEntity(this string value, SettingValueType valueType)
		{
			var retVal = new SettingValueEntity();
			retVal.ValueType = valueType.ToString();

			if(valueType == SettingValueType.Boolean)
			{
				retVal.BooleanValue = Convert.ToBoolean(value);
			}
			else if(valueType == SettingValueType.DateTime)
			{
				retVal.DateTimeValue = Convert.ToDateTime(value);
			}
			else if(valueType == SettingValueType.Decimal)
			{
				retVal.DecimalValue = Convert.ToDecimal(value);
			}
			else if(valueType == SettingValueType.Integrer)
			{
				retVal.IntegerValue = Convert.ToInt32(value);
			}
			else if(valueType == SettingValueType.LongText)
			{
				retVal.LongTextValue = value;
			}
			else if (valueType == SettingValueType.SecureString)
			{
				retVal.ShortTextValue = value;
			}
			else
			{
				retVal.ShortTextValue = value;
			}
			return retVal;
		}

		

	}
}
