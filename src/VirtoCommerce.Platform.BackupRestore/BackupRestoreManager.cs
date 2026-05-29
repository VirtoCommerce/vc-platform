using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.BackupRestore;

public class BackupRestoreManager : IPlatformExportImportManager
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
    private readonly IZipBackupArchiveFactory _zipFactory;

    private readonly int _batchSize = 50;

    public BackupRestoreManager(
        UserManager<ApplicationUser> userManager
        , RoleManager<Role> roleManager
        , ISettingsManager settingsManager
        , IDynamicPropertyService dynamicPropertyService
        , IDynamicPropertySearchService dynamicPropertySearchService
        , IModuleService moduleService
        , IDynamicPropertyDictionaryItemsService dynamicPropertyDictionaryItemsService
        , IDynamicPropertyDictionaryItemsSearchService dynamicPropertyDictionaryItemsSearchService
        , IUserApiKeyService userApiKeyService
        , IUserApiKeySearchService userApiKeySearchService
        , IZipBackupArchiveFactory zipFactory)
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
        _zipFactory = zipFactory;
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
        // Manifest.json is always written without encryption (see `ExportAsync`), so no
        // password is needed here even for encrypted backups. Reading it first lets the
        // import UX detect `IsEncrypted` and prompt the user before we open any encrypted
        // entries downstream.
        PlatformExportManifest retVal;
        using (var package = _zipFactory.OpenForReading(stream, password: null))
        {
            using var manifestStream = package.OpenEntryAsync(ManifestZipEntryName).GetAwaiter().GetResult()
                ?? throw new PlatformException($"Backup is missing required entry '{ManifestZipEntryName}'.");
            retVal = manifestStream.DeserializeJson<PlatformExportManifest>(GetJsonSerializer());
        }
        return retVal;
    }

    public async Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken сancellationToken)
    {
        ArgumentNullException.ThrowIfNull(exportOptions);

        сancellationToken.ThrowIfCancellationRequested();

        var progressInfo = new ExportImportProgressInfo
        {
            TotalCount = EstimateImportItemCount(exportOptions),
        };
        ReportProgress(progressInfo, progressCallback, "Starting platform export...");

        // Stamp the manifest with the encryption flag based on whether a password was
        // threaded through. Serialized into Manifest.json so the import flow can detect it.
        exportOptions.IsEncrypted = !string.IsNullOrEmpty(exportOptions.Password);

        await using var zipArchive = _zipFactory.CreateForWriting(outStream, exportOptions.Password);
        //Export all selected platform entries
        await ExportPlatformEntriesInternalAsync(zipArchive, exportOptions, progressInfo, progressCallback, сancellationToken);
        //Export all selected  modules
        await ExportModulesInternalAsync(zipArchive, exportOptions, progressInfo, progressCallback, сancellationToken);

        // Write the manifest LAST so it includes the per-module PartUri values stamped during
        // module export. Always written unencrypted (leaveUnencrypted: true) so the import
        // side can read it to detect encryption without first prompting for the password.
        await using var stream = await zipArchive.CreateEntryAsync(ManifestZipEntryName, leaveUnencrypted: true);
        exportOptions.SerializeJson(stream, GetJsonSerializer());
    }

    public async Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken сancellationToken)
    {
        ArgumentNullException.ThrowIfNull(importOptions);
        сancellationToken.ThrowIfCancellationRequested();

        var progressInfo = new ExportImportProgressInfo
        {
            TotalCount = EstimateImportItemCount(importOptions),
        };
        ReportProgress(progressInfo, progressCallback, "Starting platform import...");

        await using var zipArchive = _zipFactory.OpenForReading(inputStream, importOptions.Password);
        using var _ = EventSuppressor.SuppressEvents();

        try
        {
            //Import selected platform entries
            await ImportPlatformEntriesInternalAsync(zipArchive, importOptions, progressInfo, progressCallback, сancellationToken);
            //Import selected modules
            await ImportModulesInternalAsync(zipArchive, importOptions, progressInfo, progressCallback, сancellationToken);
        }
        catch (ZipException ex) when (importOptions.IsEncrypted && IsLikelyCryptoFailure(ex))
        {
            throw new PlatformException("Invalid backup password", ex);
        }
        catch (SharpZipBaseException ex) when (importOptions.IsEncrypted && IsLikelyCryptoFailure(ex))
        {
            throw new PlatformException("Invalid backup password", ex);
        }
    }

    /// <summary>
    /// SharpZipLib doesn't expose a stable, typed "bad password" exception — different
    /// versions emit `ZipException("Wrong password")`, `ZipException("invalid CRC")`, or
    /// even a `SharpZipBaseException` whose message references the encryption header.
    /// We sniff the message rather than the type so a version bump doesn't silently
    /// reclassify these as generic corruption errors and produce a misleading "backup is
    /// corrupt" toast for what's really just a typo in the password.
    /// </summary>
    private static bool IsLikelyCryptoFailure(Exception ex)
    {
        if (ex == null)
        {
            return false;
        }
        var message = ex.Message ?? string.Empty;
        return message.Contains("password", StringComparison.OrdinalIgnoreCase)
            || message.Contains("CRC", StringComparison.OrdinalIgnoreCase)
            || message.Contains("AES", StringComparison.OrdinalIgnoreCase)
            || message.Contains("encryption", StringComparison.OrdinalIgnoreCase);
    }

    #endregion IPlatformExportImportManager Members

    private static int EstimateImportItemCount(PlatformExportManifest manifest)
    {
        var moduleCount = manifest.Modules?.Count ?? 0;
        // Mirror the JSON tokens emitted on export: 3 security sections + 1 settings + 2 dynamic-property sections.
        var platformSectionCount =
            (manifest.HandleSecurity ? 3 : 0) +
            (manifest.HandleSettings ? 1 : 0) +
            (manifest.HandleDynamicProperties ? 2 : 0);
        return moduleCount + platformSectionCount;
    }

    /// <summary>
    /// Emits a structured progress message as a delta on <paramref name="progressInfo"/>,
    /// invokes the callback, then clears the delta so subsequent legacy callbacks
    /// (which only update <see cref="ExportImportProgressInfo.Description"/>) don't re-emit it.
    /// Errors are also accumulated into the legacy <see cref="ExportImportProgressInfo.Errors"/> list.
    /// </summary>
    private static void ReportProgress(ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, string message, ProgressMessageLevel level = ProgressMessageLevel.Info)
    {
        progressInfo.Description = message;
        progressInfo.ProgressLog =
        [
            new() { Level = level, Message = message },
        ];
        if (level == ProgressMessageLevel.Error)
        {
            progressInfo.Errors ??= new List<string>();
            if (!progressInfo.Errors.Contains(message))
            {
                progressInfo.Errors.Add(message);
            }
        }
        try
        {
            progressCallback(progressInfo);
        }
        finally
        {
            progressInfo.ProgressLog = [];
        }
    }

    private static List<string> AppendNewErrors(ICollection<string> target, ICollection<string> incoming)
    {
        var newErrors = new List<string>();
        if (incoming == null || target == null)
        {
            return newErrors;
        }
        foreach (var error in incoming.Where(e => !target.Contains(e)))
        {
            target.Add(error);
            newErrors.Add(error);
        }
        return newErrors;
    }

    private async Task ImportPlatformEntriesInternalAsync(IZipBackupArchive zipArchive, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
    {
        var jsonSerializer = GetJsonSerializer();

        var stream = await zipArchive.OpenEntryAsync(PlatformZipEntryName);
        if (stream is null)
        {
            return;
        }

        await using var _disposable = stream;
        await using var reader = new JsonTextReader(new StreamReader(stream));

        // Section names live at depth 1 — directly inside the root `{ ... }` of PlatformEntries.json.
        // Anything deeper (e.g. property names inside individual Role / User / Settings objects) must be
        // ignored by the dispatch: an exception during a section's deserialization can leave the reader
        // anywhere in the section's array, and we don't want a stray nested property name to trigger
        // a second import action. `SafeImportSectionAsync` also re-syncs the reader to depth 1 after a
        // failure as belt-and-suspenders.
        const int sectionDepth = 1;
        while (await reader.ReadAsync())
        {
            if (reader.TokenType != JsonToken.PropertyName || reader.Depth != sectionDepth)
            {
                continue;
            }

            var token = reader.Value?.ToString();

            switch (token)
            {
                case "Roles" when manifest.HandleSecurity:
                    await SafeImportSectionAsync(token, reader, sectionDepth, () => ImportRolesInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken), progressInfo, progressCallback);
                    break;

                case "Users" when manifest.HandleSecurity:
                    await SafeImportSectionAsync(token, reader, sectionDepth, () => ImportUsersInternalAsync(reader, jsonSerializer, manifest.CallerUserName, progressInfo, progressCallback, cancellationToken), progressInfo, progressCallback);
                    break;

                case "Settings" when manifest.HandleSettings:
                    await SafeImportSectionAsync(token, reader, sectionDepth, () => ImportSettingsInternalAsync(reader, jsonSerializer, manifest, progressInfo, progressCallback, cancellationToken), progressInfo, progressCallback);
                    break;

                case "DynamicProperties" when manifest.HandleDynamicProperties:
                    await SafeImportSectionAsync(token, reader, sectionDepth, () => ImportDynamicPropertiesInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken), progressInfo, progressCallback);
                    break;

                case "DynamicPropertyDictionaryItems" when manifest.HandleDynamicProperties:
                    await SafeImportSectionAsync(token, reader, sectionDepth, () => ImportDynamicPropertyDictionaryItemsInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken), progressInfo, progressCallback);
                    break;

                case "UserApiKeys" when manifest.HandleSecurity:
                    await SafeImportSectionAsync(token, reader, sectionDepth, () => ImportUserApiKeysInternalAsync(reader, jsonSerializer, progressInfo, progressCallback, cancellationToken), progressInfo, progressCallback);
                    break;

                default:
                    continue;
            }
        }
    }

    /// <summary>
    /// Invokes <paramref name="importAction"/> with try/catch around the per-section deserialization.
    /// On failure, advances <paramref name="reader"/> back to <paramref name="sectionDepth"/> so the
    /// outer dispatch loop resumes at the next top-level property and doesn't iterate over leftover
    /// tokens inside the failed section's array.
    /// </summary>
    private static async Task SafeImportSectionAsync(
        string sectionName,
        JsonTextReader reader,
        int sectionDepth,
        Func<Task> importAction,
        ExportImportProgressInfo progressInfo,
        Action<ExportImportProgressInfo> progressCallback)
    {
        ReportProgress(progressInfo, progressCallback, $"Importing '{sectionName}'");
        var errorsBefore = progressInfo.Errors?.Count ?? 0;
        try
        {
            await importAction();

            progressInfo.ProcessedCount++;
            var newErrors = (progressInfo.Errors?.Count ?? 0) - errorsBefore;
            if (newErrors > 0)
            {
                // Surface the per-error details (which the inner action already accumulated into Errors)
                // as Error-level progress log entries so the timeline UI can attach them to the section.
                foreach (var error in progressInfo.Errors.Skip(errorsBefore).Take(newErrors).ToList())
                {
                    ReportProgress(progressInfo, progressCallback, error, ProgressMessageLevel.Error);
                }
                ReportProgress(progressInfo, progressCallback, $"Imported '{sectionName}' with {newErrors} error(s)", ProgressMessageLevel.Error);
            }
            else
            {
                ReportProgress(progressInfo, progressCallback, $"Successfully imported '{sectionName}'");
            }
        }
        catch (Exception ex)
        {
            progressInfo.ProcessedCount++;
            ReportProgress(progressInfo, progressCallback, $"Failed to import '{sectionName}': {ex.Message}", ProgressMessageLevel.Error);

            // The reader is now somewhere inside the failed section's array (or part-way through a
            // single item that threw mid-deserialization). Skip any remaining tokens until we're back
            // at the section level so the next loop iteration starts at the next top-level property.
            while (reader.Depth > sectionDepth)
            {
                if (!await reader.ReadAsync())
                {
                    break;
                }
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

    private Task ImportUsersInternalAsync(JsonTextReader reader, JsonSerializer jsonSerializer, string callerUserName, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
    {
        return reader.DeserializeArrayWithPagingAsync<ApplicationUser>(jsonSerializer, _batchSize, async items =>
        {
            foreach (var user in items)
            {
                // Protect the admin who initiated the restore: skip their record so the
                // backup's PasswordHash / SecurityStamp / LockoutEnd don't overwrite the
                // active session's credentials. Without this guard, the admin is logged out
                // mid-restore and can't log back in with the password they used to start it.
                if (!string.IsNullOrEmpty(callerUserName) &&
                    string.Equals(user.UserName, callerUserName, StringComparison.OrdinalIgnoreCase))
                {
                    ReportProgress(progressInfo, progressCallback,
                        $"User '{user.UserName}' skipped to preserve your active session — password and security stamp left unchanged.");
                    continue;
                }

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
            progressInfo.Description = $"{processedCount} users have been imported";
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

    private async Task ExportPlatformEntriesInternalAsync(IZipBackupArchive zipArchive, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
    {
        var serializer = GetJsonSerializer();
        //Create part for platform entries (encrypted if the archive has a password)
        await using var partStream = await zipArchive.CreateEntryAsync(PlatformZipEntryName);
        await using var sw = new StreamWriter(partStream, Encoding.UTF8);
        await using var writer = new JsonTextWriter(sw);

        await writer.WriteStartObjectAsync();

        if (manifest.HandleSecurity)
        {
            #region Roles

            cancellationToken.ThrowIfCancellationRequested();

            ReportProgress(progressInfo, progressCallback, "Exporting 'Roles'");
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

                await writer.FlushAsync();
            }

            await writer.WriteEndArrayAsync();
            progressInfo.ProcessedCount++;
            ReportProgress(progressInfo, progressCallback, $"Successfully exported 'Roles' ({roles.Count})");

            #endregion Roles

            #region Users

            cancellationToken.ThrowIfCancellationRequested();

            ReportProgress(progressInfo, progressCallback, "Exporting 'Users'");
            await writer.WritePropertyNameAsync("Users");
            await writer.WriteStartArrayAsync();
            var usersResult = _userManager.Users.ToArray();
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
            await writer.WriteEndArrayAsync();
            progressInfo.ProcessedCount++;
            ReportProgress(progressInfo, progressCallback, $"Successfully exported 'Users' ({userExported} of {usersResult.Length})");

            #endregion Users

            await SerializeArray("UserApiKeys", _userApiKeySearchService, writer);
        }

        if (manifest.HandleSettings)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ReportProgress(progressInfo, progressCallback, "Exporting 'Settings'");
            await writer.WritePropertyNameAsync("Settings");
            await writer.WriteStartArrayAsync();

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

            await writer.WriteEndArrayAsync();
            progressInfo.ProcessedCount++;
            ReportProgress(progressInfo, progressCallback, "Successfully exported 'Settings'");
        }

        if (manifest.HandleDynamicProperties)
        {
            await SerializeArray("DynamicProperties", _dynamicPropertySearchService, writer);
            await SerializeArray("DynamicPropertyDictionaryItems", _dynamicPropertyDictionaryItemsSearchService, writer);
        }

        await writer.WriteEndObjectAsync();
        await writer.FlushAsync();

        async Task SerializeArray<TModel, TCriteria, TResult>(
            string name,
            ISearchService<TCriteria, TResult, TModel> searchService,
            JsonTextWriter jsonTextWriter)
            where TCriteria : SearchCriteriaBase
            where TResult : GenericSearchResult<TModel>
            where TModel : IEntity
        {
            ReportProgress(progressInfo, progressCallback, $"Exporting '{name}'");

            await jsonTextWriter.WritePropertyNameAsync(name);
            await jsonTextWriter.WriteStartArrayAsync();

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

            await jsonTextWriter.WriteEndArrayAsync();
            await jsonTextWriter.FlushAsync();

            progressInfo.ProcessedCount++;
            ReportProgress(progressInfo, progressCallback, $"Successfully exported '{name}'");
        }
    }

    private async Task ImportModulesInternalAsync(IZipBackupArchive zipArchive, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
    {
        foreach (var moduleInfo in manifest.Modules)
        {
            var moduleDescriptor = GetModulesWithImportSupport().FirstOrDefault(x => x.Id == moduleInfo.Id);
            if (moduleDescriptor != null)
            {
                ReportProgress(progressInfo, progressCallback, $"Importing '{moduleInfo.Id}'");
                var modulePartStream = await zipArchive.OpenEntryAsync(moduleInfo.PartUri.TrimStart('/'));
                if (modulePartStream is null)
                {
                    ReportProgress(progressInfo, progressCallback, $"Module entry '{moduleInfo.PartUri}' missing from backup", ProgressMessageLevel.Error);
                    continue;
                }
                await using (modulePartStream)
                {
                    void ModuleProgressCallback(ExportImportProgressInfo x)
                    {
                        progressInfo.Description = $"{moduleInfo.Id}: {x.Description}";
                        var newErrors = AppendNewErrors(progressInfo.Errors, x.Errors);
                        // Surface each new error as an Error-level entry in the progress log so the
                        // timeline parser can attach it to the current module item and flip its status.
                        progressInfo.ProgressLog = newErrors
                            .Select(e => new ProgressMessage { Level = ProgressMessageLevel.Error, Message = e })
                            .ToList<ProgressMessage>();
                        try
                        {
                            progressCallback(progressInfo);
                        }
                        finally
                        {
                            progressInfo.ProgressLog = new List<ProgressMessage>();
                        }
                    }
                    if (moduleDescriptor.ModuleInstance is IImportSupport importer)
                    {
                        var errorsBefore = progressInfo.Errors.Count;
                        try
                        {
                            //TODO: Add JsonConverter which will be materialized concrete ExportImport option type
                            var options = manifest.Options
                                .DefaultIfEmpty(new ExportImportOptions { HandleBinaryData = manifest.HandleBinaryData, ModuleIdentity = new ModuleIdentity(moduleDescriptor.Identity.Id, moduleDescriptor.Identity.Version, false) })
                                .FirstOrDefault(x => x.ModuleIdentity.Id == moduleDescriptor.Identity.Id);
                            await importer.ImportAsync(modulePartStream, options, ModuleProgressCallback, cancellationToken);
                            progressInfo.ProcessedCount++;
                            var newErrors = progressInfo.Errors.Count - errorsBefore;
                            if (newErrors > 0)
                            {
                                // The module reported errors via its callback but didn't throw —
                                // close out the timeline item with an Error-level summary so the UI shows it red.
                                ReportProgress(progressInfo, progressCallback, $"Imported '{moduleInfo.Id}' with {newErrors} error(s)", ProgressMessageLevel.Error);
                            }
                            else
                            {
                                ReportProgress(progressInfo, progressCallback, $"Successfully imported '{moduleInfo.Id}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            progressInfo.ProcessedCount++;
                            ReportProgress(progressInfo, progressCallback, $"Failed to import '{moduleInfo.Id}': {ex.Message}", ProgressMessageLevel.Error);
                        }
                    }
                }
            }
        }
    }

    private async Task ExportModulesInternalAsync(IZipBackupArchive zipArchive, PlatformExportManifest manifest, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
    {
        foreach (var module in manifest.Modules)
        {
            var moduleDescriptor = GetModulesWithImportSupport().FirstOrDefault(x => x.Id == module.Id);
            if (moduleDescriptor != null)
            {
                //Create part for module
                var moduleZipEntryName = module.Id + ".json";

                void ModuleProgressCallback(ExportImportProgressInfo x)
                {
                    progressInfo.Description = $"{module.Id}: {x.Description}";
                    var newErrors = AppendNewErrors(progressInfo.Errors, x.Errors);
                    progressInfo.ProgressLog = newErrors
                        .Select(e => new ProgressMessage { Level = ProgressMessageLevel.Error, Message = e })
                        .ToList();
                    try
                    {
                        progressCallback(progressInfo);
                    }
                    finally
                    {
                        progressInfo.ProgressLog = new List<ProgressMessage>();
                    }
                }

                ReportProgress(progressInfo, progressCallback, $"Exporting '{module.Id}'");
                if (moduleDescriptor.ModuleInstance is IExportSupport exporter)
                {
                    var errorsBefore = progressInfo.Errors.Count;
                    try
                    {
                        //TODO: Add JsonConverter which will be materialized concrete ExportImport option type
                        //ToDo: Added check ExportImportOptions for modules (DefaultIfEmpty)
                        var options = manifest.Options
                            .DefaultIfEmpty(new ExportImportOptions { HandleBinaryData = manifest.HandleBinaryData, ModuleIdentity = new ModuleIdentity(module.Id, SemanticVersion.Parse(module.Version.Trim()), module.Optional) })
                            .FirstOrDefault(x => x.ModuleIdentity.Id == moduleDescriptor.Identity.Id);

                        await using (var stream = await zipArchive.CreateEntryAsync(moduleZipEntryName))
                        {
                            await exporter.ExportAsync(stream, options, ModuleProgressCallback,
                                cancellationToken);
                        }
                        progressInfo.ProcessedCount++;
                        var newErrors = progressInfo.Errors.Count - errorsBefore;
                        if (newErrors > 0)
                        {
                            ReportProgress(progressInfo, progressCallback, $"Exported '{module.Id}' with {newErrors} error(s)", ProgressMessageLevel.Error);
                        }
                        else
                        {
                            ReportProgress(progressInfo, progressCallback, $"Successfully exported '{module.Id}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        progressInfo.ProcessedCount++;
                        ReportProgress(progressInfo, progressCallback, $"Failed to export '{module.Id}': {ex.Message}", ProgressMessageLevel.Error);
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
