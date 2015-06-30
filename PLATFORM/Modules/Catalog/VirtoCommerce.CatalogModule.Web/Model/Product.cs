using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model;

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
						_imgSrc = Images.First().Url;
					}
				}
				return _imgSrc;
			}
		}

		public ICollection<Property> Properties { get; set; }
		public ICollection<Image> Images { get; set; }
		public ICollection<Asset> Assets { get; set; }
		public ICollection<Product> Variations { get; set; }
		public ICollection<CategoryLink> Links { get; set; }
		public ICollection<SeoInfo> SeoInfos { get; set; }
		public ICollection<EditorialReview> Reviews { get; set; }
		public ICollection<ProductAssociation> Associations { get; set; }
    }
}