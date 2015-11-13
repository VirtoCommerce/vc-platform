using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Asset;
using AmazonMWSClientLib.Model.Feeds;
using System.Linq;

namespace Amazon.MerchantModule.Web.Converters
{
    public static class ProductConverter
    {
        //initial version of product converter to amazon product.
        //it should be adopted to the particular customer needs as many Amazon properties (like category) are unique and can't be mapped automatically.
        public static Product ToAmazonModel(this moduleModel.CatalogProduct product, IBlobUrlResolver assetUrlResolver, moduleModel.Property[] properties = null)
        {
            var amazonProduct = new Product();
            amazonProduct.InjectFrom(product);

            amazonProduct.DescriptionData = new ProductDescriptionData
            {
                Brand = "Brand",
                Description = "Product description",

            };

            amazonProduct.Condition = new ConditionInfo { ConditionType = ConditionType.New };
            if (product.Images != null && product.Images.Any())
                amazonProduct.ExternalProductUrl = assetUrlResolver.GetAbsoluteUrl(product.Images.First().Url.TrimStart('/'));
            amazonProduct.SKU = product.Code;
            amazonProduct.StandardProductID = new StandardProductID { Value = amazonProduct.SKU, Type = StandardProductIDType.ASIN };

            //var mainCat = new Home();
            //var subCat = new Kitchen();
            //mainCat.ProductType = new HomeProductType { Item = subCat };

            //amazonProduct.ProductData = new ProductProductData { Item = mainCat };

            return amazonProduct;
        }        
    }
}