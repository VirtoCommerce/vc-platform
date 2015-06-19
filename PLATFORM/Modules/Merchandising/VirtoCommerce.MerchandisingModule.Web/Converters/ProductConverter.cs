using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class ProductConverter
    {
        #region Public Methods and Operators
     
        public static webModel.CatalogItem ToWebModel(this coreModel.CatalogProduct product, IBlobUrlResolver blobUrlResolver = null, coreModel.Property[] properties = null)
        {
            webModel.CatalogItem retVal = new webModel.Product();
			if (product.MainProductId != null)
            {
                retVal = new webModel.ProductVariation();
            }
            retVal.InjectFrom(product);

            if (product.Assets != null)
            {
                retVal.Images = product.Assets.Where(x => x.Type == coreModel.ItemAssetType.Image)
											  .Select(x => x.ToImageWebModel(blobUrlResolver))
											  .ToArray();
				retVal.Assets = product.Assets.Where(x => x.Type == coreModel.ItemAssetType.File)
											  .Select(x => x.ToAssetWebModel(blobUrlResolver))
											  .ToArray();
            }

            if (product.Variations != null && product.Variations.Any())
            {
                ((webModel.Product)retVal).Variations = product.Variations.Select(x => x.ToWebModel(blobUrlResolver, properties))
																		  .OfType<webModel.ProductVariation>()
																		  .ToArray();
            }

            if (product.Reviews != null)
            {
                retVal.EditorialReviews = product.Reviews.Select(x => new webModel.EditorialReview().InjectFrom(x))
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

			
			if(product.PropertyValues != null)
			{
				retVal.Properties = new webModel.PropertyDictionary();
				retVal.VariationProperties = new webModel.PropertyDictionary();
				
				// dictionary properties are returned as object[], all other properties are returned as primitives
				foreach (var propValueGroup in product.PropertyValues.GroupBy(x => x.PropertyName))
				{
					var propertyValue = propValueGroup.FirstOrDefault(x => x.Value != null);
					var propertyCollection = retVal.Properties;
					if (propertyValue != null)
					{
						if(properties != null)
						{
							var propertyMetaInfo = properties.FirstOrDefault(x => string.Equals(propValueGroup.Key, x.Name));
							if(propertyMetaInfo != null && propertyMetaInfo.Type == coreModel.PropertyType.Variation)
							{
								propertyCollection = retVal.VariationProperties;
							}
						}
						propertyCollection.Add(propValueGroup.Key, propertyValue.Value);
					}
				}


			}
       
            return retVal;
        }

        #endregion
    }
}
