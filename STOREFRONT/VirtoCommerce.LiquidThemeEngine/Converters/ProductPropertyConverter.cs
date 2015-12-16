using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ProductPropertyConverter
    {
        public static ProductProperty ToShopifyModel(this StorefrontModel.Catalog.ProductProperty property)
        {
            var result = new ProductProperty();

            result.InjectFrom(property);

            return result;
        }
    }
}