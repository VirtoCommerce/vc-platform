using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Static module installation and uninstallation operations.
/// Handles ZIP extraction and directory management without DI.
/// </summary>
public static class ModulePackageInstaller
{
    /// <summary>
    /// Install a module from a ZIP file to the target module directory.
    /// </summary>
    public static void Install(string zipPath, string targetModulePath, bool deleteZip = true)
    {
        ArgumentNullException.ThrowIfNull(zipPath);
        ArgumentNullException.ThrowIfNull(targetModulePath);

        if (!File.Exists(zipPath))
        {
            throw new FileNotFoundException($"Module package not found: {zipPath}");
        }

        if (!Directory.Exists(targetModulePath))
        {
            Directory.CreateDirectory(targetModulePath);
        }

        ZipFile.ExtractToDirectory(zipPath, targetModulePath, overwriteFiles: true);

        if (deleteZip)
        {
            File.Delete(zipPath);
        }
    }

    /// <summary>
    /// Uninstall a module by deleting its directory.
    /// If deletion fails (e.g., directory locked by FileSystemWatcher), logs a warning and continues.
    /// </summary>
    public static void Uninstall(string modulePath)
    {
        ArgumentNullException.ThrowIfNull(modulePath);

        if (!Directory.Exists(modulePath))
        {
            return;
        }

        try
        {
            Directory.Delete(modulePath, recursive: true);
        }
        catch (IOException)
        {
            // Directory may be locked by FileSystemWatcher (PhysicalFileProvider) or antivirus.
            // Continue — the new module version will overwrite files via Install().
        }
    }

    /// <summary>
    /// Download and parse external module manifests from all configured URLs.
    /// Collects modules from main manifest URL + extra URLs, deduplicates.
    /// </summary>
    public static IList<ManifestModuleInfo> LoadExternalModules(ExternalModuleCatalogOptions options, SemanticVersion platformVersion, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);

        if (options.ModulesManifestUrl is null)
        {
            return [];
        }

        var result = LoadModulesManifest(options.ModulesManifestUrl, options, platformVersion, httpClient);

        if (options.ExtraModulesManifestUrls?.Length > 0)
        {
            foreach (var extraUrl in options.ExtraModulesManifestUrls)
            {
                result.AddRange(LoadModulesManifest(extraUrl, options, platformVersion, httpClient));
            }

            result = result.Distinct().ToList();
        }

        return result;
    }

    /// <summary>
    /// Download and parse a single external module manifest from a URL.
    /// </summary>
    public static IList<ManifestModuleInfo> LoadModulesManifest(Uri manifestUrl, ExternalModuleCatalogOptions options, SemanticVersion platformVersion, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(manifestUrl);

        var request = CreateHttpRequest(HttpMethod.Get, manifestUrl, options);
        using var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var stream = response.Content.ReadAsStream();
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        return ModuleBootstrapper.Instance.ParseExternalManifest(json, platformVersion, options.IncludePrerelease);
    }

    /// <summary>
    /// Validate modules, then install or update them in a single transaction.
    /// Returns true on success, false if validation failed or an error occurred (with rollback).
    /// </summary>
    public static bool InstallModules(
        IList<ManifestModuleInfo> modules,
        IList<ManifestModuleInfo> allModules,
        string discoveryPath,
        ExternalModuleCatalogOptions options,
        HttpClient httpClient,
        IProgress<ProgressMessage> progress)
    {
        var installedModuleIds = new HashSet<string>(allModules.Where(x => x.IsInstalled).Select(x => x.Id), StringComparer.OrdinalIgnoreCase);
        var modulesToUpdate = modules.Where(x => installedModuleIds.Contains(x.Id)).ToList();
        var modulesToAdd = modules.Where(x => !installedModuleIds.Contains(x.Id)).ToList();

        var changedModules = new List<ManifestModuleInfo>();
        using var scope = new TransactionScope(TransactionScopeOption.Required, TransactionManager.MaximumTimeout);

        try
        {
            foreach (var module in modulesToAdd)
            {
                Report(progress, ProgressMessageLevel.Info, "Installing '{0}'", module);
                DownloadAndInstall(module, discoveryPath, options, httpClient, progress);

                module.IsInstalled = true;
                changedModules.Add(module);
            }

            foreach (var module in modulesToUpdate)
            {
                var existingModule = allModules.First(x => x.IsInstalled && x.Id.EqualsIgnoreCase(module.Id));

                Report(progress, ProgressMessageLevel.Info, "Updating '{0}' -> '{1}'", existingModule, module);
                Uninstall(Path.Combine(discoveryPath, existingModule.Id));
                DownloadAndInstall(module, discoveryPath, options, httpClient, progress);

                module.IsInstalled = true;
                changedModules.Add(module);

                existingModule.IsInstalled = false;
                changedModules.Add(existingModule);
            }

            scope.Complete();
            return true;
        }
        catch (Exception ex)
        {
            Report(progress, ProgressMessageLevel.Error, ex.ToString());
            Report(progress, ProgressMessageLevel.Error, "Rollback all changes...");

            foreach (var changedModule in changedModules)
            {
                changedModule.IsInstalled = !changedModule.IsInstalled;
            }

            return false;
        }
    }

    private static void DownloadAndInstall(
        ManifestModuleInfo module,
        string discoveryPath,
        ExternalModuleCatalogOptions options,
        HttpClient httpClient,
        IProgress<ProgressMessage> progress)
    {
        var modulePath = Path.Combine(discoveryPath, module.Id);

        if (!Directory.Exists(modulePath))
        {
            Directory.CreateDirectory(modulePath);
        }

        if (Uri.IsWellFormedUriString(module.Ref, UriKind.Absolute))
        {
            Report(progress, ProgressMessageLevel.Info, "Downloading '{0}'", module.Ref);

            var zipUrl = new Uri(module.Ref);
            var zipPath = Path.Combine(modulePath, $"{module.Id}_{module.Version}.zip");

            Download(zipUrl, zipPath, options, httpClient);
            Install(zipPath, modulePath);
        }
        else if (File.Exists(module.Ref))
        {
            Install(module.Ref, modulePath, deleteZip: false);
        }
        else
        {
            throw new FileNotFoundException($"Module package not found: {module.Ref}");
        }

        Report(progress, ProgressMessageLevel.Info, "Successfully installed '{0}'.", module);
    }

    private static void Download(Uri zipUrl, string zipPath, ExternalModuleCatalogOptions options, HttpClient httpClient)
    {
        using var request = CreateHttpRequest(HttpMethod.Get, zipUrl, options);
        using var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var responseStream = response.Content.ReadAsStream();
        using var fileStream = File.Create(zipPath);
        responseStream.CopyTo(fileStream);
    }

    private static void Report(IProgress<ProgressMessage> progress, ProgressMessageLevel level, string format, params object[] args)
    {
        progress?.Report(new ProgressMessage { Level = level, Message = string.Format(CultureInfo.CurrentCulture, format, args) });
    }

    private static HttpRequestMessage CreateHttpRequest(HttpMethod method, Uri url, ExternalModuleCatalogOptions options)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Add("User-Agent", "Virto Commerce Manager");

        if (options != null)
        {
            if (!string.IsNullOrEmpty(options.AuthorizationToken))
            {
                request.Headers.Add("Accept", "application/octet-stream");
                request.Headers.Add("Authorization", "Token " + options.AuthorizationToken);
            }

            if (!string.IsNullOrEmpty(options.AuthorizationSchema) &&
                !string.IsNullOrEmpty(options.AuthorizationParameter))
            {
                request.Headers.Add("Authorization", $"{options.AuthorizationSchema} {options.AuthorizationParameter}");
            }
        }

        return request;
    }
}
