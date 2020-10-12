using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.ExportImport
{
    public class PlatformExportImportManager : IPlatformExportImportManager
    {
        private const string ManifestZipEntryName = "Manifest.json";
        private const string PlatformZipEntryName = "PlatformEntries.json";

        private readonly ILocalModuleCatalog _moduleCatalog;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly IDynamicPropertySearchService _dynamicPropertySearchService;
        private readonly IUserApiKeyService _userApiKeyService;
        private readonly IUserApiKeySearchService _userApiKeySearchService;
        private readonly IDynamicPropertyDictionaryItemsService _dynamicPropertyDictionaryItemsService;
        private readonly IDynamicPropertyDictionaryItemsSearchService _dynamicPropertyDictionaryItemsSearchService;

        private readonly int _batchSize = 20;

        public PlatformExportImportManager(
            UserManager<ApplicationUser> userManager
            , RoleManager<Role> roleManager
            , ISettingsManager settingsManager
            , IDynamicPropertyService dynamicPropertyService
            , IDynamicPropertySearchService dynamicPropertySearchService
            , ILocalModuleCatalog moduleCatalog
            , IDynamicPropertyDictionaryItemsService dynamicPropertyDictionaryItemsService
            , IDynamicPropertyDictionaryItemsSearchService dynamicPropertyDictionaryItemsSearchService
            , IUserApiKeyService userApiKeyService
            , IUserApiKeySearchService userApiKeySearchService)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _userManager = userManager;
            _roleManager = roleManager;
            _settingsManager = settingsManager;
            _moduleCatalog = moduleCatalog;
            _dynamicPropertyDictionaryItemsService = dynamicPropertyDictionaryItemsService;
            _dynamicPropertyDictionaryItemsSearchService = dynamicPropertyDictionaryItemsSearchService;
            _dynamicPropertySearchService = dynamicPropertySearchService;
            _userApiKeyService = userApiKeyService;
            _userApiKeySearchService = userApiKeySearchService;
        }

        #region IPlatformExportImportManager Members

        public PlatformExportManifest GetNewExportManifest(string author)
        {
            var retVal = new PlatformExportManifest
            {
                Author = author,
                PlatformVersion = PlatformVersion.CurrentVersion.ToString(),
                Modules = InnerGetModulesWithInterface(typeof(IImportSupport)).Select(x => new ExportModuleInfo
                {
                    Id = x.Id,
                    Version = x.Version.ToString()
                }).ToArray()
            };

            return retVal;
        }

        public PlatformExportManifest ReadExportManifest(Stream stream)
        {
            PlatformExportManifest retVal;
            using (var package = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                var manifestPart = package.GetEntry(ManifestZipEntryName);
                using (var manifestStream = manifestPart.Open())
                {
                    retVal = manifestStream.DeserializeJson<PlatformExportManifest>(GetJsonSerializer());
                }
            }
            return retVal;
        }

        public async Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken)
        {
            if (exportOptions == null)
            {
                throw new ArgumentNullException(nameof(exportOptions));
            }

            using (var zipArchive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
                //Export all selected platform entries
                await ExportPlatformEntriesInternalAsync(zipArchive, exportOptions, progressCallback, сancellationToken);
                //Export all selected  modules
                await ExportModulesInternalAsync(zipArchive, exportOptions, progressCallback, сancellationToken);

                //Write system information about exported modules
                var manifestZipEntry = zipArchive.CreateEntry(ManifestZipEntryName, CompressionLevel.Optimal);

                //After all modules exported need write export manifest part
                using (var stream = manifestZipEntry.Open())
                {
                    exportOptions.SerializeJson(stream, GetJsonSerializer());
                }
            }
        }

        public async Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken)
        {
            if (importOptions == null)
            {
                throw new ArgumentNullException(nameof(importOptions));
            }

            var progressInfo = new ExportImportProgressInfo();
            progressInfo.Description = "Starting platform import...";
            progressCallback(progressInfo);

            using (var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read, true))
            using (EventSuppressor.SupressEvents())
            {
                //Import selected platform entries
                await ImportPlatformEntriesInternalAsync(zipArchive, importOptions, progressCallback, сancellationToken);
                //Import selected modules
                await ImportModulesInternalAsync(zipArchive, importOptions, progressCallback, сancellationToken);
            }
        }

        #endregion IPlatformExportImportManager Members

        private async Task ImportPlatformEntriesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();
            var jsonSerializer = GetJsonSerializer();

            var platformZipEntries = zipArchive.GetEntry(PlatformZipEntryName);
            if (platformZipEntries is null)
            {
                return;
            }

            using var stream = platformZipEntries.Open();
            var streamReader = new StreamReader(stream);
            using var reader = new JsonTextReader(streamReader);
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                var token = reader.Value.ToString();

                switch (token)
                {
                    case "Roles" when manifest.HandleSecurity:
                        await ImportRolesInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken);
                        break;

                    case "Users" when manifest.HandleSecurity:
                        await ImportUsersInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken);
                        break;

                    case "Settings" when manifest.HandleSettings:
                        await ImportSettingsInternalAsync(reader, jsonSerializer, manifest, progressInfo, progressCallback, cancellationToken);
                        break;

                    case "DynamicProperties" when manifest.HandleSettings:
                        await ImportDynamicPropertiesInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken);
                        break;

                    case "DynamicPropertyDictionaryItems" when manifest.HandleSettings:
                        await ImportDynamicPropertyDictionaryItemsInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken);
                        break;

                    case "UserApiKeys" when manifest.HandleSecurity:
                        await ImportUserApiKeysInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken);
                        break;

                    default:
                        continue;
                }
            }
        }

        private async Task ImportRolesInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.DeserializeJsonArrayWithPagingAsync<Role>(jsonSerializer, _batchSize, async items =>
            {
                foreach (var role in items)
                {
                    var roleExists = !(string.IsNullOrEmpty(role.Id) || await _roleManager.FindByIdAsync(role.Id) is null)
                        || await _roleManager.RoleExistsAsync(role.Name);

                    var result = roleExists
                        ? await _roleManager.UpdateAsync(role)
                        : await _roleManager.CreateAsync(role);

                    if (!result.Succeeded)
                    {
                        progressInfo.Errors.AddRange(result.Errors.Select(x => x.Description));
                    }
                }
            }, processedCount =>
            {
                progressInfo.Description = $"{ processedCount } roles have been imported";
                progressCallback(progressInfo);
            }, cancellationToken);
        }

        private async Task ImportUsersInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.DeserializeJsonArrayWithPagingAsync<ApplicationUser>(jsonSerializer, _batchSize, async items =>
            {
                foreach (var user in items)
                {
                    var userExists = !(string.IsNullOrEmpty(user.Id) || await _userManager.FindByIdAsync(user.Id) is null)
                        || (await _userManager.FindByNameAsync(user.UserName)) != null;

                    var result = userExists
                        ? await _userManager.UpdateAsync(user)
                        : await _userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        progressInfo.Errors.AddRange(result.Errors.Select(x => x.Description));
                    }
                }
            }, processedCount =>
            {
                progressInfo.Description = $"{ processedCount } roles have been imported";
                progressCallback(progressInfo);
            }, cancellationToken);
        }

        private async Task ImportSettingsInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.DeserializeJsonArrayWithPagingAsync<ObjectSettingEntry>(jsonSerializer, int.MaxValue, async items =>
            {
                var arrayItems = items.ToArray();
                foreach (var module in manifest.Modules)
                {
                    await _settingsManager.SaveObjectSettingsAsync(arrayItems.Where(x => x.ModuleId == module.Id).ToArray());
                }
            }, processedCount =>
            {
                progressInfo.Description = $"{ processedCount } coupons have been imported";
                progressCallback(progressInfo);
            }, cancellationToken);
        }

        private async Task ImportDynamicPropertiesInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.DeserializeJsonArrayWithPagingAsync<DynamicProperty>(jsonSerializer, _batchSize,
                items => _dynamicPropertyService.SaveDynamicPropertiesAsync(items.ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{ processedCount } coupons have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private async Task ImportDynamicPropertyDictionaryItemsInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.DeserializeJsonArrayWithPagingAsync<DynamicPropertyDictionaryItem>(jsonSerializer, _batchSize,
                items => _dynamicPropertyDictionaryItemsService.SaveDictionaryItemsAsync(items.ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{ processedCount } coupons have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private async Task ImportUserApiKeysInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            await reader.DeserializeJsonArrayWithPagingAsync<UserApiKey>(jsonSerializer, _batchSize,
                items => _userApiKeyService.SaveApiKeysAsync(items.ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{ processedCount } api keys have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private async Task ExportPlatformEntriesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();

            var serializer = GetJsonSerializer();
            //Create part for platform entries
            var platformEntiriesPart = zipArchive.CreateEntry(PlatformZipEntryName, CompressionLevel.Optimal);
            using (var partStream = platformEntiriesPart.Open())
            {
                using (var sw = new StreamWriter(partStream, Encoding.UTF8))
                using (var writer = new JsonTextWriter(sw))
                {
                    await writer.WriteStartObjectAsync();

                    if (manifest.HandleSecurity)
                    {
                        #region Roles

                        progressInfo.Description = "Roles exporting...";
                        progressCallback(progressInfo);
                        cancellationToken.ThrowIfCancellationRequested();

                        await writer.WritePropertyNameAsync("Roles");
                        await writer.WriteStartArrayAsync();

                        var roles = _roleManager.Roles.ToList();
                        if (_roleManager.SupportsRoleClaims)
                        {
                            foreach (var role in roles)
                            {
                                var fullyLoadedRole = await _roleManager.FindByIdAsync(role.Id);
                                serializer.Serialize(writer, fullyLoadedRole);
                            }

                            writer.Flush();
                            progressInfo.Description = $"{ roles.Count } roles exported";
                            progressCallback(progressInfo);
                        }

                        await writer.WriteEndArrayAsync();

                        #endregion Roles

                        cancellationToken.ThrowIfCancellationRequested();

                        #region Users

                        await writer.WritePropertyNameAsync("Users");
                        await writer.WriteStartArrayAsync();
                        var usersResult = _userManager.Users.ToArray();
                        progressInfo.Description = $"Security: {usersResult.Length} users exporting...";
                        progressCallback(progressInfo);
                        var userExported = 0;

                        foreach (var user in usersResult)
                        {
                            var userExt = await _userManager.FindByIdAsync(user.Id);
                            if (userExt != null)
                            {
                                serializer.Serialize(writer, userExt);
                                userExported++;
                            }
                        }

                        await writer.FlushAsync();
                        progressInfo.Description = $"{ userExported } of { usersResult.Length } users exported";
                        progressCallback(progressInfo);

                        await writer.WriteEndArrayAsync();

                        #endregion Users

                        cancellationToken.ThrowIfCancellationRequested();

                        #region UserApiKeys

                        await writer.WritePropertyNameAsync("UserApiKeys");
                        await writer.WriteStartArrayAsync();

                        progressInfo.Description = "User Api keys: load keys...";
                        progressCallback(progressInfo);

                        var apiKeys = (await _userApiKeySearchService.SearchUserApiKeysAsync(new UserApiKeySearchCriteria { Take = int.MaxValue })).Results;
                        foreach (var apiKey in apiKeys)
                        {
                            serializer.Serialize(writer, apiKey);
                        }

                        progressInfo.Description = $"User Api keys have been exported";
                        progressCallback(progressInfo);
                        await writer.WriteEndArrayAsync();

                        #endregion UserApiKeys
                    }

                    if (manifest.HandleSettings)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        await writer.WritePropertyNameAsync("Settings");
                        await writer.WriteStartArrayAsync();

                        progressInfo.Description = "Settings: selected modules settings exporting...";
                        progressCallback(progressInfo);
                        foreach (var module in manifest.Modules)
                        {
                            var moduleSettings = await _settingsManager.GetObjectSettingsAsync(_settingsManager.AllRegisteredSettings.Where(x => x.ModuleId == module.Id).Select(x => x.Name));
                            //Export only settings with set values
                            foreach (var setting in moduleSettings.Where(x => x.ItHasValues))
                            {
                                serializer.Serialize(writer, setting);
                            }

                            await writer.FlushAsync();
                        }

                        progressInfo.Description = $"Settings of modules exported";
                        progressCallback(progressInfo);
                        await writer.WriteEndArrayAsync();
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    await writer.WritePropertyNameAsync("DynamicProperties");
                    await writer.WriteStartArrayAsync();

                    progressInfo.Description = "Dynamic properties: load properties...";
                    progressCallback(progressInfo);

                    var dynamicProperties = (await _dynamicPropertySearchService.SearchDynamicPropertiesAsync(new DynamicPropertySearchCriteria { Take = int.MaxValue })).Results;
                    foreach (var dynamicProperty in dynamicProperties)
                    {
                        serializer.Serialize(writer, dynamicProperty);
                    }

                    progressInfo.Description = $"Dynamic properties exported";
                    progressCallback(progressInfo);
                    await writer.WriteEndArrayAsync();

                    cancellationToken.ThrowIfCancellationRequested();

                    await writer.WritePropertyNameAsync("DynamicPropertyDictionaryItems");
                    await writer.WriteStartArrayAsync();

                    progressInfo.Description = "Dynamic properties Dictionary Items: load properties...";
                    progressCallback(progressInfo);

                    var dynamicPropertyDictionaryItems = (await _dynamicPropertyDictionaryItemsSearchService.SearchDictionaryItemsAsync(new DynamicPropertyDictionaryItemSearchCriteria { Take = int.MaxValue })).Results;
                    foreach (var dynamicPropertyDictionaryItem in dynamicPropertyDictionaryItems)
                    {
                        serializer.Serialize(writer, dynamicPropertyDictionaryItem);
                    }

                    progressInfo.Description = $"Dynamic properties dictionary items exported";
                    progressCallback(progressInfo);
                    await writer.WriteEndArrayAsync();

                    await writer.WriteEndObjectAsync();
                    await writer.FlushAsync();
                }
            }
        }

        private async Task ImportModulesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();
            foreach (var moduleInfo in manifest.Modules)
            {
                var moduleDescriptor = InnerGetModulesWithInterface(typeof(IImportSupport)).FirstOrDefault(x => x.Id == moduleInfo.Id);
                if (moduleDescriptor != null)
                {
                    var modulePart = zipArchive.GetEntry(moduleInfo.PartUri.TrimStart('/'));
                    using (var modulePartStream = modulePart.Open())
                    {
                        void ModuleProgressCallback(ExportImportProgressInfo x)
                        {
                            progressInfo.Description = $"{moduleInfo.Id}: {x.Description}";
                            progressInfo.Errors = x.Errors;
                            progressCallback(progressInfo);
                        }
                        if (moduleDescriptor.ModuleInstance is IImportSupport importer)
                        {
                            try
                            {
                                //TODO: Add JsonConverter which will be materialized concrete ExportImport option type
                                var options = manifest.Options
                                    .DefaultIfEmpty(new ExportImportOptions { HandleBinaryData = manifest.HandleBinaryData, ModuleIdentity = new ModuleIdentity(moduleDescriptor.Identity.Id, moduleDescriptor.Identity.Version) })
                                    .FirstOrDefault(x => x.ModuleIdentity.Id == moduleDescriptor.Identity.Id);
                                await importer.ImportAsync(modulePartStream, options, ModuleProgressCallback, cancellationToken);
                            }
                            catch (Exception ex)
                            {
                                progressInfo.Errors.Add($"{moduleInfo.Id}: {ex}");
                                progressCallback(progressInfo);
                            }
                        }
                    }
                }
            }
        }

        private async Task ExportModulesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();

            foreach (var module in manifest.Modules)
            {
                var moduleDescriptor = InnerGetModulesWithInterface(typeof(IImportSupport)).FirstOrDefault(x => x.Id == module.Id);
                if (moduleDescriptor != null)
                {
                    //Create part for module
                    var moduleZipEntryName = module.Id + ".json";
                    var zipEntry = zipArchive.CreateEntry(moduleZipEntryName, CompressionLevel.Optimal);

                    void ModuleProgressCallback(ExportImportProgressInfo x)
                    {
                        progressInfo.Description = $"{ module.Id }: { x.Description }";
                        progressInfo.Errors = x.Errors;
                        progressCallback(progressInfo);
                    }

                    progressInfo.Description = $"{module.Id}: exporting...";
                    progressCallback(progressInfo);
                    if (moduleDescriptor.ModuleInstance is IExportSupport exporter)
                    {
                        try
                        {
                            //TODO: Add JsonConverter which will be materialized concrete ExportImport option type
                            //ToDo: Added check ExportImportOptions for modules (DefaultIfEmpty)
                            var options = manifest.Options
                                .DefaultIfEmpty(new ExportImportOptions { HandleBinaryData = manifest.HandleBinaryData, ModuleIdentity = new ModuleIdentity(module.Id, SemanticVersion.Parse(module.Version)) })
                                .FirstOrDefault(x => x.ModuleIdentity.Id == moduleDescriptor.Identity.Id);
                            await exporter.ExportAsync(zipEntry.Open(), options, ModuleProgressCallback, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            progressInfo.Errors.Add($"{ module.Id}: {ex}");
                            progressCallback(progressInfo);
                        }
                    }
                    module.PartUri = moduleZipEntryName;
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
