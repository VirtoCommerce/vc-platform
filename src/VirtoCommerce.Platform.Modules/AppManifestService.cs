using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Default implementation of <see cref="IAppManifestService"/>.
    /// Walks the topologically sorted module list (already produced by
    /// <see cref="ModuleBootstrapper"/> via <see cref="IModuleService.GetInstalledModules"/>)
    /// and probes each module for plugin descriptors targeting the requested app.
    /// </summary>
    public class AppManifestService : IAppManifestService
    {
        /// <summary>Reserved app id used by the legacy AngularJS admin shell.</summary>
        public const string PlatformAppId = "platform";

        private const string DefaultPluginsDiscoveryFolder = "plugins";
        private const string DefaultRemoteEntry = "remoteEntry.js";
        private const string DefaultExposedModule = "./Module";
        private const string PluginManifestFileName = "plugin.json";

        // Legacy AngularJS bundle convention (existing — see ModulesBundleTagHelperBase).
        private const string LegacyBundleFolder = "dist";
        private const string LegacyScriptFileName = "app.js";
        private const string LegacyStyleFileName = "style.css";

        private static readonly JsonSerializerOptions s_pluginJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        private readonly IModuleService _moduleService;
        private readonly ILogger<AppManifestService> _logger;

        public AppManifestService(IModuleService moduleService, ILogger<AppManifestService> logger)
        {
            _moduleService = moduleService ?? throw new ArgumentNullException(nameof(moduleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public AppManifestDescriptor GetManifest(string appId, ClaimsPrincipal user = null)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException("App id is required.", nameof(appId));
            }

            var modules = _moduleService.GetInstalledModules();

            // Resolve the host app (any module may declare it; first match wins).
            // Special case: the legacy "platform" appId is implicit — every install
            // has one platform admin shell, so it does not require a <app> element
            // somewhere in the module catalogue.
            ManifestModuleInfo hostModule = null;
            ManifestAppInfo hostApp = null;
            if (!IsPlatformApp(appId))
            {
                hostModule = modules.FirstOrDefault(m => m.Apps.Any(a => a.Id == appId));
                hostApp = hostModule?.Apps.FirstOrDefault(a => a.Id == appId);
                if (hostApp == null)
                {
                    return null;
                }
            }

            var descriptor = new AppManifestDescriptor
            {
                AppId = appId,
                // "platform" advertises the running platform version. Other apps
                // surface the version of the module that declared this app id
                // in its module manifest.
                Version = IsPlatformApp(appId)
                    ? PlatformVersion.CurrentVersion?.ToString()
                    : hostModule?.Version?.ToString(),
                Title = hostApp?.Title ?? "Commerce Manager",
                AppPermission = hostApp?.Permission,
            };

            foreach (var module in modules)
            {
                var plugin = IsPlatformApp(appId)
                    ? TryBuildLegacyPlugin(module)
                    : TryBuildModuleFederationPlugin(module, hostApp);

                if (plugin == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(plugin.Permission) &&
                    user?.HasGlobalPermission(plugin.Permission) != true)
                {
                    continue;
                }

                descriptor.Plugins.Add(plugin);
            }

            return descriptor;
        }

        /// <summary>
        /// Probes the legacy AngularJS convention: <c>{moduleRoot}/dist/app.js</c>
        /// optionally accompanied by <c>{moduleRoot}/dist/style.css</c>.
        /// </summary>
        private static PluginDescriptor TryBuildLegacyPlugin(ManifestModuleInfo module)
        {
            if (string.IsNullOrEmpty(module.FullPhysicalPath))
            {
                return null;
            }

            var scriptPath = Path.Combine(module.FullPhysicalPath, LegacyBundleFolder, LegacyScriptFileName);
            if (!File.Exists(scriptPath))
            {
                return null;
            }

            var plugin = new PluginDescriptor
            {
                Id = module.Id,
                Version = module.Version?.ToString(),
                Entry = MakeFile(
                    type: ContentFileTypes.Script,
                    url: BuildModuleUrl(module.ModuleName, $"{LegacyBundleFolder}/{LegacyScriptFileName}"),
                    physicalPath: scriptPath),
            };

            var stylePath = Path.Combine(module.FullPhysicalPath, LegacyBundleFolder, LegacyStyleFileName);
            if (File.Exists(stylePath))
            {
                plugin.ContentFiles.Add(MakeFile(
                    type: ContentFileTypes.Style,
                    url: BuildModuleUrl(module.ModuleName, $"{LegacyBundleFolder}/{LegacyStyleFileName}"),
                    physicalPath: stylePath));
            }

            return plugin;
        }

        /// <summary>
        /// Probes <c>{moduleRoot}/{discoveryFolder}/{appId}/</c> for an MF remote.
        /// If <c>plugin.json</c> exists in that folder, its values override the defaults.
        /// </summary>
        private PluginDescriptor TryBuildModuleFederationPlugin(ManifestModuleInfo module, ManifestAppInfo hostApp)
        {
            if (string.IsNullOrEmpty(module.FullPhysicalPath))
            {
                return null;
            }

            var discoveryFolder = string.IsNullOrWhiteSpace(hostApp.PluginsDiscoveryFolder)
                ? DefaultPluginsDiscoveryFolder
                : hostApp.PluginsDiscoveryFolder.Trim('/', '\\');

            var pluginFolder = Path.Combine(module.FullPhysicalPath, discoveryFolder, hostApp.Id);
            if (!Directory.Exists(pluginFolder))
            {
                return null;
            }

            var manifest = TryReadPluginManifest(pluginFolder);
            // If neither a plugin.json nor a default remoteEntry.js is present, treat the folder as empty.
            var entryFileName = string.IsNullOrWhiteSpace(manifest?.Entry) ? DefaultRemoteEntry : manifest.Entry;
            var entryPhysicalPath = Path.Combine(pluginFolder, entryFileName);
            if (!File.Exists(entryPhysicalPath))
            {
                if (manifest != null)
                {
                    _logger.LogWarning(
                        "Plugin manifest at {ManifestPath} declares entry '{Entry}' but the file is missing; skipping plugin.",
                        Path.Combine(pluginFolder, PluginManifestFileName), entryFileName);
                }
                return null;
            }

            var urlPrefix = $"{discoveryFolder}/{hostApp.Id}";
            // Resolve the plugin id once — used both as PluginDescriptor.Id
            // and as the default federation remote name.
            var pluginId = string.IsNullOrWhiteSpace(manifest?.Id) ? module.Id : manifest.Id;
            var pluginVersion = string.IsNullOrWhiteSpace(manifest?.Version) ? module.Version?.ToString() : manifest.Version;
            var remoteName = string.IsNullOrWhiteSpace(manifest?.Remote?.Name) ? pluginId : manifest.Remote.Name;
            var remoteExposed = string.IsNullOrWhiteSpace(manifest?.Remote?.Exposed) ? DefaultExposedModule : manifest.Remote.Exposed;

            var plugin = new PluginDescriptor
            {
                Id = pluginId,
                Version = pluginVersion,
                Entry = MakeFile(
                    type: ContentFileTypes.Script,
                    url: BuildModuleUrl(module.ModuleName, $"{urlPrefix}/{entryFileName}"),
                    physicalPath: entryPhysicalPath),
                Permission = manifest?.Permission,
                Remote = new PluginRemoteDescriptor
                {
                    Name = remoteName,
                    Exposed = remoteExposed,
                },
            };

            if (manifest?.ContentFiles != null)
            {
                foreach (var file in manifest.ContentFiles.Where(f => !string.IsNullOrWhiteSpace(f)))
                {
                    var relative = file.TrimStart('/', '\\');
                    var physical = Path.Combine(pluginFolder, relative);
                    plugin.ContentFiles.Add(MakeFile(
                        type: InferContentFileType(relative),
                        url: BuildModuleUrl(module.ModuleName, $"{urlPrefix}/{relative}"),
                        physicalPath: physical));
                }
            }

            return plugin;
        }

        private PluginManifestFile TryReadPluginManifest(string pluginFolder)
        {
            var manifestPath = Path.Combine(pluginFolder, PluginManifestFileName);
            if (!File.Exists(manifestPath))
            {
                return null;
            }

            try
            {
                using var stream = File.OpenRead(manifestPath);
                return JsonSerializer.Deserialize<PluginManifestFile>(stream, s_pluginJsonOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to parse {ManifestPath}; falling back to convention defaults.", manifestPath);
                return null;
            }
            catch (IOException ex)
            {
                _logger.LogWarning(ex, "Failed to read {ManifestPath}; falling back to convention defaults.", manifestPath);
                return null;
            }
        }

        /// <summary>
        /// Builds a URL matching the static-file route registered by
        /// <c>UseModulesAndAppsFiles</c>: <c>/modules/$({moduleName})/{relativePath}</c>.
        /// The literal <c>$(...)</c> in the URL is a long-standing platform convention
        /// (mirrors <see cref="ModuleBootstrapper"/>'s static-file <c>RequestPath</c>).
        /// </summary>
        private static string BuildModuleUrl(string moduleName, string relativePath)
        {
            var normalized = relativePath.Replace('\\', '/').TrimStart('/');
            return $"/modules/$({moduleName})/{normalized}";
        }

        /// <summary>
        /// Builds a content-file descriptor with cache-busting hash computed from
        /// the file's last-write time. Fast (single stat call), stable across
        /// requests, and changes whenever the file is rebuilt — sufficient for
        /// browser cache invalidation. Returns <c>null</c> hash if the file is
        /// missing.
        /// </summary>
        private static ContentFileDescriptor MakeFile(string type, string url, string physicalPath)
        {
            return new ContentFileDescriptor
            {
                Type = type,
                Path = url,
                Hash = ComputeFileHash(physicalPath),
            };
        }

        private static string ComputeFileHash(string physicalPath)
        {
            try
            {
                if (!File.Exists(physicalPath))
                {
                    return null;
                }
                // X8 = 8 hex chars, plenty for cache-busting; ticks are unique per file mutation.
                return File.GetLastWriteTimeUtc(physicalPath).Ticks.ToString("X8", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (IOException)
            {
                return null;
            }
        }

        private static string InferContentFileType(string relativePath)
        {
            var ext = Path.GetExtension(relativePath);
            return string.Equals(ext, ".css", StringComparison.OrdinalIgnoreCase)
                ? ContentFileTypes.Style
                : ContentFileTypes.Script;
        }

        private static bool IsPlatformApp(string appId) =>
            string.Equals(appId, PlatformAppId, StringComparison.OrdinalIgnoreCase);

        // ---- plugin.json schema (server-side parsing model) ----
        //
        // These records are populated exclusively by System.Text.Json
        // deserialization. Using `init` setters + `sealed record` makes the
        // immutability intent explicit; the analyzer-flagged "unused setter"
        // / "unassigned property" warnings are inherent to JSON DTOs and are
        // mitigated here by the `record` shape (Sonar treats records as data
        // carriers and skips those rules).

        private sealed record PluginManifestFile
        {
            public string Id { get; init; }
            public string Version { get; init; }
            public string Entry { get; init; }
            public List<string> ContentFiles { get; init; }
            public PluginManifestRemote Remote { get; init; }
            public string Permission { get; init; }
        }

        private sealed record PluginManifestRemote
        {
            public string Name { get; init; }
            public string Exposed { get; init; }
        }
    }
}
