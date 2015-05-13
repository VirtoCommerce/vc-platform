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
    public abstract class Item : AuditableEntity
    {
		public Item()
		{
			Id = Guid.NewGuid().ToString("N");
			CategoryItemRelations = new NullCollection<CategoryItemRelation>();
			ItemAssets = new NullCollection<ItemAsset>();
			AssociationGroups = new NullCollection<AssociationGroup>();
			EditorialReviews = new NullCollection<EditorialReview>();
			ItemPropertyValues = new NullCollection<ItemPropertyValue>();
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

		public decimal Weight { get; set; }

		[StringLength(128)]
		public string PackageType { get; set; }

		[StringLength(128)]
		public string TaxCategory { get; set; }

		[StringLength(64)]
		[Required]
		[Index(IsUnique = true)] 
		[CustomValidation(typeof(Item), "ValidateItemCode", ErrorMessage = @"Code can't contain $+;=%{}[]|\/@ ~#!^*&()?:'<>, characters")]
		public string Code { get; set; }

        #region Navigation Properties

		public virtual ObservableCollection<CategoryItemRelation> CategoryItemRelations { get; set; }

		public virtual ObservableCollection<ItemAsset> ItemAssets { get; set; }

		public virtual ObservableCollection<AssociationGroup> AssociationGroups { get; set; }

		public virtual ObservableCollection<EditorialReview> EditorialReviews { get; set; }

		public virtual ObservableCollection<ItemPropertyValue> ItemPropertyValues { get; set; }

		[StringLength(128)]
		public string PropertySetId { get; set; }

        public virtual PropertySet PropertySet { get; set; }

		[StringLength(128)]
		public string CatalogId { get; set; }

        public virtual CatalogBase Catalog { get; set; }

        #endregion

		public static ValidationResult ValidateItemCode(string value, ValidationContext context)
		{
			if (value == null || string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Code can't be empty");
			}

			const string invalidCodeCharacters = @"$+;=%{}[]|\/@ ~#!^*&()?:'<>,";

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
