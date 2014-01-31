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

		public SeoUrlKeyword(ObjectModel.SeoUrlKeyword originalKeyword)
		{
			this.InjectFrom(originalKeyword);
		}
		
		public string SeoUrlKeywordId { get; set; }
        public string Language { get; set; }
        public string Keyword { get; set; }
		public string KeywordValue { get; set; }
        public int KeywordType { get; set; }
		public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string ImageAltDescription { get; set; }

		public bool IsModified { get; set; }
	    public string BaseUrl { get; set; }

	    private bool _isValid;
	   
		public override bool Validate()
		{
			if (!IsModified)
				return true;

			_isValid = !string.IsNullOrEmpty(Keyword) || (string.IsNullOrEmpty(ImageAltDescription) && string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(MetaDescription));

			if (_isValid)
			{
				ClearError("Keyword");
				if (!string.IsNullOrEmpty(Keyword))
				{
					_isValid = !(Keyword.IndexOfAny(InvalidKeywordCharacters.ToCharArray()) > -1);
					if (!_isValid)
						SetError("Keyword", string.Format(@"Keyword can't contain {0} characters", InvalidKeywordCharacters), false);
				}
			}
			else
				SetError("Keyword", @"Keyword is required", false);
			
			return _isValid;
		}
    }
}
