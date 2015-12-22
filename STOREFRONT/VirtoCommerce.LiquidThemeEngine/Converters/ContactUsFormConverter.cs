using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ContactUsConverter
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
