using System;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class BannerModel.
	/// </summary>
    public class BannerModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="BannerModel" /> class.
		/// </summary>
		/// <param name="items">The items.</param>
        public BannerModel(DynamicContentItem[] items)
        {
            Items = items;
        }

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
        public DynamicContentItem[] Items { get; set; }
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
            foreach (var prop in item.PropertyValues)
            {
                if (String.Equals(prop.Name, "RawHtml", StringComparison.InvariantCultureIgnoreCase))
                {
                    Html = prop.LongTextValue;
                }
            }
        }

		/// <summary>
		/// Gets or sets the HTML.
		/// </summary>
		/// <value>The HTML.</value>
        public string Html { get; set; }
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
            foreach (var prop in item.PropertyValues)
            {
                if (String.Equals(prop.Name, "Height", StringComparison.InvariantCultureIgnoreCase))
                {
                    Height = prop.IntegerValue;
                }

                if (String.Equals(prop.Name, "Width", StringComparison.InvariantCultureIgnoreCase))
                {
                    Width = prop.IntegerValue;
                }

                if (String.Equals(prop.Name, "ClassId", StringComparison.InvariantCultureIgnoreCase))
                {
                    ClassId = prop.LongTextValue;
                }

                if (String.Equals(prop.Name, "Src", StringComparison.InvariantCultureIgnoreCase))
                {
                    Src = prop.LongTextValue;
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
            foreach (var prop in item.PropertyValues)
            {
                if (String.Equals(prop.Name, "ImageFilePath", StringComparison.InvariantCultureIgnoreCase))
                {
                    ImageUrl = prop.LongTextValue;
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
            foreach (var prop in item.PropertyValues)
            {
                if (String.Equals(prop.Name, "ImageUrl", StringComparison.InvariantCultureIgnoreCase))
                {
                    ImageUrl = prop.LongTextValue;
                }

                if (String.Equals(prop.Name, "TargetUrl", StringComparison.InvariantCultureIgnoreCase))
                {
                    TargetUrl = prop.LongTextValue;
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
            foreach (var prop in item.PropertyValues)
            {
                if (String.Equals(prop.Name, "ProductCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProductCode = prop.LongTextValue;
                }
            }
        }

		/// <summary>
		/// Gets or sets the product code.
		/// </summary>
		/// <value>The product code.</value>
        public string ProductCode { get; set; }
    }
}