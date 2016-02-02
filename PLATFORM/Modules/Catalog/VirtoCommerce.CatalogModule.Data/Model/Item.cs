using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Platform.Core.Common;


namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class Item : AuditableEntity
    {
		public Item()
		{
			CategoryLinks = new NullCollection<CategoryItemRelation>();
			Images = new NullCollection<Image>();
			Assets = new NullCollection<Asset>();
			AssociationGroups = new NullCollection<AssociationGroup>();
			EditorialReviews = new NullCollection<EditorialReview>();
			ItemPropertyValues = new NullCollection<PropertyValue>();
			Childrens = new NullCollection<Item>();
            Assosiations = new NullCollection<Association>();
        }

		[StringLength(1024)]
		[Required]
		public string Name { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		[Required]
		public bool IsActive { get; set; }

		public bool IsBuyable { get; set; }

		public int AvailabilityRule { get; set; }

		public decimal MinQuantity { get; set; }

		public decimal MaxQuantity { get; set; }

		public bool TrackInventory { get; set; }


		[StringLength(128)]
		public string PackageType { get; set; }

		[StringLength(64)]
		[Required]
		[Index(IsUnique = true)] 
		[CustomValidation(typeof(Item), "ValidateItemCode", ErrorMessage = @"Code can't contain $+;=%{}[]|\/@ ~!^*&()?:'<>, characters")]
		public string Code { get; set; }

		[StringLength(128)]
		public string ManufacturerPartNumber { get; set; }
		[StringLength(64)]
		public string Gtin { get; set; }

		[StringLength(64)]
		public string ProductType { get; set; }

		[StringLength(32)]
		public string WeightUnit { get; set; }
		public decimal? Weight { get; set; }
		[StringLength(32)]
		public string MeasureUnit { get; set; }
		public decimal? Height { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }

		public bool? EnableReview { get; set; }

		public int? MaxNumberOfDownload { get; set; }
		public DateTime? DownloadExpiration { get; set; }
		[StringLength(64)]
		public string DownloadType { get; set; }
		public bool? HasUserAgreement { get; set; }
		[StringLength(64)]
		public string ShippingType { get; set; }
		[StringLength(64)]
		public string TaxType { get; set; }
		[StringLength(128)]
		public string Vendor { get; set; }

        #region Navigation Properties

		public virtual ObservableCollection<CategoryItemRelation> CategoryLinks { get; set; }

		public virtual ObservableCollection<Asset> Assets { get; set; }

		public virtual ObservableCollection<Image> Images { get; set; }

        public virtual ObservableCollection<Association> Assosiations { get; set; }
        public virtual ObservableCollection<AssociationGroup> AssociationGroups { get; set; }

		public virtual ObservableCollection<EditorialReview> EditorialReviews { get; set; }

		public virtual ObservableCollection<PropertyValue> ItemPropertyValues { get; set; }

        [Index("CatalogIdAndParentId", 1)]
        public string CatalogId { get; set; }
        public virtual Catalog Catalog { get; set; }

		public string CategoryId { get; set; }
		public virtual Category Category { get; set; }

        [Index("CatalogIdAndParentId", 2)]
        public string ParentId { get; set; }
		public virtual Item Parent { get; set; }

		public virtual ObservableCollection<Item> Childrens { get; set; }
        #endregion

		public static ValidationResult ValidateItemCode(string value, ValidationContext context)
		{
			if (value == null || string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Code can't be empty");
			}

			const string invalidCodeCharacters = @"$+;=%{}[]|\/@ ~!^*&()?:'<>,";

			if (value.IndexOfAny(invalidCodeCharacters.ToCharArray()) > -1)
			{
				return new ValidationResult((@"Code must be valid"));
			}
			else
			{
				return ValidationResult.Success;
			}
		}
    }
}
