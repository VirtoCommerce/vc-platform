using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class MenuLinkConverter
    {
        public static MenuLinkList ToWebModel(this VirtoCommerceContentWebModelsMenuLinkList serviceModel)
        {
            var webModel = new MenuLinkList();

            webModel.InjectFrom(serviceModel);

            webModel.Language = string.IsNullOrEmpty(serviceModel.Language) ? Language.InvariantLanguage : new Language(serviceModel.Language);

            if (serviceModel.MenuLinks != null)
            {
                webModel.MenuLinks = serviceModel.MenuLinks.Select(ml => ml.ToWebModel()).ToList();
            }
          
            return webModel;
        }

        public static MenuLink ToWebModel(this VirtoCommerceContentWebModelsMenuLink serviceModel)
        {
            var webModel = new MenuLink();

            if (serviceModel.AssociatedObjectType != null)
            {
                if ("product" == serviceModel.AssociatedObjectType.ToLowerInvariant())
                {
                    webModel = new ProductMenuLink();
                }
                else if ("category" == serviceModel.AssociatedObjectType.ToLowerInvariant())
                {
                    webModel = new CategoryMenuLink();
                }
            }

            webModel.InjectFrom(serviceModel);

            return webModel;
        }
    }
}