using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/product
    /// </summary>
    public class Product : Drop
    {
        private readonly Storefront.Model.Catalog.Product _product;
        private readonly Storefront.Model.WorkContext _context;
        private readonly Storefront.Model.Common.IStorefrontUrlBuilder _urlBuilder;

        public Product(Storefront.Model.Catalog.Product product, Storefront.Model.Common.IStorefrontUrlBuilder urlBuilder, Storefront.Model.WorkContext context)
        {
            _product = product;
            _context = context;
            _urlBuilder = urlBuilder;
        }


        public bool Available
        {
            get
            {
                return this.Variants.Any(v => v.Available);
            }
        }



        public decimal CompareAtPrice
        {
            get
            {
                return SelectedOrFirstAvailableVariant.CompareAtPrice;
            }
        }

        public decimal CompareAtPriceMax
        {
            get
            {
                return Variants.Max(v => v.CompareAtPrice);
            }
        }

        public decimal CompareAtPriceMin
        {
            get
            {
                return (decimal)Variants.Min(v => v.CompareAtPrice);
            }
        }

        public bool CompareAtPriceVaries
        {
            get
            {
                return this.CompareAtPriceMax != this.CompareAtPriceMin;
            }
        }

        public string Content
        {
            get
            {
                return this.Description;
            }
        }

        public string Description
        {
            get
            {
                return _product.Description;
            }
        }

        public string Excerpt
        {
            get
            {
                return _product.Name;
            }
        }

        public string TaxType
        {
            get
            {
                return _product.TaxType;
            }
        }

        public Image FeaturedImage
        {
            get
            {
                Image retVal = null;
                if (_product.PrimaryImage != null)
                {
                    retVal = new Image(_product.PrimaryImage);
                }

                return retVal;
            }
        }

        public Variant FirstAvailableVariant
        {
            get
            {
                return this.Variants.FirstOrDefault(v => v.Available);
            }
        }

        public string Handle { get; set; }

        public string Id
        {
            get
            {
                return _product.Id;
            }
        }

        public Image[] Images
        {
            get
            {
                return _product.Images.Select(i => new Image(i)).ToArray();
            }
        }

        public MetaFieldNamespacesCollection Metafields { get; set; }

        public string[] Options
        {
            get
            {
                var options = new List<string>();
                if (_product.Properties != null)
                {
                    foreach (var property in _product.Properties)
                    {
                        if (property.Type == "Variation")
                        {
                            options.Add(property.Name);
                        }
                    }
                }
                return options.ToArray();
            }
        }

        /// <summary>
        /// Returns the price of the product. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal Price
        {
            get
            {
                return SelectedOrFirstAvailableVariant.Price;
            }
        }

        /// <summary>
        /// Returns the highest price of the product. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal PriceMax
        {
            get
            {
                return Variants.Max(v => v.Price);
            }
        }

        /// <summary>
        /// Returns the lowest price of the product. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal PriceMin
        {
            get
            {
                return Variants.Min(v => v.Price);
            }
        }

        public bool PriceVaries
        {
            get
            {
                return this.PriceMin != this.PriceMax;
            }
        }

        public Variant SelectedOrFirstAvailableVariant
        {
            get
            {
                return this.SelectedVariant ?? this.FirstAvailableVariant;
            }
        }

        public Variant SelectedVariant
        {
            get
            {
                return this.Variants.FirstOrDefault();
            }
        }

        public string[] Tags { get; set; }

        public string TemplateSuffix
        {
            get
            {
                //TODO no info
                return null;
            }
        }

        public string Title
        {
            get
            {
                return _product.Name;
            }
        }

        public string Type
        {
            get
            {
                return _product.ProductType;
            }
        }

        public string Url
        {
            get
            {
                var url = "~/product/" + _product.Id;
                if (_product.SeoInfo != null)
                {
                    url = _product.SeoInfo.Slug;
                }
                return _urlBuilder.ToAppAbsolute(_context, url, _context.CurrentStore, _context.CurrentLanguage);
            }
        }

        public ICollection<Variant> Variants
        {
            get
            {
                var variants = new List<Variant>();
                variants.Add(new Variant(_product, _context, _urlBuilder, _product.Id));
                foreach (var v in _product.Variations.Select(v => new Variant(v, _context, _urlBuilder, _product.Id)))
                    variants.Add(v);
                return variants;
            }
        }

        public string Vendor
        {
            get
            {
                return _product.Vendor;
            }
        }

        public bool IsQuotable
        {
            get
            {
                //TODO no info
                return true;
            }
        }
    }
}
