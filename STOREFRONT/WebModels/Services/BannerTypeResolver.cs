using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.Web.Models.Banners;
using VirtoCommerce.Web.Models.Convertors;

namespace VirtoCommerce.Web.Models.Services
{
    public class BannerTypeResolver
    {
        public Banner ResolveBannerFromContent(DynamicContentItem item)
        {
            var contentType = item.ContentType.ToLowerInvariant();
            switch (contentType)
            {
                case "productwithimageandprice":
                    return item.AsWebModel<ProductWithImageAndPriceBanner>();
                case "categoryurl":
                    return item.AsWebModel<CategorySearchBanner>();
            }

            return item.AsWebModel();

            /*
    if (item.ContentType == DynamicContentType.Html.ToString())
    {
        @Html.DisplayForModel("RawHtml", new { Model = new RawHtmlModel(item) })
    }
    
    if (item.ContentTypeId == DynamicContentType.Razor.ToString())
    {
        @Html.DisplayForModel("RawHtml", new { Model = new RazorHtmlModel(item, ViewContext) })
    }

    if (item.ContentTypeId == DynamicContentType.Flash.ToString())
    {
        @Html.DisplayForModel("Flash", new { Model = new FlashModel(item) })
    }

    if (item.ContentTypeId == DynamicContentType.ImageNonClickable.ToString())
    {
        @Html.DisplayForModel("ImageNoClick", new { Model = new ImageNonClickableModel(item) })
    }

    if (item.ContentTypeId == DynamicContentType.ImageClickable.ToString())
    {
        @Html.DisplayForModel("ImageClick", new { Model = new ImageClickableModel(item) })
    }

    if (item.ContentTypeId == DynamicContentType.ProductWithImageAndPrice.ToString())
    {
        @Html.DisplayForModel("ProductImageAndPrice", new { Model = new ProductWithImageAndPriceModel(item) })
    }

    if (item.ContentTypeId == DynamicContentType.CategoryUrl.ToString())
    {
        @Html.DisplayForModel("CategoryUrl", new { Model = new CategoryUrlModel(item) })
    }
             * */
            
        }
    }
}
