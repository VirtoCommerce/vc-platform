using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class MenuLinkConverter
    {
        public static MenuLinkList ToWebModel(this VirtoCommerceContentWebModelsMenuLinkList serviceModel, IStorefrontUrlBuilder urlBuilder)
        {
            var webModel = new MenuLinkList();

            webModel.InjectFrom(serviceModel);

            if (serviceModel.MenuLinks != null)
            {
                webModel.MenuLinks = serviceModel.MenuLinks.Select(ml => ml.ToWebModel(urlBuilder)).ToList();
            }
            if (serviceModel.SecurityScopes != null)
            {
                webModel.SecurityScopes = serviceModel.SecurityScopes.Select(ss => ss).ToList();
            }

            return webModel;
        }

        public static MenuLink ToWebModel(this VirtoCommerceContentWebModelsMenuLink serviceModel, IStorefrontUrlBuilder urlBuilder)
        {
            var webModel = new MenuLink();

            webModel.InjectFrom(serviceModel);

            if (serviceModel.SecurityScopes != null)
            {
                webModel.SecurityScopes = serviceModel.SecurityScopes.Select(ss => ss).ToList();
            }

            webModel.Url = urlBuilder.ToAppAbsolute("/" + serviceModel.Url);

            return webModel;
        }
    }
}