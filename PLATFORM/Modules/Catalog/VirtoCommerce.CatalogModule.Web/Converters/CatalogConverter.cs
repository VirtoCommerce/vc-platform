using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class CatalogConverter
	{
		public static webModel.Catalog ToWebModel(this moduleModel.Catalog catalog, bool convertProps = true)
		{
			var retVal = new webModel.Catalog();
			retVal.InjectFrom(catalog);
			retVal.Properties = new List<webModel.Property>();
			if (catalog.Languages != null)
			{
				retVal.Languages = catalog.Languages.Select(x=>x.ToWebModel()).ToList();
			}

            if (convertProps)
            {
                //Need add property for each meta info
                if (catalog.Properties != null)
                {
                    foreach (var property in catalog.Properties)
                    {
                        var webModelProperty = property.ToWebModel();
                        //Reset dict values to decrease response size
                        webModelProperty.DictionaryValues = null;
                        webModelProperty.Values = new List<webModel.PropertyValue>();
                        webModelProperty.IsManageable = true;
                        webModelProperty.IsReadOnly = property.Type != moduleModel.PropertyType.Catalog;
                        retVal.Properties.Add(webModelProperty);
                    }
                }

                //Populate property for property values
                if (catalog.PropertyValues != null)
                {
                    foreach (var propValue in catalog.PropertyValues.Select(x => x.ToWebModel()))
                    {
                        var property = retVal.Properties.FirstOrDefault(x => x.Id == propValue.PropertyId);
                        if (property == null)
                        {
                            property = retVal.Properties.FirstOrDefault(x => x.Name.EqualsInvariant(propValue.PropertyName));
                        }
                        if (property == null)
                        {
                            //Need add dummy property for each value without property
                            property = new webModel.Property(propValue, catalog.Id, null, moduleModel.PropertyType.Catalog);
                            retVal.Properties.Add(property);
                        }
                       property.Values.Add(propValue);
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
						propValue.ValueType = property.ValueType;
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
