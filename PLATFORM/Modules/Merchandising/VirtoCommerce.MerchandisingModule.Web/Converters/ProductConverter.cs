using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using coreInvModel = VirtoCommerce.Domain.Inventory.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class ProductConverter
    {
        #region Public Methods and Operators

        public static webModel.CatalogItem ToWebModel(this coreModel.CatalogProduct product, IBlobUrlResolver blobUrlResolver = null, coreModel.Property[] properties = null, coreInvModel.InventoryInfo inventory = null)
        {
            webModel.CatalogItem retVal = new webModel.Product();
			if (product.MainProductId != null)
            {
                retVal = new webModel.ProductVariation();
            }
            retVal.InjectFrom(product);

			if(product.Images != null && product.Images.Any())
			{
				//Back compability check group to detect primary image (remove later)
				var primaryImage = product.Images.FirstOrDefault(x => String.Equals(x.Group, "primaryimage", StringComparison.InvariantCultureIgnoreCase));
				if(primaryImage == null)
				{
					primaryImage = product.Images.OrderBy(x => x.SortOrder).First();
				}
				retVal.PrimaryImage = primaryImage.ToWebModel(blobUrlResolver);
				retVal.Images = product.Images.Skip(1).Select(x => x.ToWebModel(blobUrlResolver)).ToArray();
			}
            if (product.Assets != null)
            {
				retVal.Assets = product.Assets.Select(x => x.ToWebModel(blobUrlResolver)).ToArray();
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
					var displayName = propValueGroup.Key;
					var propertyValue = propValueGroup.FirstOrDefault(x => x.Value != null);
					var propertyCollection = retVal.Properties;
					if (propertyValue != null)
					{
						if(properties != null)
						{
							var propertyMetaInfo = properties.FirstOrDefault(x => string.Equals(propValueGroup.Key, x.Name, StringComparison.OrdinalIgnoreCase));
							if(propertyMetaInfo != null && propertyMetaInfo.DisplayNames != null)
							{
								//TODO: use display name for specific language
								displayName = propertyMetaInfo.DisplayNames.Select(x=>x.Name).FirstOrDefault(x => !String.IsNullOrEmpty(x)) ?? displayName;
							}

							if(propertyMetaInfo != null && propertyMetaInfo.Type == coreModel.PropertyType.Variation)
							{
								propertyCollection = retVal.VariationProperties;
							}
						}
						propertyCollection.Add(displayName, propertyValue.Value);
					}
				}
			}

            if (inventory != null)
            {
                retVal.Inventory = inventory.ToWebModel();
            }

            return retVal;
        }
        #endregion
    }
}
