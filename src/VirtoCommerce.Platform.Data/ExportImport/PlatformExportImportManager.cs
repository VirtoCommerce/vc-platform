using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.GenericCrud;
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

        private readonly IModuleService _moduleService;
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
            , IModuleService moduleService
            , IDynamicPropertyDictionaryItemsService dynamicPropertyDictionaryItemsService
            , IDynamicPropertyDictionaryItemsSearchService dynamicPropertyDictionaryItemsSearchService
            , IUserApiKeyService userApiKeyService
            , IUserApiKeySearchService userApiKeySearchService)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _userManager = userManager;
            _roleManager = roleManager;
            _settingsManager = settingsManager;
            _moduleService = moduleService;
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
                Modules = GetModulesWithImportSupport().Select(x => new ExportModuleInfo
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
                using var manifestStream = manifestPart.Open();
                retVal = manifestStream.DeserializeJson<PlatformExportManifest>(GetJsonSerializer());
            }
            return retVal;
        }

        // Cancellation intentionally dropped - callers must migrate to the CancellationToken overload.
        public Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken)
            => ExportAsync(outStream, exportOptions, progressCallback, CancellationToken.None);

        public Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken)
            => ImportAsync(inputStream, importOptions, progressCallback, CancellationToken.None);

        public Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(exportOptions);
            return ExportAsyncCore(outStream, exportOptions, progressCallback, cancellationToken);
        }

        private async Task ExportAsyncCore(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var zipArchive = new ZipArchive(outStream, ZipArchiveMode.Create, true);
            //Export all selected platform entries
            await ExportPlatformEntriesInternalAsync(zipArchive, exportOptions, progressCallback, cancellationToken);
            //Export all selected  modules
            await ExportModulesInternalAsync(zipArchive, exportOptions, progressCallback, cancellationToken);

            //Write system information about exported modules
            var manifestZipEntry = zipArchive.CreateEntry(ManifestZipEntryName, CompressionLevel.Optimal);

            //After all modules exported need write export manifest part
            await using var stream = await manifestZipEntry.OpenAsync(cancellationToken);
            exportOptions.SerializeJson(stream, GetJsonSerializer());
        }

        public Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(importOptions);
            return ImportAsyncCore(inputStream, importOptions, progressCallback, cancellationToken);
        }

        private async Task ImportAsyncCore(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var progressInfo = new ExportImportProgressInfo
            {
                Description = "Starting platform import..."
            };
            progressCallback(progressInfo);

            using (var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read, true))
            using (EventSuppressor.SuppressEvents())
            {
                //Import selected platform entries
                await ImportPlatformEntriesInternalAsync(zipArchive, importOptions, progressCallback, cancellationToken);
                //Import selected modules
                await ImportModulesInternalAsync(zipArchive, importOptions, progressCallback, cancellationToken);
            }
        }

        #endregion IPlatformExportImportManager Members

        private async Task ImportPlatformEntriesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();
            var jsonSerializer = GetJsonSerializer();

            var platformZipEntries = zipArchive.GetEntry(PlatformZipEntryName);
            if (platformZipEntries is null)
            {
                return;
            }

            await using var stream = await platformZipEntries.OpenAsync(cancellationToken);
            await using var reader = new JsonTextReader(new StreamReader(stream));
            while (await reader.ReadAsync(cancellationToken))
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                var token = reader.Value?.ToString();

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

                    case "DynamicProperties" when manifest.HandleDynamicProperties:
                        await ImportDynamicPropertiesInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken);
                        break;

                    case "DynamicPropertyDictionaryItems" when manifest.HandleDynamicProperties:
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

        private Task ImportRolesInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            return reader.DeserializeArrayWithPagingAsync<Role>(jsonSerializer, _batchSize, async items =>
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
                progressInfo.Description = $"{processedCount} roles have been imported";
                progressCallback(progressInfo);
            }, cancellationToken);
        }

        private Task ImportUsersInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            return reader.DeserializeArrayWithPagingAsync<ApplicationUser>(jsonSerializer, _batchSize, async items =>
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
                progressInfo.Description = $"{processedCount} roles have been imported";
                progressCallback(progressInfo);
            }, cancellationToken);
        }

        private Task ImportSettingsInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            var moduleIds = manifest.Modules.Select(x => x.Id).ToHashSet();

            return reader.DeserializeArrayWithPagingAsync<ObjectSettingEntry>(jsonSerializer, _batchSize,
                items => _settingsManager.SaveObjectSettingsAsync(items.Where(x => moduleIds.Contains(x.ModuleId)).ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{processedCount} settings have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private Task ImportDynamicPropertiesInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            return reader.DeserializeArrayWithPagingAsync<DynamicProperty>(jsonSerializer, _batchSize,
                items => _dynamicPropertyService.SaveDynamicPropertiesAsync(items.ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{processedCount} dynamic properties have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private Task ImportDynamicPropertyDictionaryItemsInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            return reader.DeserializeArrayWithPagingAsync<DynamicPropertyDictionaryItem>(jsonSerializer, _batchSize,
                items => _dynamicPropertyDictionaryItemsService.SaveDictionaryItemsAsync(items.ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{processedCount} dynamic property dictionary items have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private Task ImportUserApiKeysInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            return reader.DeserializeArrayWithPagingAsync<UserApiKey>(jsonSerializer, _batchSize,
                items => _userApiKeyService.SaveApiKeysAsync(items.ToArray()),
                processedCount =>
                {
                    progressInfo.Description = $"{processedCount} API keys have been imported";
                    progressCallback(progressInfo);
                }, cancellationToken);
        }

        private async Task ExportPlatformEntriesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();

            var serializer = GetJsonSerializer();
            //Create part for platform entries
            var platformEntriesPart = zipArchive.CreateEntry(PlatformZipEntryName, CompressionLevel.Optimal);

            await using var partStream = await platformEntriesPart.OpenAsync(cancellationToken);
            await using var sw = new StreamWriter(partStream, Encoding.UTF8);
            await using var writer = new JsonTextWriter(sw);

            await writer.WriteStartObjectAsync(cancellationToken);

            if (manifest.HandleSecurity)
            {
                await ExportRolesAsync(writer, serializer, progressInfo, progressCallback, cancellationToken);
                await ExportUsersAsync(writer, serializer, progressInfo, progressCallback, cancellationToken);
                await SerializeArray("UserApiKeys", "User API keys", _userApiKeySearchService, writer);
            }

            if (manifest.HandleSettings)
            {
                await ExportSettingsAsync(writer, serializer, manifest, progressInfo, progressCallback, cancellationToken);
            }

            if (manifest.HandleDynamicProperties)
            {
                await SerializeArray("DynamicProperties", "Dynamic properties", _dynamicPropertySearchService, writer);
                await SerializeArray("DynamicPropertyDictionaryItems", "Dynamic property dictionary items", _dynamicPropertyDictionaryItemsSearchService, writer);
            }

            await writer.WriteEndObjectAsync(cancellationToken);
            await writer.FlushAsync(cancellationToken);

            async Task SerializeArray<TModel, TCriteria, TResult>(
                string name,
                string displayName,
                ISearchService<TCriteria, TResult, TModel> searchService,
                JsonTextWriter jsonTextWriter)
                where TCriteria : SearchCriteriaBase
                where TResult : GenericSearchResult<TModel>
                where TModel : IEntity
            {
                progressInfo.Description = $"{displayName}: exporting...";
                progressCallback(progressInfo);

                await jsonTextWriter.WritePropertyNameAsync(name, cancellationToken);
                await jsonTextWriter.WriteStartArrayAsync(cancellationToken);

                var criteria = AbstractTypeFactory<TCriteria>.TryCreateInstance();
                criteria.Take = _batchSize;

                await foreach (var searchResult in searchService.SearchBatchesNoCloneAsync(criteria))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    foreach (var item in searchResult.Results)
                    {
                        serializer.Serialize(jsonTextWriter, item);
                    }
                }

                await jsonTextWriter.WriteEndArrayAsync(cancellationToken);
                await jsonTextWriter.FlushAsync(cancellationToken);

                progressInfo.Description = $"{displayName}: exported";
                progressCallback(progressInfo);
            }
        }

        private async Task ExportRolesAsync(JsonTextWriter writer, JsonSerializer serializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            progressInfo.Description = "Roles exporting...";
            progressCallback(progressInfo);

            await writer.WritePropertyNameAsync("Roles", cancellationToken);
            await writer.WriteStartArrayAsync(cancellationToken);

            var roles = _roleManager.Roles.ToList();
            if (_roleManager.SupportsRoleClaims)
            {
                foreach (var role in roles)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var fullyLoadedRole = await _roleManager.FindByIdAsync(role.Id);
                    serializer.Serialize(writer, fullyLoadedRole);
                }

                await writer.FlushAsync(cancellationToken);
                progressInfo.Description = $"{roles.Count} roles exported";
                progressCallback(progressInfo);
            }

            await writer.WriteEndArrayAsync(cancellationToken);
        }

        private async Task ExportUsersAsync(JsonTextWriter writer, JsonSerializer serializer, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await writer.WritePropertyNameAsync("Users", cancellationToken);
            await writer.WriteStartArrayAsync(cancellationToken);
            var usersResult = _userManager.Users.ToArray();
            progressInfo.Description = $"Security: {usersResult.Length} users exporting...";
            progressCallback(progressInfo);
            var userExported = 0;

            foreach (var user in usersResult)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var userExt = await _userManager.FindByIdAsync(user.Id);
                if (userExt != null)
                {
                    serializer.Serialize(writer, userExt);
                    userExported++;
                }
            }

            await writer.FlushAsync(cancellationToken);
            progressInfo.Description = $"{userExported} of {usersResult.Length} users exported";
            progressCallback(progressInfo);

            await writer.WriteEndArrayAsync(cancellationToken);
        }

        private async Task ExportSettingsAsync(JsonTextWriter writer, JsonSerializer serializer, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await writer.WritePropertyNameAsync("Settings", cancellationToken);
            await writer.WriteStartArrayAsync(cancellationToken);

            progressInfo.Description = "Settings: selected modules settings exporting...";
            progressCallback(progressInfo);

            foreach (var module in manifest.Modules ?? [])
            {
                var moduleSettings = await _settingsManager.GetObjectSettingsAsync(
                    _settingsManager.AllRegisteredSettings
                        .Where(x => x.ModuleId == module.Id)
                        .Select(x => x.Name));
                //Export only settings with set values
                foreach (var setting in moduleSettings.Where(x => x.ItHasValues))
                {
                    serializer.Serialize(writer, setting);
                }

                await writer.FlushAsync(cancellationToken);
            }

            progressInfo.Description = "Settings of modules exported";
            progressCallback(progressInfo);
            await writer.WriteEndArrayAsync(cancellationToken);
        }

        private async Task ImportModulesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            var errors = new StringBuilder();
            var progressInfo = new ExportImportProgressInfo();
            foreach (var moduleInfo in manifest.Modules ?? [])
            {
                var moduleDescriptor = GetModulesWithImportSupport().FirstOrDefault(x => x.Id == moduleInfo.Id);
                if (moduleDescriptor != null)
                {
                    var modulePart = zipArchive.GetEntry(moduleInfo.PartUri.TrimStart('/'));
                    await using var modulePartStream = await modulePart.OpenAsync(cancellationToken);
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
                                .DefaultIfEmpty(new ExportImportOptions { HandleBinaryData = manifest.HandleBinaryData, ModuleIdentity = new ModuleIdentity(moduleDescriptor.Identity.Id, moduleDescriptor.Identity.Version, false) })
                                .FirstOrDefault(x => x.ModuleIdentity.Id == moduleDescriptor.Identity.Id);
                            await importer.ImportAsync(modulePartStream, options, ModuleProgressCallback, cancellationToken);
                        }
                        catch (OperationCanceledException)
                        {
                            // Let cancellation reach the outer job handler, not be swallowed as a module error.
                            throw;
                        }
                        catch (Exception ex)
                        {
                            errors.AppendLine($"<b> {moduleInfo.Id} </b>: {ex} <br><br>");
                            progressInfo.Errors.Add($"{moduleInfo.Id}: {ex}");
                            progressCallback(progressInfo);
                        }
                    }
                }
            }

            if (errors.Length != 0)
            {
                throw new InvalidOperationException(errors.ToString());
            }
        }

        private async Task ExportModulesInternalAsync(ZipArchive zipArchive, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
        {
            var progressInfo = new ExportImportProgressInfo();

            foreach (var module in manifest.Modules ?? [])
            {
                var moduleDescriptor = GetModulesWithImportSupport().FirstOrDefault(x => x.Id == module.Id);
                if (moduleDescriptor != null)
                {
                    //Create part for module
                    var moduleZipEntryName = module.Id + ".json";
                    var zipEntry = zipArchive.CreateEntry(moduleZipEntryName, CompressionLevel.Optimal);

                    void ModuleProgressCallback(ExportImportProgressInfo x)
                    {
                        progressInfo.Description = $"{module.Id}: {x.Description}";
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
                                .DefaultIfEmpty(new ExportImportOptions { HandleBinaryData = manifest.HandleBinaryData, ModuleIdentity = new ModuleIdentity(module.Id, SemanticVersion.Parse(module.Version.Trim()), module.Optional) })
                                .FirstOrDefault(x => x.ModuleIdentity.Id == moduleDescriptor.Identity.Id);

                            await using var stream = await zipEntry.OpenAsync(cancellationToken);
                            await exporter.ExportAsync(stream, options, ModuleProgressCallback, cancellationToken);
                        }
                        catch (OperationCanceledException)
                        {
                            // Let cancellation reach the outer job handler, not be swallowed as a module error.
                            throw;
                        }
                        catch (Exception ex)
                        {
                            progressInfo.Errors.Add($"{module.Id}: {ex}");
                            progressCallback(progressInfo);
                        }
                    }
                    module.PartUri = moduleZipEntryName;
                }
            }
        }

        private IEnumerable<ManifestModuleInfo> GetModulesWithImportSupport()
        {
            return _moduleService.GetInstalledModules().Where(x => x.ModuleInstance is IImportSupport);
        }

        private static JsonSerializer GetJsonSerializer()
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            return serializer;
        }
    }
}
