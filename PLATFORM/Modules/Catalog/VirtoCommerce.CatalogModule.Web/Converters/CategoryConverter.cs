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
			retVal.Properties = new List<webModel.Property>();
			//Need add property for each meta info
			if(properties != null)
			{
				retVal.Properties = new List<webModel.Property>();
				foreach(var property in properties)
				{
					var webModelProperty = property.ToWebModel();
					webModelProperty.Values = new List<webModel.PropertyValue>();
					webModelProperty.IsManageable = true;
					webModelProperty.IsReadOnly = property.Type != moduleModel.PropertyType.Category;
					retVal.Properties.Add(webModelProperty);
				}
			}

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

			//Populate property values
			if (category.PropertyValues != null)
			{
				foreach (var propValue in category.PropertyValues)
				{
					var property = retVal.Properties.FirstOrDefault(x => x.Id == propValue.PropertyId);
					if (property == null)
					{
						//Need add dummy property for each value without property
						property = new webModel.Property
						{
							Catalog = retVal.Catalog,
							CatalogId = category.CatalogId,
							Category = retVal,
							CategoryId = category.Id,
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
						property.Values = category.PropertyValues
														  .Where(x => x.PropertyId == property.Id)
														  .Select(x => x.ToWebModel())
														  .ToList();
					}
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
