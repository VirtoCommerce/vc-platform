using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.ApiWebClient.Extensions;

namespace VirtoCommerce.Web.Models
{
    public enum DynamicContentType
    {
        Undefined,
        CategoryWithImage,
        CategoryUrl,
        Flash,
        Html,
        Razor,
        ImageClickable,
        ImageNonClickable,
        ProductWithImageAndPrice
    }
    /// <summary>
    /// Class BannerModel.
    /// </summary>
    public class BannerModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerModel" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public BannerModel(ICollection<DynamicContentItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public ICollection<DynamicContentItem> Items { get; set; }
    }

    /// <summary>
    /// Class RawHtmlModel.
    /// </summary>
    public class RawHtmlModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawHtmlModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public RawHtmlModel(DynamicContentItem item)
        {
            foreach (var prop in item.Properties.Where(prop => String.Equals(prop.Key, "RawHtml", StringComparison.InvariantCultureIgnoreCase)))
            {
                Html = prop.Value;
                break;
            }
        }

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>The HTML.</value>
        public string Html { get; set; }
    }

    public class RazorHtmlModel : RawHtmlModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawHtmlModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context"></param>
        public RazorHtmlModel(DynamicContentItem item, ViewContext context)
            : base(item)
        {
            if (string.IsNullOrWhiteSpace(Html))
            {
                foreach (
                    var prop in
                        item.Properties.Where(
                            prop => String.Equals(prop.Key, "RazorHtml", StringComparison.InvariantCultureIgnoreCase)))
                {
                    Html = prop.Value;
                    break;
                }
            }

            Html = ViewRenderer.RenderTemplate(Html, this, context.Controller.ControllerContext);
        }
    }

    /// <summary>
    /// Class FlashModel.
    /// </summary>
    public class FlashModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlashModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public FlashModel(DynamicContentItem item)
        {
            foreach (var prop in item.Properties)
            {
                if (String.Equals(prop.Key, "Height", StringComparison.InvariantCultureIgnoreCase))
                {
                    Height = int.Parse(prop.Value);
                }

                if (String.Equals(prop.Key, "Width", StringComparison.InvariantCultureIgnoreCase))
                {
                    Width = int.Parse(prop.Value);
                }

                if (String.Equals(prop.Key, "ClassId", StringComparison.InvariantCultureIgnoreCase))
                {
                    ClassId = prop.Value;
                }

                if (String.Equals(prop.Key, "Src", StringComparison.InvariantCultureIgnoreCase))
                {
                    Src = prop.Value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the class identifier.
        /// </summary>
        /// <value>The class identifier.</value>
        public string ClassId { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Src { get; set; }
    }

    /// <summary>
    /// Class ImageNonClickableModel.
    /// </summary>
    public class ImageNonClickableModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageNonClickableModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ImageNonClickableModel(DynamicContentItem item)
        {
            foreach (var prop in item.Properties)
            {
                if (String.Equals(prop.Key, "ImageFilePath", StringComparison.InvariantCultureIgnoreCase))
                {
                    ImageUrl = prop.Value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }
    }

    /// <summary>
    /// Class ImageClickableModel.
    /// </summary>
    public class ImageClickableModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageClickableModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ImageClickableModel(DynamicContentItem item)
        {
            foreach (var prop in item.Properties)
            {
                if (String.Equals(prop.Key, "ImageUrl", StringComparison.InvariantCultureIgnoreCase))
                {
                    ImageUrl = prop.Value;
                }

                if (String.Equals(prop.Key, "TargetUrl", StringComparison.InvariantCultureIgnoreCase))
                {
                    TargetUrl = prop.Value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Gets or sets the target URL.
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetUrl { get; set; }
    }

    /// <summary>
    /// Class ProductWithImageAndPriceModel.
    /// </summary>
    public class ProductWithImageAndPriceModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductWithImageAndPriceModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ProductWithImageAndPriceModel(DynamicContentItem item)
        {
            foreach (var prop in item.Properties)
            {
                if (String.Equals(prop.Key, "ProductCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProductCode = prop.Value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        /// <value>The product code.</value>
        public string ProductCode { get; set; }
    }

    [DataContract]
    public class CategoryUrlModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryUrlModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public CategoryUrlModel(DynamicContentItem item)
        {
            foreach (var prop in item.Properties)
            {
                if (String.Equals(prop.Key, "CategoryCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    CategoryCode = prop.Value;
                }

                if (String.Equals(prop.Key, "Title", StringComparison.InvariantCultureIgnoreCase))
                {
                    Title = prop.Value;
                }

                if (String.Equals(prop.Key, "SortField", StringComparison.InvariantCultureIgnoreCase))
                {
                    SortField = prop.Value;
                }

                if (String.Equals(prop.Key, "ItemCount", StringComparison.InvariantCultureIgnoreCase))
                {
                    ItemCount = int.Parse(prop.Value);
                }

                if (String.Equals(prop.Key, "NewItems", StringComparison.InvariantCultureIgnoreCase))
                {
                    NewItemsOnly = bool.Parse(prop.Value);
                }
            }
        }

        [DataMember]
        public string CategoryCode { get; set; }
        [DataMember]
        public string SortField { get; set; }
        [DataMember]
        public int ItemCount { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public bool NewItemsOnly { get; set; }

    }
}