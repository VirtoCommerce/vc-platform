using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CoreModule.Data.Model
{
	public class SeoUrlKeyword : AuditableEntity
	{
		[StringLength(5)]
		public string Language { get; set; }

		[StringLength(255)]
		[Required]
		[CustomValidation(typeof(SeoUrlKeyword), "ValidateKeywordUrl", ErrorMessage = @"Keyword can't contain $+;=%{}[]|\/@ ~#!^*&?:'<>, characters")]
        [Index("KeywordStoreId", Order = 1)]
        public string Keyword { get; set; }

		[StringLength(255)]
		[Required]
        [Index("ObjectIdAndObjectType", 1)]
        public string ObjectId { get; set; }


		[Required]
		public bool IsActive { get; set; }

		[StringLength(64)]
		[Required]
        [Index("ObjectIdAndObjectType", 2)]
        public string ObjectType { get; set; }

        [StringLength(128)]
        [Index("KeywordStoreId", Order = 2)]
        public string StoreId { get; set; }

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
