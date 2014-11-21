using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.Foundation.Marketing.Model;
using module = VirtoCommerce.MerchandisingModule.Model;

namespace VirtoCommerce.MerchandisingModule.Data.Convertors
{
    public static class DynamicContentConvertor
    {
        public static module.DynamicContentItem ToModuleModel(this foundation.DynamicContent.DynamicContentItem contentItem)
        {
            if (contentItem == null)
                throw new ArgumentNullException("contentItem");

            var retVal = new module.DynamicContentItem()
            {
                Id = contentItem.DynamicContentItemId,
                Name = contentItem.Name,
                Description = contentItem.Description,
                ContentType = contentItem.ContentTypeId,
                IsMultilingual = contentItem.IsMultilingual
            };

            retVal.Properties = new module.PropertyDictionary();
            foreach (var value in contentItem.PropertyValues)
            {
                retVal.Properties.Add(value.Name, value.ToString());
            }

            return retVal;
        }
    }
}
