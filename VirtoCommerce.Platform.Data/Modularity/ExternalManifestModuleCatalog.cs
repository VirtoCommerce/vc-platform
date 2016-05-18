using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Data.Modularity
{
    public class ExternalManifestModuleCatalog : ModuleCatalog
    {
        private readonly string[] _externalManifestUrls;
        private readonly ILog _logger;
        private List<ModuleInfo> _externalModules = new List<ModuleInfo>();

        public ExternalManifestModuleCatalog(IEnumerable<ModuleInfo> installedModules, string[] externalManifestUrls, ILog logger)
        {
            foreach(var installedModule in installedModules)
            {
                base.AddModule(installedModule);
            }
            _externalManifestUrls = externalManifestUrls;
            _logger = logger;
        }       

        protected override void InnerLoad()
        {
            //Load remote modules 
            if (!_externalManifestUrls.IsNullOrEmpty())
            {
                var comparer = AnonymousComparer.Create((ManifestModuleInfo x) => x.Id.ToLowerInvariant() + ":" + x.Version.ToString());
                foreach (var externalManifestUrl in _externalManifestUrls)
                {
                    var externalModuleInfos = LoadExternalModulesManifest(externalManifestUrl);
                    foreach (var externalModuleInfo in externalModuleInfos)
                    {
                        if (!Modules.OfType<ManifestModuleInfo>().Contains(externalModuleInfo, comparer))
                        {
                            externalModuleInfo.IsInstalled = false;
                            externalModuleInfo.InitializationMode = InitializationMode.OnDemand;
                            AddModule(externalModuleInfo);
                        }
                    }
                }
            }
        }

        private IEnumerable<ManifestModuleInfo> LoadExternalModulesManifest(string externalManifestUrl)
        {
            var retVal = new List<ManifestModuleInfo>();
            try
            {
                _logger.Debug("Download module manifests from " + externalManifestUrl);
                using (WebClient webClient = new WebClient())
                using (var stream = webClient.OpenRead(externalManifestUrl))
                {
                    var manifests = stream.DeserializeJson<List<ModuleManifest>>();
                    if (!manifests.IsNullOrEmpty())
                    {
                        foreach (var manifest in manifests)
                        {
                            retVal.Add(new ManifestModuleInfo(manifest));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return retVal;
        }
    }
}
