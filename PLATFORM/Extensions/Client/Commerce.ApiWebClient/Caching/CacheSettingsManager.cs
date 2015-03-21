using System.Configuration;
using System.Diagnostics;
using System.Security;
using System.Web;
using System.Web.Configuration;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;

namespace VirtoCommerce.ApiWebClient.Caching
{
    public class CacheSettingsManager : ICacheSettingsManager
    {
        private const string AspnetInternalProviderName = "AspNetInternalProvider";
        private readonly OutputCacheSection _outputCacheSection;

        public CacheSettingsManager()
        {
            try
            {
                _outputCacheSection = (OutputCacheSection)ConfigurationManager.GetSection("system.web/caching/outputCache");
            }
            catch (SecurityException)
            {
                Trace.WriteLine("MvcDonutCaching does not have permission to read web.config section 'OutputCacheSection'. Using default provider.");
                _outputCacheSection = new OutputCacheSection
                {
                    DefaultProviderName = AspnetInternalProviderName,
                    EnableOutputCache = true
                };
            }
        }

        public ProviderSettings RetrieveOutputCacheProviderSettings()
        {
            return _outputCacheSection.DefaultProviderName == AspnetInternalProviderName 
                ? null 
                : _outputCacheSection.Providers[_outputCacheSection.DefaultProviderName];
        }

        public OutputCacheProfile RetrieveOutputCacheProfile(string cacheProfileName)
        {
            OutputCacheSettingsSection outputCacheSettingsSection;

            try
            {
                outputCacheSettingsSection = (OutputCacheSettingsSection)ConfigurationManager.GetSection("system.web/caching/outputCacheSettings");
            }
            catch (SecurityException)
            {
                throw new SecurityException("MvcDonutCaching does not have permission to read web.config section 'OutputCacheSettingsSection'.");
            }

            if (outputCacheSettingsSection != null && outputCacheSettingsSection.OutputCacheProfiles.Count > 0)
            {
                var cacheProfile = outputCacheSettingsSection.OutputCacheProfiles[cacheProfileName];

                if (cacheProfile != null)
                {
                    return cacheProfile;
                }
            }

            throw new HttpException(string.Format("The '{0}' cache profile is not defined.  Please define it in the configuration file.", cacheProfileName));
        }

        public bool IsCachingEnabledGlobally
        {
            get { return _outputCacheSection.EnableOutputCache; }
        }
    }
}
