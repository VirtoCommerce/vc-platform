using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class SeoUrlKeyword : AuditableEntity
    {
		[StringLength(5)]
		[Required]
		public string Language { get; set; }

   
        [StringLength(255)]
        [Required]
		[CustomValidation(typeof(SeoUrlKeyword), "ValidateKeywordUrl", ErrorMessage = @"Keyword can't contain $+;=%{}[]|\/@ ~#!^*&?:'<>, characters")]
		public string Keyword { get; set; }

        [StringLength(255)]
        [Required]
		public string KeywordValue { get; set; }


        [Required]
		public bool IsActive { get; set; }

        [Required]
		public int KeywordType { get; set; }

        [StringLength(255)]
		public string Title { get; set; }

        [StringLength(1024)]
		public string MetaDescription { get; set; }

        [StringLength(255)]
		public string MetaKeywords { get; set; }

        [StringLength(255)]
		public string ImageAltDescription { get; set; }

		public static ValidationResult ValidateKeywordUrl(string value, ValidationContext context)
		{
			if (string.IsNullOrEmpty(value))
			{
				return new ValidationResult("Keyword can't be empty");
			}

			const string invalidKeywordCharacters = @"$+;=%{}[]|\/@ ~#!^*&?:'<>,";

			if (value.IndexOfAny(invalidKeywordCharacters.ToCharArray()) > -1)
			{
				return new ValidationResult((@"Keyword must be valid URL"));
			}
			else
			{
				return ValidationResult.Success;
			}
		}
    }
}
