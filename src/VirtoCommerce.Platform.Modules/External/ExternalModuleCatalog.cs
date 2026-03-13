using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Modules.External;

#pragma warning disable VC0014 // Type is obsolete
namespace VirtoCommerce.Platform.Modules
{
    [Obsolete("Use ModuleDiscovery class instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class ExternalModuleCatalog : ModuleCatalog, IExternalModuleCatalog
    {
        private readonly IEnumerable<ModuleInfo> _installedModules;
        private readonly ExternalModuleCatalogOptions _options;
        private readonly IExternalModulesClient _externalClient;
        private readonly ILogger<ExternalModuleCatalog> _logger;

        private static readonly object _lockObject = new object();

        public ExternalModuleCatalog(
            ILocalModuleCatalog otherCatalog,
            IExternalModulesClient externalClient,
            IOptions<ExternalModuleCatalogOptions> options,
            ILogger<ExternalModuleCatalog> logger,
            IOptions<ModuleSequenceBoostOptions> boostOptions)
            : base(boostOptions)
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
                if (_options.ModulesManifestUrl == null)
                {
                    return;
                }

                // Collect external modules from all manifest URLs
                var externalModuleInfos = LoadModulesManifests(_options.ModulesManifestUrl).ToList();
                if (!_options.ExtraModulesManifestUrls.IsNullOrEmpty())
                {
                    foreach (var extraUrl in _options.ExtraModulesManifestUrls)
                    {
                        externalModuleInfos.AddRange(LoadModulesManifests(extraUrl));
                    }
                    externalModuleInfos = externalModuleInfos.Distinct().ToList();
                }

                // Merge with installed using ModuleDiscovery
                var installedList = _installedModules.OfType<ManifestModuleInfo>().ToList();
                var merged = ModuleDiscovery.MergeWithInstalled(externalModuleInfos, installedList);

                foreach (var module in merged)
                {
                    if (!Modules.OfType<ManifestModuleInfo>().Contains(module))
                    {
                        module.InitializationMode = InitializationMode.OnDemand;
                        AddModule(module);
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
            ArgumentNullException.ThrowIfNull(manifestUrl);

            _logger.LogDebug("Download module manifests from " + manifestUrl);

            using var stream = _externalClient.OpenRead(manifestUrl);
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            return ModuleDiscovery.ParseExternalManifest(json, PlatformVersion.CurrentVersion, _options.IncludePrerelease);
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
