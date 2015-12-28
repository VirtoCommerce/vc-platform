using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class Category : AuditableEntity
	{
		public Category()
		{
	    	Images = new NullCollection<Image>();
			CategoryPropertyValues = new NullCollection<PropertyValue>();
			OutgoingLinks = new NullCollection<CategoryRelation>();
			IncommingLinks = new NullCollection<CategoryRelation>();
            Properties = new NullCollection<Property>();
        }

		[Required]
		[StringLength(64)]
		[CustomValidation(typeof(Category), "ValidateCategoryCode", ErrorMessage = @"Code can't contain $+;=%{}[]|\/@ ~!^*&()?:'<>, characters")]
		public string Code { get; set; }

		[Required]
		public bool IsActive { get; set; }

		public int Priority { get; set; }
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		[StringLength(64)]
		public string TaxType { get; set; }

        [NotMapped]
        public Category[] AllParents { get; set; }

		#region Navigation Properties
		[StringLength(128)]
		[ForeignKey("Catalog")]
		[Required]
		public string CatalogId { get; set; }

		public virtual Catalog Catalog { get; set; }

		[StringLength(128)]
		[ForeignKey("ParentCategory")]
		public string ParentCategoryId { get; set; }

		public virtual Category ParentCategory { get; set; }

		public virtual ObservableCollection<Image> Images { get; set; }

		public virtual ObservableCollection<PropertyValue> CategoryPropertyValues { get; set; }
		//It new navigation property for link replace to stupid CategoryLink (will be removed later)
		public virtual ObservableCollection<CategoryRelation> OutgoingLinks { get; set; }
		public virtual ObservableCollection<CategoryRelation> IncommingLinks { get; set; }
        public virtual ObservableCollection<Property> Properties { get; set; }
        #endregion

        public static ValidationResult ValidateCategoryCode(string value, ValidationContext context)
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
