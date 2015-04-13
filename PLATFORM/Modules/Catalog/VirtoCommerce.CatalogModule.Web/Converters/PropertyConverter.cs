using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class PropertyConverter
	{
		public static webModel.Property ToWebModel(this moduleModel.Property property)
		{
			var retVal = new webModel.Property();

			retVal.InjectFrom(property);
			retVal.Catalog = property.Catalog.ToWebModel();
			if (property.Category != null)
			{
				retVal.Category = property.Category.ToWebModel();
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
			return retVal;
		}

		public static moduleModel.Property ToModuleModel(this webModel.Property property)
		{
			var retVal = new moduleModel.Property();

			retVal.InjectFrom(property);
			retVal.ValueType = (moduleModel.PropertyValueType)(int)property.ValueType;
			retVal.Type = (moduleModel.PropertyType)(int)property.Type;

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
