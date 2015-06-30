using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class CategoryConverter
    {
        #region Public Methods and Operators

		public static webModel.Category ToWebModel(this moduleModel.Category category, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new webModel.Category();
            retVal.InjectFrom(category);

            if (category.Parents != null && category.Parents.Any())
            {
                //retVal.Parents = category.Parents.Select(x => x.ToWebModel(keywords != null ? keywords.Where(k => k.KeywordValue == x.Id) : null));
				retVal.Parents = category.Parents.Select(x => x.ToWebModel(blobUrlResolver));
            }

            if (category.SeoInfos != null)
            {
                retVal.Seo = category.SeoInfos.Select(x => x.ToWebModel());
            }

			if(category.Images != null && category.Images.Any())
			{
				retVal.Image = category.Images.First().ToWebModel(blobUrlResolver);
			}
            return retVal;
        }

        #endregion
    }
}
