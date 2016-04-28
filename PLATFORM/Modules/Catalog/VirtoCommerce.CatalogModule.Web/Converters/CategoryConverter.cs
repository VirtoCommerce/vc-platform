using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class CategoryConverter
    {
        public static webModel.Category ToWebModel(this moduleModel.Category category, IBlobUrlResolver blobUrlResolver = null, bool convertProps = true)
        {
            var retVal = new webModel.Category();
            retVal.InjectFrom(category);
            retVal.Catalog = category.Catalog.ToWebModel();
            //Reset properties for size economy
            retVal.Catalog.Properties = null;
            retVal.SeoInfos = category.SeoInfos;
            retVal.Outlines = category.Outlines;

            if (category.Parents != null)
            {
                retVal.Parents = new List<webModel.Category>();
                foreach (var parent in category.Parents.Select(x => x.ToWebModel()))
                {
                    //Reset some props to decrease size of resulting json
                    parent.Catalog = null;
                    parent.Properties = null;
                    retVal.Parents.Add(parent);
                }
            }
            //For virtual category links not needed
            if (!category.IsVirtual && category.Links != null)
            {
                retVal.Links = category.Links.Select(x => x.ToWebModel()).ToList();
            }

            //Need add property for each meta info
            retVal.Properties = new List<webModel.Property>();
            if (convertProps)
            {
                foreach (var property in category.Properties)
                {
                    var webModelProperty = property.ToWebModel();
                    //Reset dict values to decrease response size
                    webModelProperty.DictionaryValues = null;
                    webModelProperty.Values = new List<webModel.PropertyValue>();
                    webModelProperty.IsManageable = true;
                    webModelProperty.IsReadOnly = property.Type != moduleModel.PropertyType.Category;
                    retVal.Properties.Add(webModelProperty);
                }

                //Populate property values
                if (category.PropertyValues != null)
                {
                    foreach (var propValue in category.PropertyValues.Select(x => x.ToWebModel()))
                    {
                        var property = retVal.Properties.FirstOrDefault(x => x.Id == propValue.PropertyId);
                        if (property == null)
                        {
                            property = retVal.Properties.FirstOrDefault(x => x.Name.EqualsInvariant(propValue.PropertyName));
                        }
                        if (property == null)
                        {
                            //Need add dummy property for each value without property
                            property = new webModel.Property(propValue, category.CatalogId, category.Id, moduleModel.PropertyType.Category);
                            retVal.Properties.Add(property);
                        }
                        property.Values.Add(propValue);
                    }
                }
            }
            if (category.Images != null)
            {
                retVal.Images = category.Images.Select(x => x.ToWebModel(blobUrlResolver)).ToList();
            }
            return retVal;
        }

        public static moduleModel.Category ToModuleModel(this webModel.Category category)
        {
            var retVal = new moduleModel.Category();
            retVal.InjectFrom(category);
            retVal.SeoInfos = category.SeoInfos;

            if (category.Links != null)
            {
                retVal.Links = category.Links.Select(x => x.ToModuleModel()).ToList();
            }


            if (category.Properties != null)
            {
                retVal.PropertyValues = new List<moduleModel.PropertyValue>();
                foreach (var property in category.Properties)
                {
                    foreach (var propValue in property.Values)
                    {
                        propValue.ValueType = property.ValueType;
                        //Need populate required fields
                        propValue.PropertyName = property.Name;
                        retVal.PropertyValues.Add(propValue.ToModuleModel());
                    }
                }
            }

            if (category.Images != null)
            {
                retVal.Images = category.Images.Select(x => x.ToCoreModel()).ToList();
                var index = 0;
                foreach (var image in retVal.Images)
                {
                    image.SortOrder = index++;
                }
            }

            return retVal;
        }


    }
}
