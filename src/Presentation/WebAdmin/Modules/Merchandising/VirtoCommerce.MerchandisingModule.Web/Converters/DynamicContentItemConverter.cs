using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class DynamicContentItemConverter
	{
		public static webModel.DynamicContentItem ToWebModel(this moduleModel.DynamicContentItem contentItem)
		{
			var retVal = new webModel.DynamicContentItem
			{
			    Id = contentItem.DynamicContentItemId,
			    Name = contentItem.Name,
			    Description = contentItem.Description,
			    ContentType = contentItem.ContentTypeId,
			    IsMultilingual = contentItem.IsMultilingual,
			    Properties = new webModel.PropertyDictionary()
			};

		    foreach (var value in contentItem.PropertyValues)
			{
				retVal.Properties.Add(value.Name, value.ToString());
			}

			return retVal;
		}

		public static moduleModel.DynamicContentItem ToModuleModel(this webModel.DynamicContentItem contentItem)
		{
			var retVal = new moduleModel.DynamicContentItem();
			//TODO:
			return retVal;
		}


	}
}
