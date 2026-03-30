using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
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
    public static void Install(string zipPath, string targetModulePath)
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

        var logger = ModuleLogger.CreateLogger(typeof(ModulePackageInstaller));
        logger.LogInformation("Extracting {FileName} to {TargetPath}", Path.GetFileName(zipPath), targetModulePath);
        ZipFile.ExtractToDirectory(zipPath, targetModulePath, overwriteFiles: true);
        logger.LogInformation("Successfully installed to {TargetPath}", targetModulePath);
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

        var logger = ModuleLogger.CreateLogger(typeof(ModulePackageInstaller));
        logger.LogInformation("Removing module directory: {ModulePath}", modulePath);

        try
        {
            Directory.Delete(modulePath, recursive: true);
            logger.LogInformation("Successfully removed directory {ModulePath}", modulePath);
        }
        catch (IOException ex)
        {
            // Directory may be locked by FileSystemWatcher (PhysicalFileProvider) or antivirus.
            // Log warning and continue — the new module version will overwrite files via Install().
            logger.LogWarning("Could not fully delete directory {ModulePath}: {Message}. Continuing with installation.", modulePath, ex.Message);
        }
    }

    /// <summary>
    /// Download a module package from a URL to a local file.
    /// Replaces <c>IExternalModulesClient.OpenRead()</c> with a static method.
    /// </summary>
    public static void Download(Uri packageUrl, string targetZipPath, HttpClient httpClient, ExternalModuleCatalogOptions options = null)
    {
        ArgumentNullException.ThrowIfNull(packageUrl);
        ArgumentNullException.ThrowIfNull(targetZipPath);
        ArgumentNullException.ThrowIfNull(httpClient);

        var logger = ModuleLogger.CreateLogger(typeof(ModulePackageInstaller));
        logger.LogInformation("Downloading {PackageUrl}", packageUrl);

        var request = CreateHttpRequest(HttpMethod.Get, packageUrl, options);
        using var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var responseStream = response.Content.ReadAsStream();
        using var fileStream = File.Create(targetZipPath);
        responseStream.CopyTo(fileStream);

        logger.LogInformation("Downloaded to {TargetZipPath}", targetZipPath);
    }

    /// <summary>
    /// Download and parse external module manifests from all configured URLs.
    /// Collects modules from main manifest URL + extra URLs, deduplicates.
    /// </summary>
    public static IList<ManifestModuleInfo> LoadExternalModules(HttpClient httpClient, ExternalModuleCatalogOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        if (options.ModulesManifestUrl == null)
        {
            return [];
        }

        var result = LoadModulesManifest(httpClient, options, options.ModulesManifestUrl);

        if (!options.ExtraModulesManifestUrls.IsNullOrEmpty())
        {
            foreach (var extraUrl in options.ExtraModulesManifestUrls)
            {
                result.AddRange(LoadModulesManifest(httpClient, options, extraUrl));
            }

            result = result.Distinct().ToList();
        }

        return result;
    }

    /// <summary>
    /// Download and parse a single external module manifest from a URL.
    /// </summary>
    public static IList<ManifestModuleInfo> LoadModulesManifest(HttpClient httpClient, ExternalModuleCatalogOptions options, Uri manifestUrl)
    {
        ArgumentNullException.ThrowIfNull(manifestUrl);

        var logger = ModuleLogger.CreateLogger(typeof(ModulePackageInstaller));
        logger.LogDebug("Downloading module manifest from {Url}", manifestUrl);

        var request = CreateHttpRequest(HttpMethod.Get, manifestUrl, options);
        using var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var stream = response.Content.ReadAsStream();
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        return ModuleDiscovery.ParseExternalManifest(json, PlatformVersion.CurrentVersion, options.IncludePrerelease);
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
