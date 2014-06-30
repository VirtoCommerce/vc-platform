using System.Collections.Generic;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    public static class PropertySets
    {
        public static List<DynamicContentItemProperty> GetPropertySetByItemType(DynamicContentType itemType)
        {
            List<DynamicContentItemProperty> retVal = null;

            switch (itemType)
            {
                case DynamicContentType.CategoryWithImage:
                    var propCat = new DynamicContentItemProperty();
                    propCat.Name = "CategoryId";
                    propCat.ValueType = (int)PropertyValueType.Category;

                    var propCat1 = new DynamicContentItemProperty();
                    propCat1.Name = "ImageUrl";
                    propCat1.ValueType = (int)PropertyValueType.Image;

                    var propCat2 = new DynamicContentItemProperty();
                    propCat2.Name = "ExternalImageUrl";
                    propCat2.ValueType = (int)PropertyValueType.Image;

                    var propCat3 = new DynamicContentItemProperty();
                    propCat3.Name = "Message";
                    propCat3.ValueType = (int)PropertyValueType.LongString;

                    retVal = new List<DynamicContentItemProperty>() { propCat, propCat1, propCat2, propCat3 };
                    break;
                case DynamicContentType.Flash:
                    var propFlash = new DynamicContentItemProperty();
                    propFlash.Name = "FlashFilePath";
                    propFlash.ValueType = (int)PropertyValueType.Image;

                    var propFlash1 = new DynamicContentItemProperty();
                    propFlash1.Name = "Link1Url";
                    propFlash1.ValueType = (int)PropertyValueType.LongString;

                    var propFlash2 = new DynamicContentItemProperty();
                    propFlash2.Name = "Link2Url";
                    propFlash2.ValueType = (int)PropertyValueType.LongString;

                    var propFlash3 = new DynamicContentItemProperty();
                    propFlash3.Name = "Link3Url";
                    propFlash3.ValueType = (int)PropertyValueType.LongString;

                    retVal = new List<DynamicContentItemProperty>() { propFlash, propFlash1, propFlash2, propFlash3 };
                    break;
                case DynamicContentType.Html:
                    var propHtml = new DynamicContentItemProperty();
                    propHtml.Name = "RawHtml";
                    propHtml.ValueType = (int)PropertyValueType.LongString;

                    retVal = new List<DynamicContentItemProperty>() { propHtml };
                    break;
                case DynamicContentType.Razor:
                    var propRazor = new DynamicContentItemProperty();
                    propRazor.Name = "RazorHtml";
                    propRazor.ValueType = (int)PropertyValueType.LongString;

                    retVal = new List<DynamicContentItemProperty> { propRazor };
                    break;
                case DynamicContentType.ImageClickable:
                    var propImageClickable1 = new DynamicContentItemProperty();
                    propImageClickable1.Name = "AlternativeText";
                    propImageClickable1.ValueType = (int)PropertyValueType.LongString;

                    var propImageClickable2 = new DynamicContentItemProperty();
                    propImageClickable2.Name = "ImageUrl";
                    propImageClickable2.ValueType = (int)PropertyValueType.Image;

                    var propImageClickable3 = new DynamicContentItemProperty();
                    propImageClickable3.Name = "TargetUrl";
                    propImageClickable3.ValueType = (int)PropertyValueType.LongString;

                    var propImageClickable4 = new DynamicContentItemProperty();
                    propImageClickable4.Name = "Title";
                    propImageClickable4.ValueType = (int)PropertyValueType.LongString;

                    retVal = new List<DynamicContentItemProperty>() { propImageClickable1, propImageClickable2, propImageClickable3, propImageClickable4 };
                    break;
                case DynamicContentType.ImageNonClickable:
                    var propImageNotClickable1 = new DynamicContentItemProperty();
                    propImageNotClickable1.Name = "AlternativeText";
                    propImageNotClickable1.ValueType = (int)PropertyValueType.LongString;

                    var propImageNotClickable2 = new DynamicContentItemProperty();
                    propImageNotClickable2.Name = "ImageFilePath";
                    propImageNotClickable2.ValueType = (int)PropertyValueType.Image;

                    retVal = new List<DynamicContentItemProperty>() { propImageNotClickable1, propImageNotClickable2 };
                    break;
                case DynamicContentType.ProductWithImageAndPrice:
                    var propProduct = new DynamicContentItemProperty();
                    propProduct.Name = "ProductCode";
                    propProduct.ValueType = (int)PropertyValueType.LongString;

                    retVal = new List<DynamicContentItemProperty>() { propProduct };
                    break;
                case DynamicContentType.CategoryUrl:
                    var propCatUrl = new DynamicContentItemProperty();
                    propCatUrl.Name = "CategoryCode";
                    propCatUrl.ValueType = (int)PropertyValueType.Category;

                    var propCatUrl1 = new DynamicContentItemProperty();
                    propCatUrl1.Name = "Title";
                    propCatUrl1.ValueType = (int)PropertyValueType.LongString;

                    var propCatSort = new DynamicContentItemProperty();
                    propCatSort.Name = "SortField";
                    propCatSort.ValueType = (int)PropertyValueType.ShortString;

                    var propCatItemsCount = new DynamicContentItemProperty();
                    propCatItemsCount.Name = "ItemCount";
                    propCatItemsCount.ValueType = (int)PropertyValueType.Integer;

                    var propCatItemsNew = new DynamicContentItemProperty();
                    propCatItemsNew.Name = "NewItems";
                    propCatItemsNew.ValueType = (int)PropertyValueType.Boolean;

                    retVal = new List<DynamicContentItemProperty>() { propCatUrl, propCatUrl1, propCatSort, propCatItemsCount, propCatItemsNew };
                    break;
                case DynamicContentType.Undefined:
                    break;
            }

            return retVal;
        }

    }
}
