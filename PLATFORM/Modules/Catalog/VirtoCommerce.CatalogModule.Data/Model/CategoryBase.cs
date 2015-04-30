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
	public abstract class CategoryBase : AuditableEntity
	{
		public CategoryBase()
		{
			Id = Guid.NewGuid().ToString("N");
			LinkedCategories = new ObservableCollection<LinkedCategory>();
		}

		[Required]
		[StringLength(64)]
		[CustomValidation(typeof(CategoryBase), "ValidateCategoryCode", ErrorMessage = @"Code can't contain $+;=%{}[]|\/@ ~#!^*&()?:'<>, characters")]
		public string Code { get; set; }

	    [Required]
		public bool IsActive { get; set; }

		public int Priority { get; set; }

		#region Navigation Properties

		[StringLength(128)]
		[ForeignKey("Catalog")]
		[Required]
		public string CatalogId { get; set; }

		public virtual CatalogBase Catalog { get; set; }

		[StringLength(128)]
		[ForeignKey("ParentCategory")]
		public string ParentCategoryId { get; set; }

		public virtual CategoryBase ParentCategory { get; set; }

		public virtual ObservableCollection<LinkedCategory> LinkedCategories { get; set; }

		#endregion

		public static ValidationResult ValidateCategoryCode(string value, ValidationContext context)
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
