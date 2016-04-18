using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class ProductConverter
    {
        public static webModel.Product ToWebModel(this moduleModel.CatalogProduct product, IBlobUrlResolver blobUrlResolver)
        {
            var retVal = new webModel.Product();
            retVal.InjectFrom(product);

            retVal.SeoInfos = product.SeoInfos;
            retVal.Outlines = product.Outlines;

            if (product.Catalog != null)
            {
                retVal.Catalog = product.Catalog.ToWebModel();
                //Reset catalog properties and languages for response size economy
                retVal.Catalog.Properties = null;
            }

            if (product.Category != null)
            {
                retVal.Category = product.Category.ToWebModel(blobUrlResolver);
                //Reset  category catalog, properties  for response size economy
                retVal.Category.Catalog = null;
                retVal.Category.Properties = null;
            }

            if (product.Images != null)
            {
                retVal.Images = product.Images.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }

            if (product.Assets != null)
            {
                retVal.Assets = product.Assets.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }

            if (product.Links != null)
            {
                retVal.Links = product.Links.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Reviews != null)
            {
                retVal.Reviews = product.Reviews.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Associations != null)
            {
                retVal.Associations = product.Associations.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }
            //Init parents
            if (product.Category != null)
            {
                retVal.Parents = new List<webModel.Category>();
                if (product.Category.Parents != null)
                {
                    retVal.Parents.AddRange(product.Category.Parents.Select(x => x.ToWebModel()));
                }
                retVal.Parents.Add(product.Category.ToWebModel());
                foreach (var parent in retVal.Parents)
                {
                    //Reset some props to decrease size of resulting json
                    parent.Catalog = null;
                    parent.Properties = null;
                }
            }

            retVal.TitularItemId = product.MainProductId;

            retVal.Properties = new List<webModel.Property>();
            //Need add property for each meta info
            if (product.Properties != null)
            {
                foreach (var property in product.Properties)
                {
                    var webModelProperty = property.ToWebModel();
                    //Reset dict values to decrease response size
                    webModelProperty.DictionaryValues = null;
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
                foreach (var propValue in product.PropertyValues.Select(x => x.ToWebModel()))
                {
                    var property = retVal.Properties.FirstOrDefault(x => x.Id == propValue.PropertyId);
                    if (property == null)
                    {
                        property = retVal.Properties.FirstOrDefault(x => x.Name.EqualsInvariant(propValue.PropertyName));
                    }
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
            retVal.SeoInfos = product.SeoInfos;

            if (product.Images != null)
            {
                retVal.Images = product.Images.Select(x => x.ToCoreModel()).ToList();
                var index = 0;
                foreach (var image in retVal.Images)
                {
                    image.SortOrder = index++;
                }
            }

            if (product.Assets != null)
            {
                retVal.Assets = product.Assets.Select(x => x.ToCoreModel()).ToList();
            }

            if (product.Properties != null)
            {
                retVal.PropertyValues = new List<moduleModel.PropertyValue>();
                foreach (var property in product.Properties)
                {
                    if (property.Values != null)
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
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(x => x.ToModuleModel(blobUrlResolver)).ToList();
            }

            if (product.Links != null)
            {
                retVal.Links = product.Links.Select(x => x.ToModuleModel()).ToList();
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
