using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class CategoryConverter
	{
		public static webModel.Category ToWebModel(this moduleModel.Category category)
		{
			var retVal = new webModel.Category();
			retVal.InjectFrom(category);

			if (category.Parents != null)
			{
				retVal.Parents = category.Parents.ToDictionary(x => x.Id, x => x.Name);
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
