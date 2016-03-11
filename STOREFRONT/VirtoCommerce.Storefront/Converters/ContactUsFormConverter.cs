using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

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
            retVal.Fields = contactUsForm.Contact.ToDictionary(x => x.Key, x => x.Value != null ? x.Value.ToString() : string.Empty);
            return retVal;
        }
    }
}
