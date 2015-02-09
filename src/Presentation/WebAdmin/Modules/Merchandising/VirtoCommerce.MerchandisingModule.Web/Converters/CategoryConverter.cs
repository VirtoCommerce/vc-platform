using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.AppConfig.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class CategoryConverter
	{
		public static webModel.Category ToWebModel(this moduleModel.Category category, SeoUrlKeyword[] keywords = null)
		{
			var retVal = new webModel.Category();
			retVal.InjectFrom(category);

			if (category.Parents != null)
			{
			    retVal.Parents = category.Parents.Select(x => new webModel.CategoryInfo
			    {
                    Id = x.Id,
                    Name = x.Name,
                    SeoKeywords = keywords !=null ? keywords.Where(k => k.KeywordValue == x.Id).Select(k => k.ToWebModel()).ToArray() : null
			    }).ToArray();
			}

		    if (category.SeoInfos != null)
		    {
		        retVal.SeoKeywords = category.SeoInfos.Select(x => x.ToWebModel()).ToArray();
		    }

			return retVal;
		}

		public static moduleModel.Category ToModuleModel(this webModel.Category category)
		{
			var retVal = new moduleModel.Category();
			retVal.InjectFrom(category);
	
			return retVal;
		}


	}
}
