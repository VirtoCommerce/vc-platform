using System.Collections.Generic;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Localizations;

namespace VirtoCommerce.Platform.Data.Localizations
{
    public class PlatformTranslationDataProvider : FileSystemTranslationDataProvider
    {
        private readonly string  _path;
        public PlatformTranslationDataProvider(IOptions<TranslationOptions> options)
        {
            _path = options.Value.PlatformTranslationFolderPath;
        }
        protected override IEnumerable<string> DiscoveryFolders
        {
            get
            {
                yield return _path;
            }
        }
    }
}
