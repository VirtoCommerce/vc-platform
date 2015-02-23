using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.AppConfig.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class CategoryConverter
	{
		public static webModel.Category ToWebModel(this moduleModel.Category category)
		{
			var retVal = new webModel.Category();
			retVal.InjectFrom(category);

			if (category.Parents != null && category.Parents.Any())
			{
			    retVal.Parents = category.Parents.Select(x => x.ToWebModel());
			}

		    if (category.SeoInfos != null)
		    {
		        retVal.Seo = category.SeoInfos.Select(x => x.ToWebModel());
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
