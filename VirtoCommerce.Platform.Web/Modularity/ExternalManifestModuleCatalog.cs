using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Common.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ExternalManifestModuleCatalog : ModuleCatalog
    {
        private readonly IEnumerable<ModuleInfo> _installedModules;
        private readonly string[] _externalManifestUrls;
        private readonly ILog _logger;
        private static readonly object _lockObject = new object();

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
                // Load remote modules 
                if (!_externalManifestUrls.IsNullOrEmpty())
                {
                    foreach (var externalManifestUrl in _externalManifestUrls)
                    {
                        var externalModuleInfos = LoadExternalModulesManifest(externalManifestUrl);
                        foreach (var externalModuleInfo in externalModuleInfos)
                        {
                            if (!Modules.OfType<ManifestModuleInfo>().Contains(externalModuleInfo))
                            {
                                var alreadyInstalledModule = _installedModules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.Equals(externalModuleInfo));
                                if (alreadyInstalledModule != null)
                                {
                                    externalModuleInfo.IsInstalled = alreadyInstalledModule.IsInstalled;
                                    externalModuleInfo.Errors = alreadyInstalledModule.Errors;
                                }
                                externalModuleInfo.InitializationMode = InitializationMode.OnDemand;
                                AddModule(externalModuleInfo);
                            }
                        }
                    }
                }

                // Add already installed module not presenting in external modules list
                foreach (var installedModuleNotFoundInExternal in _installedModules.Except(Modules))
                {
                    AddModule(installedModuleNotFoundInExternal);
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
            var manifestModule = moduleInfo as ManifestModuleInfo;
            if (manifestModule == null)
            {
                throw new ModularityException("moduleInfo is not ManifestModuleInfo type");
            }

            var result = new List<ModuleInfo>();

            // Get all dependency modules with all versions
            var dependecies = base.GetDependentModulesInner(moduleInfo).OfType<ManifestModuleInfo>();

            foreach (var dependencyGroup in dependecies.GroupBy(x => x.Id))
            {
                var dependency = manifestModule.Dependencies.First(x => x.Id == dependencyGroup.Key);
                var allCompatibleDependencies = dependencyGroup.Where(x => dependency.Version.IsCompatibleWithBySemVer(x.Version)).OrderByDescending(x => x.Version);
                var latestCompatibleDependency = allCompatibleDependencies.FirstOrDefault(x => x.IsInstalled);

                // If dependency is not installed, find latest compatible version
                if (latestCompatibleDependency == null)
                {
                    latestCompatibleDependency = allCompatibleDependencies.FirstOrDefault();
                }

                if (latestCompatibleDependency != null)
                {
                    result.Add(latestCompatibleDependency);
                }
            }

            return result;
        }

        protected override void ValidateUniqueModules()
        {
            var modules = Modules.OfType<ManifestModuleInfo>().ToList();

            var duplicateModule = modules.FirstOrDefault(x => modules.Count(y => y.Equals(x)) > 1);

            if (duplicateModule != null)
            {
                throw new DuplicateModuleException(duplicateModule.ToString());
            }
        }

        #endregion


        private IEnumerable<ManifestModuleInfo> LoadExternalModulesManifest(string externalManifestUrl)
        {
            var result = new List<ManifestModuleInfo>();

            try
            {
                _logger.Debug("Download module manifests from " + externalManifestUrl);
                using (var webClient = new WebClient())
                {
                    webClient.AddAuthorizationTokenForGitHub(externalManifestUrl);

                    using (var stream = webClient.OpenRead(externalManifestUrl))
                    {
                        var manifests = stream.DeserializeJson<List<ModuleManifest>>();
                        if (!manifests.IsNullOrEmpty())
                        {
                            result.AddRange(manifests.Select(manifest => new ManifestModuleInfo(manifest)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                throw;
            }

            return result;
        }
    }
}
