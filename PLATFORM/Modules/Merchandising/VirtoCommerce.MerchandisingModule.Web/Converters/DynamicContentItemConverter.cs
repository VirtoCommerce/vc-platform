using moduleModel = VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class DynamicContentItemConverter
    {
        #region Public Methods and Operators

        public static moduleModel.DynamicContentItem ToModuleModel(this webModel.DynamicContentItem contentItem)
        {
            var retVal = new moduleModel.DynamicContentItem();
            //TODO:
            return retVal;
        }

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

        #endregion
    }
}
