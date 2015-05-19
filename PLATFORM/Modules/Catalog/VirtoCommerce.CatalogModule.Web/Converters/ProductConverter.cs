using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class ProductConverter
    {
        public static webModel.Product ToWebModel(this moduleModel.CatalogProduct product, IBlobUrlResolver blobUrlResolver, moduleModel.Property[] properties = null)
        {
            var retVal = new webModel.Product();
            retVal.InjectFrom(product);

            if (product.Catalog != null)
            {
                retVal.Catalog = product.Catalog.ToWebModel();
            }

            if (product.Category != null)
            {
                retVal.Category = product.Category.ToWebModel();
            }

            if (product.Assets != null)
            {
                var assetBases = product.Assets.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
                retVal.Images = assetBases.OfType<webModel.ProductImage>().ToList();
                retVal.Assets = assetBases.OfType<webModel.ProductAsset>().ToList();
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }

            if (product.Links != null)
            {
                retVal.Links = product.Links.Select(x => x.ToWebModel()).ToList();
            }

            if (product.SeoInfos != null)
            {
                retVal.SeoInfos = product.SeoInfos.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Reviews != null)
            {
                retVal.Reviews = product.Reviews.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Associations != null)
            {
                retVal.Associations = product.Associations.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }
            retVal.TitularItemId = product.MainProductId;

            retVal.Properties = new List<webModel.Property>();
            //Need add property for each meta info
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var webModelProperty = property.ToWebModel();
                    webModelProperty.Category = null;
                    webModelProperty.Values = new List<webModel.PropertyValue>();
                    webModelProperty.IsManageable = true;
                    webModelProperty.IsReadOnly = property.Type != moduleModel.PropertyType.Product && property.Type != moduleModel.PropertyType.Variation;
                    retVal.Properties.Add(webModelProperty);
                }
            }

            //Populate property values
            if (product.PropertyValues != null)
            {
                foreach (var propValue in product.PropertyValues.Select(x=>x.ToWebModel()))
                {
					var property = retVal.Properties.FirstOrDefault(x => x.IsSuitableForValue(propValue));
                    if (property == null)
					{  
						//Need add dummy property for each value without property
						property = new webModel.Property(propValue, product.CatalogId, product.CategoryId, moduleModel.PropertyType.Product);
                        retVal.Properties.Add(property);
                    }
					property.Values.Add(propValue);
                }
            }

            return retVal;
        }

        public static moduleModel.CatalogProduct ToModuleModel(this webModel.Product product, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new moduleModel.CatalogProduct();
            retVal.InjectFrom(product);
            if (product.Images != null)
            {
                retVal.Assets = new List<moduleModel.ItemAsset>();
                bool isMain = true;
                foreach (var productImage in product.Images)
                {
                    var image = productImage.ToModuleModel();
                    image.Type = moduleModel.ItemAssetType.Image;
                    image.Group = isMain ? "primaryimage" : "images";
                    retVal.Assets.Add(image);
                    isMain = false;
                }
            }

            if (product.Assets != null)
            {
                if (retVal.Assets == null)
                {
                    retVal.Assets = new List<moduleModel.ItemAsset>();
                }

                foreach (var productAsset in product.Assets)
                {
                    var asset = productAsset.ToModuleModel();
                    asset.Type = moduleModel.ItemAssetType.File;
                    retVal.Assets.Add(asset);
                }
            }

            if (product.Properties != null)
            {
                retVal.PropertyValues = new List<moduleModel.PropertyValue>();
                foreach (var property in product.Properties)
                {
                    foreach (var propValue in property.Values)
                    {
                        //Need populate required fields
                        propValue.PropertyName = property.Name;
                        propValue.ValueType = property.ValueType;
                        retVal.PropertyValues.Add(propValue.ToModuleModel());
                    }
                }
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(x => x.ToModuleModel(blobUrlResolver)).ToList();
            }

            if (product.Links != null)
            {
                retVal.Links = product.Links.Select(x => x.ToModuleModel()).ToList();
            }

            if (product.SeoInfos != null)
            {
                retVal.SeoInfos = product.SeoInfos.Select(x => x.ToModuleModel()).ToList();
            }

            if (product.Reviews != null)
            {
                retVal.Reviews = product.Reviews.Select(x => x.ToModuleModel()).ToList();
            }

            if (product.Associations != null)
            {
                retVal.Associations = product.Associations.Select(x => x.ToModuleModel()).ToList();
            }
            retVal.MainProductId = product.TitularItemId;

            return retVal;
        }
    }
}
