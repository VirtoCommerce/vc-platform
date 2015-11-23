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


        //public Collections Collections { get; set; }

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
                if (_product.Images != null && _product.Images.Any())
                    return new Image(_product.Images.First(), _product);
                return null;
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

        public ItemCollection<Image> Images
        {
            get
            {
                return new ItemCollection<Image>(_product.Images.Select(i => new Image(i, _product)));
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

        public decimal Price
        {
            get
            {
                return SelectedOrFirstAvailableVariant.Price;
            }
        }

        public decimal PriceMax
        {
            get
            {
                return Variants.Max(v => v.Price);
            }
        }

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
                var queryString = System.Web.HttpContext.Current.Request.QueryString;

                return queryString["variant"] != null ?
                    this.Variants.FirstOrDefault(v => v.Id == queryString["variant"]) :
                    this.Variants.FirstOrDefault(v => v.Id == Handle);
            }
        }

        public ItemCollection<string> Tags { get; set; }

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
                return _urlBuilder.ToAbsolute(_context, string.Format("~/products/{0}", _product.Id), _context.CurrentStore, _context.CurrentLanguage);
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
