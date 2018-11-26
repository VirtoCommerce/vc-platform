using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.Platform.Data.ExportImport
{
    public sealed class PlatformExportEntries
    {
        public PlatformExportEntries()
        {
            Users = new List<ApplicationUserExtended>();
            Settings = new List<SettingEntry>();
            DynamicProperties = new List<DynamicProperty>();
            DynamicPropertyDictionaryItems = new List<DynamicPropertyDictionaryItem>();
            NotificationTemplates = new List<NotificationTemplate>();
            AssetEntries = new List<AssetEntry>();
        }
        public bool IsNotEmpty
        {
            get
            {
                return Users.Any() || Settings.Any();
            }
        }
        public ICollection<ApplicationUserExtended> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<SettingEntry> Settings { get; set; }
        public ICollection<DynamicPropertyDictionaryItem> DynamicPropertyDictionaryItems { get; set; }
        public ICollection<DynamicProperty> DynamicProperties { get; set; }
        public ICollection<NotificationTemplate> NotificationTemplates { get; set; }
        public ICollection<AssetEntry> AssetEntries { get; set; }
    }



    public class PlatformExportImportManager : IPlatformExportImportManager
    {
        private readonly Uri _manifestPartUri;
        private readonly Uri _platformEntriesPartUri;

        private readonly IModuleCatalog _moduleCatalog;
        private readonly ISecurityService _securityService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly ISettingsManager _settingsManager;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly INotificationTemplateService _notificationTemplateService;
        private readonly ICacheManager<object> _cacheManager;
        private readonly IAssetEntryService _assetEntryService;
        private readonly IAssetEntrySearchService _assetEntrySearchService;

        [CLSCompliant(false)]
        public PlatformExportImportManager(ISecurityService securityService, IRoleManagementService roleManagementService, ISettingsManager settingsManager, IDynamicPropertyService dynamicPropertyService, IModuleCatalog moduleCatalog, ICacheManager<object> cacheManager,
                                           INotificationTemplateService notificationTemplateService, IAssetEntryService assetEntryService, IAssetEntrySearchService assetEntrySearchService)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _securityService = securityService;
            _roleManagementService = roleManagementService;
            _settingsManager = settingsManager;
            _moduleCatalog = moduleCatalog;
            _cacheManager = cacheManager;
            _notificationTemplateService = notificationTemplateService;
            _manifestPartUri = PackUriHelper.CreatePartUri(new Uri("Manifest.json", UriKind.Relative));
            _platformEntriesPartUri = PackUriHelper.CreatePartUri(new Uri("PlatformEntries.json", UriKind.Relative));
            _assetEntryService = assetEntryService;
            _assetEntrySearchService = assetEntrySearchService;
        }

        #region IPlatformExportImportManager Members

        public PlatformExportManifest GetNewExportManifest(string author)
        {
            var retVal = new PlatformExportManifest
            {
                Author = author,
                PlatformVersion = PlatformVersion.CurrentVersion.ToString(),
                Modules = InnerGetModulesWithInterface(typeof(ISupportExportImportModule)).Select(x => new ExportModuleInfo
                {
                    Id = x.Id,
                    Version = x.Version.ToString(),
                    Description = ((ISupportExportImportModule)x.ModuleInstance).ExportDescription
                }).ToArray()
            };

            return retVal;
        }

        public PlatformExportManifest ReadExportManifest(Stream stream)
        {
            PlatformExportManifest retVal = null;
            using (var package = Package.Open(stream, FileMode.Open))
            {
                var manifestPart = package.GetPart(_manifestPartUri);
                using (var manifestStream = manifestPart.GetStream())
                {
                    retVal = manifestStream.DeserializeJson<PlatformExportManifest>(GetJsonSerializer());
                }
            }

            return retVal;
        }

        public void Export(Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            using (var package = Package.Open(outStream, FileMode.Create))
            {
                //Export all selected platform entries
                ExportPlatformEntriesInternal(package, manifest, progressCallback);
                //Export all selected  modules
                ExportModulesInternal(package, manifest, progressCallback);

                //Write system information about exported modules
                var manifestPart = package.CreatePart(_manifestPartUri, "application/javascript");

                //After all modules exported need write export manifest part
                using (var stream = manifestPart.GetStream())
                {
                    manifest.SerializeJson(stream, GetJsonSerializer());
                }
            }
        }

        public void Import(Stream stream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            var progressInfo = new ExportImportProgressInfo
            {
                Description = "Starting platform import..."
            };
            progressCallback(progressInfo);

            using (var package = Package.Open(stream, FileMode.Open))
            using (var guard = EventSupressor.SupressEvents())
            {
                //Import selected platform entries
                ImportPlatformEntriesInternal(package, manifest, progressCallback);
                //Import selected modules
                ImportModulesInternal(package, manifest, progressCallback);
                //Reset cache
                _cacheManager.Clear();
            }
        }

        #endregion

        private void ImportPlatformEntriesInternal(Package package, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var progressInfo = new ExportImportProgressInfo();

            var platformEntriesPart = package.GetPart(_platformEntriesPartUri);
            if (platformEntriesPart != null)
            {
                PlatformExportEntries platformEntries;
                using (var stream = platformEntriesPart.GetStream())
                {
                    platformEntries = stream.DeserializeJson<PlatformExportEntries>(GetJsonSerializer());
                }

                //Import security objects
                if (manifest.HandleSecurity)
                {
                    progressInfo.Description = $"Import {platformEntries.Users.Count()} users with roles...";
                    progressCallback(progressInfo);

                    //First need import roles
                    foreach (var role in platformEntries.Roles)
                    {
                        _roleManagementService.AddOrUpdateRole(role);
                    }
                    //Next create or update users
                    foreach (var user in platformEntries.Users)
                    {
                        if (_securityService.FindByIdAsync(user.Id, UserDetails.Reduced).Result != null)
                        {
                            var dummy = _securityService.UpdateAsync(user).Result;
                        }
                        else
                        {
                            var dummy = _securityService.CreateAsync(user).Result;
                        }
                    }
                }

                //Import modules settings
                if (manifest.HandleSettings)
                {
                    //Import dynamic properties
                    _dynamicPropertyService.SaveProperties(platformEntries.DynamicProperties.ToArray());
                    foreach (var propDicGroup in platformEntries.DynamicPropertyDictionaryItems.GroupBy(x => x.PropertyId))
                    {
                        _dynamicPropertyService.SaveDictionaryItems(propDicGroup.Key, propDicGroup.ToArray());
                    }

                    foreach (var module in manifest.Modules)
                    {
                        _settingsManager.SaveSettings(platformEntries.Settings.Where(x => x.ModuleId == module.Id).ToArray());
                    }
                }

                //Import notification templates
                if (!platformEntries.NotificationTemplates.IsNullOrEmpty())
                {
                    _notificationTemplateService.Update(platformEntries.NotificationTemplates.ToArray());
                }

                //Import asset entires 
                if (!platformEntries.AssetEntries.IsNullOrEmpty())
                {
                    var totalAssetsEntriesCount = platformEntries.AssetEntries.Count();
                    const int batchSize = 50;
                    for (var i = 0; i <= totalAssetsEntriesCount; i += batchSize)
                    {
                        progressInfo.Description = $"Asset: {Math.Min(totalAssetsEntriesCount, i + batchSize) } of {totalAssetsEntriesCount} asset entries have been imported...";
                        progressCallback(progressInfo);
                        _assetEntryService.SaveChanges(platformEntries.AssetEntries.Skip(i).Take(batchSize));
                    }
                }
            }
        }

        private void ExportPlatformEntriesInternal(Package package, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var progressInfo = new ExportImportProgressInfo();
            var platformExportObj = new PlatformExportEntries();

            if (manifest.HandleSecurity)
            {
                //Roles
                platformExportObj.Roles = _roleManagementService.SearchRoles(new RoleSearchRequest { SkipCount = 0, TakeCount = int.MaxValue }).Roles;
                //users 
                var usersResult = Task.Run(() => _securityService.SearchUsersAsync(new UserSearchRequest { TakeCount = int.MaxValue })).Result;
                progressInfo.Description = $"Security: {usersResult.Users.Count()} users exporting...";
                progressCallback(progressInfo);

                foreach (var user in usersResult.Users)
                {
                    var userExt = Task.Run(() => _securityService.FindByIdAsync(user.Id, UserDetails.Export)).Result;
                    if (userExt != null)
                    {
                        platformExportObj.Users.Add(userExt);
                    }
                }
            }

            //Export setting for selected modules
            if (manifest.HandleSettings)
            {
                progressInfo.Description = "Settings: selected modules settings exporting...";
                progressCallback(progressInfo);

                platformExportObj.Settings = manifest.Modules.SelectMany(x => _settingsManager.GetModuleSettings(x.Id)).ToList();
            }

            //Dynamic properties
            var allTypes = _dynamicPropertyService.GetAvailableObjectTypeNames();

            progressInfo.Description = "Dynamic properties: load properties...";
            progressCallback(progressInfo);

            platformExportObj.DynamicProperties = allTypes.SelectMany(x => _dynamicPropertyService.GetProperties(x)).ToList();
            platformExportObj.DynamicPropertyDictionaryItems = platformExportObj.DynamicProperties.Where(x => x.IsDictionary).SelectMany(x => _dynamicPropertyService.GetDictionaryItems(x.Id)).ToList();

            //Notification templates
            progressInfo.Description = "Notifications: load templates...";
            progressCallback(progressInfo);
            platformExportObj.NotificationTemplates = _notificationTemplateService.GetAllTemplates().ToList();

            //Asset entries
            progressInfo.Description = "Asset: Evaluate asset entries count...";
            progressCallback(progressInfo);
            const int batchSize = 50;
            var totalAssetsEntriesCount = _assetEntrySearchService.SearchAssetEntries(new AssetEntrySearchCriteria { Skip = 0, Take = 0 }).TotalCount;
            for (var i = 0; i <= totalAssetsEntriesCount; i += batchSize)
            {
                progressInfo.Description = $"Asset: {Math.Min(totalAssetsEntriesCount, i + batchSize) } of {totalAssetsEntriesCount} asset entries have been loaded...";
                progressCallback(progressInfo);
                platformExportObj.AssetEntries.AddRange(_assetEntrySearchService.SearchAssetEntries(new AssetEntrySearchCriteria { Skip = i, Take = batchSize }).Results);
            }

            //Create part for platform entries
            var platformEntiriesPart = package.CreatePart(_platformEntriesPartUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Normal);
            using (var partStream = platformEntiriesPart.GetStream())
            {
                platformExportObj.SerializeJson(partStream);
            }
        }

        private void ImportModulesInternal(Package package, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var progressInfo = new ExportImportProgressInfo();
            foreach (var moduleInfo in manifest.Modules)
            {
                var moduleDescriptor = InnerGetModulesWithInterface(typeof(ISupportExportImportModule)).FirstOrDefault(x => x.Id == moduleInfo.Id);
                if (moduleDescriptor != null)
                {
                    var modulePart = package.GetPart(new Uri(moduleInfo.PartUri, UriKind.Relative));
                    using (var modulePartStream = modulePart.GetStream())
                    {
                        Action<ExportImportProgressInfo> modulePorgressCallback = (x) =>
                        {
                            progressInfo.Description = $"{moduleInfo.Id}: {x.Description}";
                            progressInfo.Errors = x.Errors;
                            progressCallback(progressInfo);
                        };
                        try
                        {
                            ((ISupportExportImportModule)moduleDescriptor.ModuleInstance).DoImport(modulePartStream, manifest, modulePorgressCallback);
                        }
                        catch (Exception ex)
                        {
                            progressInfo.Errors.Add($"{moduleInfo.Id}: {ex.ExpandExceptionMessage()}");
                            progressCallback(progressInfo);
                        }
                    }
                }
            }
        }

        private void ExportModulesInternal(Package package, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var progressInfo = new ExportImportProgressInfo();

            foreach (var module in manifest.Modules)
            {
                var moduleDescriptor = InnerGetModulesWithInterface(typeof(ISupportExportImportModule)).FirstOrDefault(x => x.Id == module.Id);
                if (moduleDescriptor != null)
                {
                    //Create part for module
                    var modulePartUri = PackUriHelper.CreatePartUri(new Uri(module.Id + ".json", UriKind.Relative));
                    var modulePart = package.CreatePart(modulePartUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Normal);

                    Action<ExportImportProgressInfo> modulePorgressCallback = (x) =>
                    {
                        progressInfo.Description = $"{module.Id}: {x.Description}";
                        progressInfo.Errors = x.Errors;
                        progressCallback(progressInfo);
                    };

                    progressInfo.Description = $"{module.Id}: exporting...";
                    progressCallback(progressInfo);

                    try
                    {
                        ((ISupportExportImportModule)moduleDescriptor.ModuleInstance).DoExport(modulePart.GetStream(), manifest, modulePorgressCallback);
                    }
                    catch (Exception ex)
                    {
                        progressInfo.Errors.Add($"{module.Id}: {ex.ExpandExceptionMessage()}");
                        progressCallback(progressInfo);
                    }

                    module.PartUri = modulePartUri.ToString();
                }
            }

        }


        private ManifestModuleInfo[] InnerGetModulesWithInterface(Type interfaceType)
        {
            var retVal = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.ModuleInstance != null)
                                        .Where(x => x.ModuleInstance.GetType().GetInterfaces().Contains(interfaceType))
                                        .ToArray();
            return retVal;
        }

        private static JsonSerializer GetJsonSerializer()
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return serializer;
        }

    }
}
