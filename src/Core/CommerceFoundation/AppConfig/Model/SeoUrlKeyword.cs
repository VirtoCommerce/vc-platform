using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
    [DataContract]
    [EntitySet("SeoUrlKeywords")]
    [DataServiceKey("SeoUrlKeywordId")]
    public class SeoUrlKeyword : StorageEntity
    {
        public SeoUrlKeyword()
		{
            _seoUrlKeywordId = GenerateNewKey();
		}

        private string _seoUrlKeywordId;
        /// <summary>
        /// Gets or sets the seo URL keyword identifier.
        /// </summary>
        /// <value>
        /// The seo URL keyword identifier.
        /// </value>
        [Key]
        [StringLength(64)]
        [DataMember]
        public string SeoUrlKeywordId
        {
            get { return _seoUrlKeywordId; }
            set { SetValue(ref _seoUrlKeywordId, () => SeoUrlKeywordId, value); }
        }

        private string _language;
        /// <summary>
        /// Gets or sets the language. en-us, en, etc.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [StringLength(5)]
        [DataMember]
        [Required]
        public string Language
        {
            get { return _language; }
            set { SetValue(ref _language, () => Language, value); }
        }

        private string _keyword;
        /// <summary>
        /// Gets or sets the unique keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        [StringLength(255)]
        [DataMember]
        [Required]
		[CustomValidation(typeof(SeoUrlKeyword), "ValidateKeywordUrl", ErrorMessage = @"Keyword can't contain $+;=%{}[]|\/@ ~#!^*&?:'<>, characters")]
        public string Keyword
        {
            get { return _keyword; }
            set { SetValue(ref _keyword, () => Keyword, value); }
        }

        private string _keywordValue;
        /// <summary>
        /// Gets or sets the keyword value.
        /// </summary>
        /// <value>
        /// The keyword value.
        /// </value>
        [StringLength(255)]
        [DataMember]
        [Required]
        public string KeywordValue
        {
            get { return _keywordValue; }
            set { SetValue(ref _keywordValue, () => KeywordValue, value); }
        }

        private bool _isActive;
        /// <summary>
        /// Gets or sets a value indicating whether seo keyword [is active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is active]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        [Required]
        public bool IsActive
        {
            get { return _isActive; }
            set { SetValue(ref _isActive, () => IsActive, value); }
        }

        private int _keywordType;
        /// <summary>
        /// Gets or sets the type of the keyword. 0 - category, 1 - item, 2 - store
        /// </summary>
        /// <value>
        /// The type of the keyword.
        /// </value>
        [DataMember]
        [Required]
        public int KeywordType
        {
            get { return _keywordType; }
            set { SetValue(ref _keywordType, () => KeywordType, value); }
        }

        private string _title;
        /// <summary>
        /// Gets or sets the language specific page title..
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [StringLength(255)]
        [DataMember]
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, () => Title, value); }
        }

        private string _metaDescription;
        /// <summary>
        /// Gets or sets the language specific meta description for the page.
        /// </summary>
        /// <value>
        /// The meta description.
        /// </value>
        [StringLength(1024)]
        [DataMember]
        public string MetaDescription
        {
            get { return _metaDescription; }
            set { SetValue(ref _metaDescription, () => MetaDescription, value); }
        }

        private string _metaKeywords;
        /// <summary>
        /// Gets or sets the language specific meta keywords for the page.
        /// </summary>
        /// <value>
        /// The meta keywords.
        /// </value>
        [StringLength(255)]
        [DataMember]
        public string MetaKeywords
        {
            get { return _metaKeywords; }
            set { SetValue(ref _metaKeywords, () => MetaKeywords, value); }
        }

        private string _imageAltDescription;
        /// <summary>
        /// Gets or sets the language specific image alternate description for the page.
        /// </summary>
        /// <value>
        /// The image alt description.
        /// </value>
        [StringLength(255)]
        [DataMember]
        public string ImageAltDescription
        {
            get { return _imageAltDescription; }
            set { SetValue(ref _imageAltDescription, () => ImageAltDescription, value); }
        }

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
