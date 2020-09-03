namespace VirtoCommerce.Platform.Core.Localizations
{
    public class TranslationOptions
    {
        //The name of folder where localization resources being discovered
        public string PlatformTranslationFolderPath { get; set; } = "~/Localizations";
        public string ModuleTranslationFolderName { get; set; } = "Localizations";
        public string FallbackLanguage { get; set; } = "en";
    }
}
