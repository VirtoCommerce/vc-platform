using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Localizations;

namespace VirtoCommerce.Platform.Data.Localizations
{
    /// <summary>
    /// Intend for load Json localization resources from file system  
    /// </summary>
    public abstract class FileSystemTranslationDataProvider : ITranslationDataProvider
    {
        #region ILocalizationResourceProvider members
        public virtual JObject GetTranslationDataForLanguage(string lang)
        {
            if(string.IsNullOrEmpty(lang))
            {
                throw new ArgumentException($"{nameof(lang)} must be set");
            }
            if (!TranslationLanguage.IsValid(lang))
            {
                return new JObject();
            }

            var files = GetAllTranslationFiles($"{lang}.*.json");
            return ReadTranslationJson(files);
        }

        public virtual string[] GetListOfInstalledLanguages()
        {
            var files = GetAllTranslationFiles("*.json");
            return files.Select(Path.GetFileName).Select(x => x.Substring(0, x.IndexOf('.'))).Distinct().ToArray();
        }
        #endregion

        protected abstract IEnumerable<string> DiscoveryFolders { get; }

        protected virtual IEnumerable<string> GetAllTranslationFiles(string pattern = null)
        {
            foreach (var discoveryFolder in DiscoveryFolders ?? Array.Empty<string>())
            {
                //Return resource files for platform project
                if (!Directory.Exists(discoveryFolder))
                {
                    continue;
                }

                // A pattern can carry a directory part ("../x.json") that moves the search out of the folder
                var folder = Path.GetFullPath(discoveryFolder);
                if (!Path.EndsInDirectorySeparator(folder))
                {
                    folder += Path.DirectorySeparatorChar;
                }

                foreach (var file in Directory.EnumerateFiles(discoveryFolder, pattern, SearchOption.AllDirectories)
                    .Where(x => Path.GetFullPath(x).StartsWith(folder, StringComparison.OrdinalIgnoreCase)))
                {
                    yield return file;
                }
            }
        }

        protected virtual JObject ReadTranslationJson(IEnumerable<string> files)
        {
            var result = new JObject();
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var part = JObject.Parse(File.ReadAllText(file));
                    result.Merge(part, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                }
            }
            return result;
        }
    }
}
