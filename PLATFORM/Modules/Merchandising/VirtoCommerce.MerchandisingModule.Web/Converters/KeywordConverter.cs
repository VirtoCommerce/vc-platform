using Omu.ValueInjecter;
using VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class KeywordConverter
    {
        #region Public Methods and Operators

        public static webModel.SeoKeyword ToWebModel(this SeoInfo keyword)
        {
            var retVal = new webModel.SeoKeyword();
            retVal.InjectFrom(keyword);
            return retVal;
        }

   
        #endregion
    }
}
