using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.External;

namespace VirtoCommerce.Platform.Modules
{
    public class ExternalModuleCatalog : ModuleCatalog, IExternalModuleCatalog
    {
        private readonly IEnumerable<ModuleInfo> _installedModules;
        private readonly ExternalModuleCatalogOptions _options;
        private readonly IExternalModulesClient _externalClient;
        private readonly ILogger<ExternalModuleCatalog> _logger;

        private static readonly object _lockObject = new object();

        public ExternalModuleCatalog(ILocalModuleCatalog otherCatalog, IExternalModulesClient externalClient, IOptions<ExternalModuleCatalogOptions> options, ILogger<ExternalModuleCatalog> logger)
        {
            _externalClient = externalClient;
            _installedModules = otherCatalog.Modules;
            _options = options.Value;
            _logger = logger;
        }

        #region ModuleCatalog overrides      

        protected override void InnerLoad()
        {
            lock (_lockObject)
            {
                if (_options.ModulesManifestUrl != null)
                {
                    var externalModuleInfos = LoadModulesManifests(_options.ModulesManifestUrl);
                    if(!_options.ExtraModulesManifestUrls.IsNullOrEmpty())
                    {
                        foreach (var extraUrl in _options.ExtraModulesManifestUrls)
                        {
                            var extraManifests = LoadModulesManifests(extraUrl);
                            externalModuleInfos = externalModuleInfos.Concat(extraManifests).Distinct();
                        }
                    }
                    foreach (var externalModuleInfo in externalModuleInfos)
                    {
                        if (!Modules.OfType<ManifestModuleInfo>().Contains(externalModuleInfo))
                        {
                            var alreadyInstalledModule = _installedModules.OfType<ManifestModuleInfo>().FirstOrDefault(x => x.Equals(externalModuleInfo));
                            if (alreadyInstalledModule != null)
                            {
                                externalModuleInfo.IsInstalled = alreadyInstalledModule.IsInstalled;
                                externalModuleInfo.Errors.AddRange(alreadyInstalledModule.Errors);
                            }
                            externalModuleInfo.InitializationMode = InitializationMode.OnDemand;
                            AddModule(externalModuleInfo);
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
            if (!(moduleInfo is ManifestModuleInfo manifestModule))
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

        private IEnumerable<ManifestModuleInfo> LoadModulesManifests(Uri manifestUrl)
        {
            if (manifestUrl == null)
            {
                throw new ArgumentNullException(nameof(manifestUrl));
            }
            
            var result = new List<ManifestModuleInfo>();
            
            _logger.LogDebug("Download module manifests from " + manifestUrl);

            using (var stream = _externalClient.OpenRead(manifestUrl))
            {
                var manifests = stream.DeserializeJson<List<ExternalModuleManifest>>();
                if (!manifests.IsNullOrEmpty())
                {
                    foreach (var manifest in manifests)
                    {
                        if (manifest.Versions != null)
                        {
                            //Select from all versions of module the latest compatible by semVer with the current platform version.
                            var latestStablePlatformCompatibleVersion = manifest.Versions.OrderByDescending(x => x.SemanticVersion)
                                                                           .FirstOrDefault(x => x.PlatformSemanticVersion.IsCompatibleWithBySemVer(PlatformVersion.CurrentVersion)
                                                                                                && string.IsNullOrEmpty(x.VersionTag));
                            if (latestStablePlatformCompatibleVersion != null)
                            {
                                var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                                moduleInfo.LoadFromExternalManifest(manifest, latestStablePlatformCompatibleVersion);
                                result.Add(moduleInfo);
                            }

                            if (_options.IncludePrerelease)
                            {
                                //Select from all versions of module the latest compatible by semVer with the current platform version.
                                var latestPrereleasePlatformCompatibleVersion = manifest.Versions.OrderByDescending(x => x.SemanticVersion)
                                                                               .FirstOrDefault(x => x.PlatformSemanticVersion.IsCompatibleWithBySemVer(PlatformVersion.CurrentVersion)
                                                                                                    && (!string.IsNullOrEmpty(x.VersionTag) && _options.IncludePrerelease));
                                if (latestPrereleasePlatformCompatibleVersion != null)
                                {
                                    var moduleInfo = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                                    moduleInfo.LoadFromExternalManifest(manifest, latestPrereleasePlatformCompatibleVersion);
                                    result.Add(moduleInfo);
                                }
                            }


                        }
                        else
                        {
                            _logger.LogError($"module {manifest.Id} has  invalid  format, missed 'versions'");
                        }
                    }
                }
            }

            return result;
        }


        protected override void ValidateDependencyGraph()
        {
            var modules = Modules.OfType<ManifestModuleInfo>();
            var manifestModules = modules as ManifestModuleInfo[] ?? modules.ToArray();
            try
            {
                base.ValidateDependencyGraph();
            }
            catch (MissedModuleException exception)
            {
                foreach (var module in manifestModules)
                {
                    if (exception.MissedDependenciesMatrix.Keys.Contains(module.ModuleName))
                    {
                        var errorMessage = $"A module declared a dependency on another module which is not declared to be loaded. Missing module(s): {string.Join(", ", exception.MissedDependenciesMatrix[module.ModuleName])}";
                        if (!module.Errors.Any(x => x.Contains(errorMessage)))
                        {
                            module.Errors.Add(errorMessage);
                        }
                    }
                }
            }
        }

    }
}
