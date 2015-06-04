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
	public static class EditorialReviewConverter
	{
		public static webModel.EditorialReview ToWebModel(this moduleModel.EditorialReview review)
		{
			var retVal = new webModel.EditorialReview();
			retVal.InjectFrom(review);
			return retVal;
		}

		public static moduleModel.EditorialReview ToModuleModel(this webModel.EditorialReview review)
		{
			var retVal = new moduleModel.EditorialReview();
			retVal.InjectFrom(review);
			return retVal;
		}


	}
}
