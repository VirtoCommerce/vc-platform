using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Assets.Services;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class ProductConverter
    {
        #region Public Methods and Operators

        public static moduleModel.CatalogProduct ToModuleModel(this webModel.CatalogItem catalogItem)
        {
            var retVal = new moduleModel.CatalogProduct();
            retVal.InjectFrom(catalogItem);

            if (catalogItem.Images != null)
            {
                retVal.Assets = new List<moduleModel.ItemAsset>();
                var isMain = true;
                foreach (var productImage in catalogItem.Images)
                {
                    var image = productImage.ToModuleModel();
                    image.Type = moduleModel.ItemAssetType.Image;
                    image.Group = isMain ? "primaryimage" : "images";
                    retVal.Assets.Add(image);
                    isMain = false;
                }
            }

            if (catalogItem.Properties != null)
            {
                retVal.PropertyValues = new List<moduleModel.PropertyValue>();
                foreach (var keyValue in catalogItem.Properties)
                {
                    var propValue = new moduleModel.PropertyValue
                                    {
                                        PropertyName = keyValue.Key,
                                        Value = keyValue.Value.ToString()
                                    };
                }
            }

            return retVal;
        }

        public static webModel.CatalogItem ToWebModel(
            this moduleModel.CatalogProduct product,
            IAssetUrl resolver = null,
            webModel.Product parentProduct = null)
        {
            webModel.CatalogItem retVal = new webModel.Product();
            if (parentProduct != null)
            {
                retVal = new webModel.ProductVariation();
            }
            retVal.InjectFrom(product);

            if (product.Assets != null)
            {
                retVal.Images =
                    product.Assets.Where(x => x.Type == moduleModel.ItemAssetType.Image)
                        .Select(x => x.ToWebModel(resolver))
                        .ToArray();
            }

            if (product.Variations != null && product.Variations.Any())
            {
                ((webModel.Product)retVal).Variations =
                    product.Variations.Select(x => x.ToWebModel(resolver, (webModel.Product)retVal))
                        .OfType<webModel.ProductVariation>()
                        .ToArray();
            }

            if (product.Reviews != null)
            {
                retVal.EditorialReviews =
                    product.Reviews.Select(x => new webModel.EditorialReview().InjectFrom(x))
                        .Cast<webModel.EditorialReview>()
                        .ToArray();
            }

            if (product.Links != null)
            {
                retVal.Categories = product.Links.Select(x => x.CategoryId).ToArray();
            }

            if (product.SeoInfos != null)
            {
                retVal.Seo = product.SeoInfos.Select(x => x.ToWebModel()).ToArray();
            }

            if (product.Associations != null && product.Associations.Any())
            {
                retVal.Associations = product.Associations.Select(x => x.ToWebModel()).ToArray();
            }

            retVal.Properties = new webModel.PropertyDictionary();

            //Need add property for each meta info
            /* SASHA: no need to group elements here, simply return one key per value
			foreach (var propValueGroup in product.PropertyValues.GroupBy(x=>x.PropertyName))
			{
                retVal.Properties.Add(propValueGroup.Key, propValueGroup.Select(g=>g.Value));
			}
             * */

            // dictionary properties are returned as object[], all other properties are returned as primitives
            foreach (var propValueGroup in product.PropertyValues.GroupBy(x => x.PropertyName))
            {
                var val = propValueGroup.Select(g => g.Value);
                if (val.Any())
                {
                    retVal.Properties.Add(propValueGroup.Key, val.Count() > 1 ? val : val.First());
                }
            }

            /*
		    foreach (var propValue in product.PropertyValues)
		    {
                //TODO create property collection not Dictionary to support multivalues
                if (retVal.Properties.ContainsKey(propValue.PropertyName))continue;
		        
                retVal.Properties.Add(propValue.PropertyName, propValue.Value);
		    }
             * */

            return retVal;
        }

        #endregion
    }
}
