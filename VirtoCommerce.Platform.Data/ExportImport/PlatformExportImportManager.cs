using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Packaging;
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
            DynamicPropertyDictionaryItems = new List<DynamicPropertyDictionaryItem>();
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
    }



    public class PlatformExportImportManager : IPlatformExportImportManager
    {
        private readonly Uri _manifestPartUri;
        private readonly Uri _platformEntriesPartUri;

        private readonly IPackageService _packageService;
        private readonly ISecurityService _securityService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly ISettingsManager _settingsManager;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly ICacheManager<object> _cacheManager;
        [CLSCompliant(false)]
        public PlatformExportImportManager(ISecurityService securityService, IRoleManagementService roleManagementService, ISettingsManager settingsManager, IDynamicPropertyService dynamicPropertyService, IPackageService packageService, ICacheManager<object> cacheManager)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _securityService = securityService;
            _roleManagementService = roleManagementService;
            _settingsManager = settingsManager;
            _packageService = packageService;
            _cacheManager = cacheManager;
            _manifestPartUri = PackUriHelper.CreatePartUri(new Uri("Manifest.json", UriKind.Relative));
            _platformEntriesPartUri = PackUriHelper.CreatePartUri(new Uri("PlatformEntries.json", UriKind.Relative));
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
                    Dependencies = x.Dependencies != null ? x.Dependencies.ToArray() : null,
                    Version = x.Version,
                    Description = ((ISupportExportImportModule)x.ModuleInfo.ModuleInstance).ExportDescription
                }).ToArray()
            };

            return retVal;
        }

        public PlatformExportManifest ReadExportManifest(Stream stream)
        {
            PlatformExportManifest retVal = null;
            using (var package = ZipPackage.Open(stream, FileMode.Open))
            {
                var manifestPart = package.GetPart(_manifestPartUri);
                using (var manifestStream = manifestPart.GetStream())
                {
                    retVal = manifestStream.DeserializeJson<PlatformExportManifest>();
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

            using (var package = ZipPackage.Open(outStream, FileMode.Create))
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
                    manifest.SerializeJson<PlatformExportManifest>(stream);
                }
            }
        }

        public void Import(Stream stream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            var progressInfo = new ExportImportProgressInfo();
            progressInfo.Description = "Starting platform import...";
            progressCallback(progressInfo);

            using (var package = ZipPackage.Open(stream, FileMode.Open))
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
                    platformEntries = stream.DeserializeJson<PlatformExportEntries>();
                }

                //Import security objects
                if (manifest.HandleSecurity)
                {
                    progressInfo.Description = String.Format("Import {0} users with roles...", platformEntries.Users.Count());
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
                            _securityService.UpdateAsync(user);
                        }
                        else
                        {
                            _securityService.CreateAsync(user);
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
                progressInfo.Description = String.Format("Security: {0} users exporting...", usersResult.Users.Count());
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
                progressInfo.Description = String.Format("Settings: selected modules settings exporting...");
                progressCallback(progressInfo);

                platformExportObj.Settings = manifest.Modules.SelectMany(x => _settingsManager.GetModuleSettings(x.Id)).ToList();
            }

            //Dynamic properties
            var allTypes = _dynamicPropertyService.GetAvailableObjectTypeNames();

            progressInfo.Description = String.Format("Dynamic properties: load properties...");
            progressCallback(progressInfo);

            platformExportObj.DynamicProperties = allTypes.SelectMany(x => _dynamicPropertyService.GetProperties(x)).ToList();
            platformExportObj.DynamicPropertyDictionaryItems = platformExportObj.DynamicProperties.Where(x => x.IsDictionary).SelectMany(x => _dynamicPropertyService.GetDictionaryItems(x.Id)).ToList();

            //Create part for platform entries
            var platformEntiriesPart = package.CreatePart(_platformEntriesPartUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Normal);
            using (var partStream = platformEntiriesPart.GetStream())
            {
                platformExportObj.SerializeJson<PlatformExportEntries>(partStream);
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
                            progressInfo.Description = String.Format("{0}: {1}", moduleInfo.Id, x.Description);
                            progressCallback(progressInfo);
                        };
                        try
                        {
                            ((ISupportExportImportModule)moduleDescriptor.ModuleInfo.ModuleInstance).DoImport(modulePartStream, manifest, modulePorgressCallback);
                        }
                        catch (Exception ex)
                        {
                            progressInfo.Errors.Add(String.Format("{0}: {1}", moduleInfo.Id, ex.ExpandExceptionMessage()));
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
                        progressInfo.Description = String.Format("{0}: {1}", module.Id, x.Description);
                        progressCallback(progressInfo);
                    };

                    progressInfo.Description = String.Format("{0}: exporting...", module.Id);
                    progressCallback(progressInfo);

                    try
                    {
                        ((ISupportExportImportModule)moduleDescriptor.ModuleInfo.ModuleInstance).DoExport(modulePart.GetStream(), manifest, modulePorgressCallback);
                    }
                    catch (Exception ex)
                    {
                        progressInfo.Errors.Add(String.Format("{0}: {1}", module.Id, ex.ExpandExceptionMessage()));
                        progressCallback(progressInfo);
                    }

                    module.PartUri = modulePartUri.ToString();
                }
            }

        }


        private VirtoCommerce.Platform.Core.Packaging.ModuleDescriptor[] InnerGetModulesWithInterface(Type interfaceType)
        {
            var retVal = _packageService.GetModules().Where(x => x.ModuleInfo.ModuleInstance != null)
                                        .Where(x => x.ModuleInfo.ModuleInstance.GetType().GetInterfaces().Contains(interfaceType))
                                        .ToArray();
            return retVal;
        }

    }
}
