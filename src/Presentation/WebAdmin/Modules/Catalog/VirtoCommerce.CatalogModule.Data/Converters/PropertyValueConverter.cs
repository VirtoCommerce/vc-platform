using System;
using System.Collections.Generic;
using System.Linq;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class PropertyValueConverter
	{
		
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.PropertyValue ToModuleModel(this foundation.PropertyValueBase dbPropValue,	 module.Property[] properties)
		{
			if (dbPropValue == null)
				throw new ArgumentNullException("dbPropValue");

			var retVal = new module.PropertyValue
			{
				Id = dbPropValue.PropertyValueId,
				LanguageCode = dbPropValue.Locale,
				PropertyName = dbPropValue.Name,
				Value = dbPropValue.ToString(),
				ValueId = dbPropValue.KeyValue,
                ValueType = ((foundation.PropertyValueType)dbPropValue.ValueType).ToModuleModel()
			};
		
			if (properties != null)
			{
				var property = properties.FirstOrDefault(x => String.Equals(x.Name, dbPropValue.Name, StringComparison.InvariantCultureIgnoreCase));
				if (property != null)
				{
					retVal.PropertyId = property.Id;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.PropertyValueBase ToFoundation<T>(this module.PropertyValue propValue) where T : new()
		{
			if (propValue == null)
				throw new ArgumentNullException("propValue");

			foundation.PropertyValueBase retVal = new T() as foundation.PropertyValueBase;
			
            if (propValue.Id != null)
				retVal.PropertyValueId = propValue.Id;

			retVal.Name = propValue.PropertyName;
			retVal.KeyValue = propValue.ValueId;
			retVal.Locale = propValue.LanguageCode;
			retVal.ValueType = (int)propValue.ValueType;
            SetPropertyValue(retVal, propValue.ValueType.ToFoundation(), propValue.Value);

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.PropertyValueBase source, foundation.PropertyValueBase target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var newValue = target.ToString();
			if (newValue != null)
				SetPropertyValue(target, (foundation.PropertyValueType)target.ValueType, target.ToString());
			if (source.KeyValue != null)
				target.KeyValue = source.KeyValue;
	
		}

		private static void SetPropertyValue(foundation.PropertyValueBase retVal, foundation.PropertyValueType type, string value)
		{
			switch (type)
			{
				case foundation.PropertyValueType.LongString:
					retVal.LongTextValue = value;
					break;
				case foundation.PropertyValueType.ShortString:
					retVal.ShortTextValue = value;
					break;
				case foundation.PropertyValueType.Decimal:
					retVal.DecimalValue = Decimal.Parse(value);
					break;
			}
		}
	}

	public class PropertyValueComparer : IEqualityComparer<foundation.PropertyValueBase>
	{
		#region IEqualityComparer<Item> Members

		public bool Equals(foundation.PropertyValueBase x, foundation.PropertyValueBase y)
		{
			return x.PropertyValueId == y.PropertyValueId;
		}

		public int GetHashCode(foundation.PropertyValueBase obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}
