using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class PropertyConverter
	{
		public static webModel.Property ToWebModel(this coreModel.Property property)
		{
			var retVal = new webModel.Property();

			retVal.InjectFrom(property);
			retVal.Catalog = property.Catalog.ToWebModel(convertProps: false);
			if (property.Category != null)
			{
				retVal.Category = property.Category.ToWebModel(convertProps: false);
			}
			retVal.ValueType = property.ValueType;
			retVal.Type = property.Type;

			if (property.DictionaryValues != null)
			{
				retVal.DictionaryValues = property.DictionaryValues.Select(x => x.ToWebModel()).ToList();
			}

			if(property.Attributes != null)
			{
				retVal.Attributes = property.Attributes.Select(x => x.ToWebModel()).ToList();
			}
			retVal.DisplayNames = property.DisplayNames;

			return retVal;
		}

		public static coreModel.Property ToModuleModel(this webModel.Property property)
		{
			var retVal = new coreModel.Property();

			retVal.InjectFrom(property);
			retVal.ValueType = (coreModel.PropertyValueType)(int)property.ValueType;
			retVal.Type = (coreModel.PropertyType)(int)property.Type;
			retVal.DisplayNames = property.DisplayNames;
			if (property.DictionaryValues != null)
			{
				retVal.DictionaryValues = property.DictionaryValues.Select(x => x.ToModuleModel()).ToList();
			}
			if (property.Attributes != null)
			{
				retVal.Attributes = property.Attributes.Select(x => x.ToModuleModel()).ToList();
			}

			return retVal;
		}


	}
}
