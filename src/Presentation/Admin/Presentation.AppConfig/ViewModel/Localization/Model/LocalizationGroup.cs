namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Model
{
    public class LocalizationGroup
    {
        public string Name { get; set; }

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

    }
}
