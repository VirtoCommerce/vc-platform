using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ProductPropertyConverter
    {
        public static ProductProperty ToShopifyModel(this storefrontModel.Catalog.ProductProperty property)
        {
            var retVal = new ProductProperty();
            retVal.InjectFrom(property);
            return retVal;
        }
    }
}
