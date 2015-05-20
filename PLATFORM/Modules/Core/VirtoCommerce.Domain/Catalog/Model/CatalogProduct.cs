using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class CatalogProduct : AuditableEntity, ILinkSupport, ISeoSupport
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

		public string CategoryId { get; set; }
		public Category Category { get; set; }

		public string MainProductId { get; set; }
		public CatalogProduct MainProduct { get; set; }
        public bool? IsBuyable { get; set; }
        public bool? IsActive { get; set; }
        public bool? TrackInventory { get; set; }
		public DateTime? IndexingDate { get; set; }
		public int? MaxQuantity { get; set; }
		public int? MinQuantity { get; set; }

		public ICollection<PropertyValue> PropertyValues { get; set; }
		public ICollection<ItemAsset> Assets { get; set; }
		public ICollection<CategoryLink> Links { get; set; }
		public ICollection<CatalogProduct> Variations { get; set; }
		public ICollection<SeoInfo> SeoInfos { get; set; }
		public ICollection<EditorialReview> Reviews { get; set; }
		public ICollection<ProductAssociation> Associations { get; set; }
	    public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
