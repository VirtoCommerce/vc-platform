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

            webModel.MenuLinks = serviceModel.MenuLinks.Select(ml => ml.ToWebModel(urlBuilder)).ToList();
            webModel.SecurityScopes = serviceModel.SecurityScopes != null ? serviceModel.SecurityScopes.Select(ss => ss).ToList() : null;

            return webModel;
        }

        public static MenuLink ToWebModel(this VirtoCommerceContentWebModelsMenuLink serviceModel, IStorefrontUrlBuilder urlBuilder)
        {
            var webModel = new MenuLink();

            webModel.InjectFrom(serviceModel);

            webModel.SecurityScopes = serviceModel.SecurityScopes != null ? serviceModel.SecurityScopes.Select(ss => ss).ToList() : null;
            webModel.Url = urlBuilder.ToAppAbsolute("/" + serviceModel.Url);

            return webModel;
        }
    }
}