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
	public static class CatalogConverter
	{
		public static webModel.Catalog ToWebModel(this moduleModel.Catalog catalog, moduleModel.Property[] properties = null)
		{
			var retVal = new webModel.Catalog();
			retVal.InjectFrom(catalog);
			retVal.Properties = new List<webModel.Property>();
			if (catalog.Languages != null)
			{
				retVal.Languages = catalog.Languages.Select(x=>x.ToWebModel()).ToList();
			}
			//Need add property for each meta info
			if (properties != null)
			{
				foreach (var property in properties)
				{
					var webModelProperty = property.ToWebModel();
					webModelProperty.Values = new List<webModel.PropertyValue>();
					webModelProperty.IsManageable = true;
					webModelProperty.IsReadOnly = property.Type != moduleModel.PropertyType.Catalog;
					retVal.Properties.Add(webModelProperty);
				}
			}

			//Populate property values
			if (catalog.PropertyValues != null)
			{
				foreach (var propValue in catalog.PropertyValues)
				{
					var property = retVal.Properties.FirstOrDefault(x => x.Id == propValue.PropertyId);
					if (property == null)
					{
						//Need add dummy property for each value without property
						property = new webModel.Property
						{
							Catalog = retVal,
							CatalogId = retVal.Id,
							IsManageable = false,
							Name = propValue.PropertyName,
							Type = webModel.PropertyType.Category,
							ValueType = (webModel.PropertyValueType)(int)propValue.ValueType,
						};
						property.Values = new List<webModel.PropertyValue>();
						property.Values.Add(propValue.ToWebModel());
						retVal.Properties.Add(property);
					}
					else
					{
						property.Values = catalog.PropertyValues
														  .Where(x => x.PropertyId == property.Id)
														  .Select(x => x.ToWebModel())
														  .ToList();
					}
				}

			}
			return retVal;
		}

		public static moduleModel.Catalog ToModuleModel(this webModel.Catalog catalog)
		{
			var retVal = new moduleModel.Catalog();
			retVal.InjectFrom(catalog);
			if (catalog.Languages != null)
			{
				retVal.Languages = catalog.Languages.Select(x => x.ToModuleModel()).ToList();
			}

			if (catalog.Properties != null)
			{
				retVal.PropertyValues = new List<moduleModel.PropertyValue>();
				foreach (var property in catalog.Properties)
				{
					foreach (var propValue in property.Values)
					{
						//Need populate required fields
						propValue.PropertyName = property.Name;
						retVal.PropertyValues.Add(propValue.ToModuleModel());
					}
				}

			}
			return retVal;
		}


	}
}
