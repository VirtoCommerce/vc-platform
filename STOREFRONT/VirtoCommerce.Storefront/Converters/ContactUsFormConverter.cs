using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ContactUsFormConverter
    {
        public static VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest ToServiceModel(this ContactUsForm contactUsForm, WorkContext workContext)
        {
            var retVal = new VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest();
            retVal.Language = workContext.CurrentLanguage.CultureName;
            retVal.StoreId = workContext.CurrentStore.Id;
            retVal.Type = contactUsForm.FormType;
            retVal.Fields = contactUsForm.Contact.ToDictionary(x => x.Key, x => (object)((string[])x.Value).FirstOrDefault());
            return retVal;
        }
    }
}
