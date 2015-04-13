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
	public static class CategoryConverter
	{
		public static webModel.Category ToWebModel(this moduleModel.Category category, moduleModel.Property[] properties = null)
		{
			var retVal = new webModel.Category();
			retVal.InjectFrom(category);
			retVal.Catalog = category.Catalog.ToWebModel();
	
			if(category.Parents != null)
			{
				retVal.Parents = category.Parents.ToDictionary(x => x.Id, x => x.Name);
			}
			//For virtual category links not needed
			if (!category.Virtual && category.Links != null)
			{
				retVal.Links = category.Links.Select(x => x.ToWebModel()).ToList();
			}

			if (category.SeoInfos != null)
			{
				retVal.SeoInfos = category.SeoInfos.Select(x => x.ToWebModel()).ToList();
			}

			retVal.Properties = new List<webModel.Property>();
			//Need add property for each meta info
			if (properties != null)
			{
				retVal.Properties = new List<webModel.Property>();
				foreach (var property in properties)
				{
					var webModelProperty = property.ToWebModel();
					webModelProperty.Values = new List<webModel.PropertyValue>();
					webModelProperty.IsManageable = true;
					webModelProperty.IsReadOnly = property.Type != moduleModel.PropertyType.Category;
					retVal.Properties.Add(webModelProperty);
				}
			}

			//Populate property values
			if (category.PropertyValues != null)
			{
				foreach (var propValue in category.PropertyValues.Select(x => x.ToWebModel()))
				{
					var property = retVal.Properties.FirstOrDefault(x => x.IsSuitableForValue(propValue));
					if (property == null)
					{
						//Need add dummy property for each value without property
						property = new webModel.Property(propValue, category.CatalogId, category.Id, moduleModel.PropertyType.Category);
						retVal.Properties.Add(property);
					}
					property.Values.Add(propValue);
				}
			}

			return retVal;
		}

		public static moduleModel.Category ToModuleModel(this webModel.Category category)
		{
			var retVal = new moduleModel.Category();
			retVal.InjectFrom(category);

			if (category.Links != null)
			{
				retVal.Links = category.Links.Select(x => x.ToModuleModel()).ToList();
			}

			if (category.SeoInfos != null)
			{
				retVal.SeoInfos = category.SeoInfos.Select(x => x.ToModuleModel()).ToList();
			}

			if (category.Properties != null)
			{
				retVal.PropertyValues = new List<moduleModel.PropertyValue>();
				foreach (var property in category.Properties)
				{
					foreach(var propValue in property.Values)
					{
						propValue.ValueType = property.ValueType;
						//Need populate required fields
						propValue.PropertyName = property.Name;
						retVal.PropertyValues.Add(propValue.ToModuleModel());
					}
				}
				
			}

		    retVal.IsActive = true;
			return retVal;
		}


	}
}
