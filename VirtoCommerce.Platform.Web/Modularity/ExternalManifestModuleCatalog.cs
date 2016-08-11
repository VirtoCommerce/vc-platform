using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ExternalManifestModuleCatalog : ModuleCatalog
    {
        private readonly string[] _externalManifestUrls;
        private readonly ILog _logger;
        private IEnumerable<ModuleInfo> _installedModules;
        private static object _lockObject = new object();
 
        public ExternalManifestModuleCatalog(IEnumerable<ModuleInfo> installedModules, string[] externalManifestUrls, ILog logger)
        {
            _installedModules = installedModules;
            _externalManifestUrls = externalManifestUrls;
            _logger = logger;
        }

        #region ModuleCatalog overrides      
     
        protected override void InnerLoad()
        {
            lock (_lockObject)
            {
                //Load remote modules 
                if (!_externalManifestUrls.IsNullOrEmpty())
                {
                    foreach (var externalManifestUrl in _externalManifestUrls)
                    {
                        var externalModuleInfos = LoadExternalModulesManifest(externalManifestUrl);
                        foreach (var externalModuleInfo in externalModuleInfos)
                        {
                            if (!base.Modules.OfType<ManifestModuleInfo>().Contains(externalModuleInfo))
                            {
                                var alreadyInstalledModule = _installedModules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.Equals(externalModuleInfo));
                                if(alreadyInstalledModule != null)
                                {
                                    externalModuleInfo.IsInstalled = alreadyInstalledModule.IsInstalled;
                                    externalModuleInfo.Errors = alreadyInstalledModule.Errors;
                                }
                                externalModuleInfo.InitializationMode = InitializationMode.OnDemand;
                                AddModule(externalModuleInfo);
                            }
                        }
                    }
                    //Add already installed module not presenting in external modules list
                    foreach (var installedModuleNotFoundInExternal in _installedModules.Except(base.Modules))
                    {
                        AddModule(installedModuleNotFoundInExternal);
                    }
                }
            }
        }

        /// <summary>
        /// Return all dependencies with best compatible version
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        protected override IEnumerable<ModuleInfo> GetDependentModulesInner(ModuleInfo moduleInfo)
        {
            var retVal = new List<ModuleInfo>();
            var manifestModule = moduleInfo as ManifestModuleInfo;
            if (moduleInfo == null)
            {
                throw new ModularityException("moduleInfo not ManifestModuleInfo type");
            }
            //get all dependency modules with all versions
            var dependecies = base.GetDependentModulesInner(moduleInfo).OfType<ManifestModuleInfo>();
            foreach (var dependencyVersions in dependecies.GroupBy(x => x.Id))
            {
                var dependency = manifestModule.Dependencies.First(x => x.Id == dependencyVersions.Key);
                var allCompatibleDependencies = dependencyVersions.Where(x => dependency.Version.IsCompatibleWithBySemVer(x.Version)).OrderByDescending(x => x.Version);
                var latestCompatibleDependency = allCompatibleDependencies.FirstOrDefault(x => x.IsInstalled);
                //If dependency not installed need find latest compatible version
                if (latestCompatibleDependency == null)
                {
                    latestCompatibleDependency = allCompatibleDependencies.FirstOrDefault();
                }
                if (latestCompatibleDependency != null)
                {
                    retVal.Add(latestCompatibleDependency);
                }
            }
            return retVal;
        }

        protected override void ValidateUniqueModules()
        {
            var modules = this.Modules.OfType<ManifestModuleInfo>().ToList();

            var duplicateModule = modules.FirstOrDefault(x => modules.Count(y => y.Equals(x)) > 1);

            if (duplicateModule != null)
            {
                throw new DuplicateModuleException(duplicateModule.ToString());
            }
        } 
        #endregion
     
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
                throw (ex);
            }
            return retVal;
        }
    }
}
