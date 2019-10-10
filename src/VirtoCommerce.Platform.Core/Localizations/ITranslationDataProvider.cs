using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Core.Localizations
{
    public interface ITranslationDataProvider
    {
        /// <summary>
        /// Returns  a translation table as JSON object.
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        JObject GetTranslationDataForLanguage(string lang);

        /// <summary>
        /// Returns list of all installed languages names in ISO two letter notation
        /// </summary>
        /// <returns></returns>
        string[] GetListOfInstalledLanguages();
    }
}
