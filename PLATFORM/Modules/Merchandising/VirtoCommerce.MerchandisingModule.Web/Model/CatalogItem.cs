using System;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class CatalogItem
    {
        #region Public Properties

        public Association[] Associations { get; set; }

        [Required]
        public string CatalogId { get; set; }

        public string[] Categories { get; set; }

        [Required]
        public string Code { get; set; }

        public EditorialReview[] EditorialReviews { get; set; }

        [Required]
        public string Id { get; set; }

        public ItemImage[] Images { get; set; }
        public string MainProductId { get; set; }

        public bool? TrackInventory { get; set; }

        public bool? IsBuyable { get; set; }

        public bool? IsActive { get; set; }

		public int? MaxQuantity { get; set; }
		public int? MinQuantity { get; set; }

        public string Name { get; set; }

        public string Outline { get; set; }

        public PropertyDictionary Properties { get; set; }

        public double Rating { get; set; }
        public int ReviewsTotal { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #endregion
    }
}
