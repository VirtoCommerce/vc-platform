using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ContactUsFormConverter
    {
        public static Form ToShopifyModel(this StorefrontModel.ContactUsForm contactUsForm)
        {
            var retVal = new Form();
            if(contactUsForm.Contact != null)
            {
                retVal.Properties = contactUsForm.Contact;
            }
          
            return retVal;
        }
    }
}