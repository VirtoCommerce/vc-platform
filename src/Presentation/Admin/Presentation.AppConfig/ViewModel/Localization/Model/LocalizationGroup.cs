using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Model
{
    public class LocalizationGroup
    {
		public string Name { get; set; }
		public string Category { get; set; }

        public Foundation.AppConfig.Model.Localization TranslateLocalization { get; set; }
        
        public Foundation.AppConfig.Model.Localization OriginalLocalization { get; set; }

		public string OriginalValue
        {
            get { return OriginalLocalization != null ? OriginalLocalization.Value : string.Empty; }
        }

		public string TranslateValue 
        {
            get { return TranslateLocalization != null ? TranslateLocalization.Value : string.Empty; }
        }

		public string LocalizationGroupId
        {
            get
            {
                return OriginalLocalization.Name + OriginalLocalization.LanguageCode + TranslateLocalization.LanguageCode;
            }
        }
		
		public IEnumerable<string> ToExportCollection()
		{
			return new List<string>
				{
					string.IsNullOrEmpty(Name) ? string.Empty : Name,
					string.IsNullOrEmpty(Category) || Category.Equals("Web pages") ? string.Empty : Category,
					string.IsNullOrEmpty(OriginalValue) ? string.Empty : OriginalValue,
					string.IsNullOrEmpty(TranslateLocalization.LanguageCode) ? string.Empty : TranslateLocalization.LanguageCode,
					string.IsNullOrEmpty(TranslateValue) ? string.Empty : TranslateValue
				};
		}

    }
}
