using System.ComponentModel;
using Omu.ValueInjecter;
using PropertyChanged;
using VirtoCommerce.Foundation.Frameworks;
using ObjectModel = VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Model
{
	[ImplementPropertyChanged]
    public class SeoUrlKeyword : StorageEntity
    {
		//invalid keyword characters
		const string InvalidKeywordCharacters = @"$+;=%{}[]|\/@ ~#!^*&?:'<>,";

        public SeoUrlKeyword()
		{
			SeoUrlKeywordId = GenerateNewKey();
		}

		public SeoUrlKeyword(string locale, int keywordType, string keywordValue)
		{
			SeoUrlKeywordId = GenerateNewKey();
			Language = locale;
			KeywordType = keywordType;
			KeywordValue = keywordValue;
			IsChanged = false;
		}

		public SeoUrlKeyword(ObjectModel.SeoUrlKeyword originalKeyword)
		{
			this.InjectFrom(originalKeyword);
			IsChanged = false;
		}
		
		[DoNotSetChanged]
		[DoNotNotify]
		public string SeoUrlKeywordId { get; set; }
        public string Language { get; set; }
        public string Keyword { get; set; }
		public string KeywordValue { get; set; }
        public int KeywordType { get; set; }
		public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string ImageAltDescription { get; set; }

		[DoNotNotify]
		public bool IsChanged { get; set; }

		[DoNotNotify]
		[DoNotSetChanged]
	    public string BaseUrl { get; set; }
 
		public override bool Validate()
		{
			if (!IsChanged)
				return true;

			var _isValid = !string.IsNullOrEmpty(Keyword) || (string.IsNullOrEmpty(ImageAltDescription) && string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(MetaDescription));

			if (_isValid)
			{
				if (!string.IsNullOrEmpty(Keyword))
				{
					_isValid = !(Keyword.IndexOfAny(InvalidKeywordCharacters.ToCharArray()) > -1);
					if (!_isValid)
						SetError("Keyword", string.Format(@"Keyword can't contain {0} characters".Localize(), InvalidKeywordCharacters), true);
				}
			}
			else
				SetError("Keyword", @"Keyword is required".Localize(), true);
			
			if (_isValid)
				ClearError("Keyword");

			return _isValid;
		}
    }
}
