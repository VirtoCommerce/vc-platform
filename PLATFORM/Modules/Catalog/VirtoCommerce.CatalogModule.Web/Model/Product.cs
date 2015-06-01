using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Product
    {
        public string Id { get; set; }
		public string ManufacturerPartNumber { get; set; }
		public string Gtin { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

		public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
		public string Outline { get; set; }
		public DateTime? IndexingDate { get; set; } 
        public string TitularItemId { get; set; }
        public bool? IsBuyable { get; set; }
        public bool? IsActive { get; set; }
        public bool? TrackInventory { get; set; }
		public int? MaxQuantity { get; set; }
		public int? MinQuantity { get; set; }

		public string ProductType { get; set; }
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }

		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		public bool? EnableReview { get; set; }

		public int? MaxNumberOfDownload { get; set; }
		public DateTime? DownloadExpiration { get; set; }
		public string DownloadType { get; set; }
		public bool? HasUserAgreement { get; set; }

		public string ShippingType { get; set; }
		public string TaxType { get; set; }

		public string Vendor { get; set; }

		private string _imgSrc;
        public string ImgSrc 
		{ 
			get
			{
				if (_imgSrc == null)
				{
					if (Images != null && Images.Any())
					{
						var img = Images.FirstOrDefault(x => x.Group == "primaryimage");
						if (img == null)
						{
							img = Images.First();
						}
						_imgSrc = img.Url;
					}
				}
				return _imgSrc;
			}
		}

        public List<Property> Properties { get; set; }
        public List<ProductImage> Images { get; set; }
		public List<ProductAsset> Assets { get; set; }
        public List<Product> Variations { get; set; }
        public List<CategoryLink> Links { get; set; }
		public List<SeoInfo> SeoInfos { get; set; }
		public List<EditorialReview> Reviews { get; set; }
		public List<ProductAssociation> Associations { get; set; }
    }
}