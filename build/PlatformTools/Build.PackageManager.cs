using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using PlatformTools;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;

partial class Build: NukeBuild
{
    [Parameter("Platform version")] public static string PlatformVersion;
    [Parameter("vc-package.json path")] public static string PackageManifestPath = "./vc-package.json";
    [Parameter("Install params (install -module VirtoCommerce.Core:1.2.3)")] public static string[] Module;
    [Parameter("Skip dependency solving")] public static bool SkipDependencySolving = false;
    Target Init => _ => _
    .Executes(async () =>
    {
        var platformRelease = await GithubManager.GetPlatformRelease(GitHubToken, PlatformVersion);
        var packageManifest = PackageManager.CreatePackageManifest(platformRelease.TagName, platformRelease.Assets.First().BrowserDownloadUrl);
        PackageManager.ToFile(packageManifest, PackageManifestPath);
    });

    Target Install => _ => _
    .Triggers(InstallPlatform, InstallModules)
    .Executes(async () => {
        PackageManifest packageManifest;
        if (!File.Exists(PackageManifestPath))
        {
            var platformRelease = await GithubManager.GetPlatformRelease(GitHubToken, PlatformVersion);
            packageManifest = PackageManager.CreatePackageManifest(platformRelease.TagName);
            await InstallPlatformAsync(platformRelease.TagName);
        }
        else
        {
            packageManifest = PackageManager.FromFile(PackageManifestPath);
        }
        var localModuleCatalog = LocalModuleCatalog.GetCatalog(GetDiscoveryPath(), ProbingPath);
        var externalCatalog = ExtModuleCatalog.GetCatalog(GitHubToken, localModuleCatalog, packageManifest.ModuleSources);
        if (Module?.Length > 0)
        {
            foreach(var module in Module)
            {
                string moduleId, moduleVersion;
                if (module.Contains(":"))
                {
                    var splitedModule = module.Split(":");
                    moduleId = splitedModule.First();
                    moduleVersion = splitedModule.Last();
                }
                else
                {
                    moduleId = module;
                    var moduleInfo = externalCatalog.Items.OfType<ManifestModuleInfo>().Where(m => m.Id == moduleId).FirstOrDefault(m => m.Ref.Contains("github.com"));
                    if (moduleInfo == null)
                    {
                        ControlFlow.Fail($"No module {moduleId} found");
                    }
                    moduleVersion = moduleInfo.Version.ToString();
                }
                var moduleItem = new ModuleItem(moduleId, moduleVersion);
                var existedModule = packageManifest.Modules.Where(m => m.Id == moduleItem.Id).FirstOrDefault();
                if(existedModule == null)
                {
                    packageManifest.Modules.Add(moduleItem);
                } else
                {
                    existedModule.Version = moduleItem.Version;
                }
            }
        }
        else
        {
            if (!packageManifest.Modules.Any())
            {
                var commerce = externalCatalog.Modules.OfType<ManifestModuleInfo>().Where(m => m.Groups.Contains("commerce")).Select(m => new ModuleItem(m.ModuleName, m.Version.ToString()));
                packageManifest.Modules.AddRange(commerce);
            }
        }
        PackageManager.ToFile(packageManifest);
    });

    Target InstallPlatform => _ => _
    .Executes(async () =>
    {
        var packageManifest = PackageManager.FromFile(PackageManifestPath);
        if (NeedToInstallPlatform(packageManifest.PlatformVersion))
        {
            await InstallPlatformAsync(packageManifest.PlatformVersion);
        }
    });

    private async Task InstallPlatformAsync(string platformVersion)
    {
        var platformRelease = await GithubManager.GetPlatformRelease(platformVersion);
        var platformAssetUrl = platformRelease.Assets.FirstOrDefault().BrowserDownloadUrl;
        var platformZip = TemporaryDirectory / "platform.zip";
        if (string.IsNullOrEmpty(platformAssetUrl))
        {
            ControlFlow.Fail($"No platform's assets found with tag {platformVersion}");
        }
        await HttpTasks.HttpDownloadFileAsync(platformAssetUrl, platformZip);
        CompressionTasks.Uncompress(platformZip, RootDirectory);
    }

    private string GetDiscoveryPath()
    {
        var configuration = AppSettings.GetConfiguration(RootDirectory, AppsettingsPath);
        return string.IsNullOrEmpty(DiscoveryPath) ? configuration.GetModulesDiscoveryPath() : DiscoveryPath;
    }

    private bool NeedToInstallPlatform(string version)
    {
        bool result = true;
        var platformWeb = RootDirectory / "VirtoCommerce.Platform.Web.dll";
        var newVersion = new Version(version);
        if (File.Exists(platformWeb))
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(platformWeb);
            var currentProductVersion = Version.Parse(versionInfo.ProductVersion);
            if(newVersion <= currentProductVersion)
            {
                result = false;
            }
        }
        return result;
    }
    Target InstallModules => _ => _
    .After(InstallPlatform)
    .Executes(async () => {
        var packageManifest = PackageManager.FromFile(PackageManifestPath);
        var discoveryPath = GetDiscoveryPath();
        var localModuleCatalog = LocalModuleCatalog.GetCatalog(discoveryPath, ProbingPath);
        var externalModuleCatalog = ExtModuleCatalog.GetCatalog(GitHubToken, localModuleCatalog, packageManifest.ModuleSources);
        var moduleInstaller = ModuleInstallerFacade.GetModuleInstaller(discoveryPath, ProbingPath, GitHubToken, packageManifest.ModuleSources);
        var modules = new List<ModuleInfo>();
        modules.AddRange(externalModuleCatalog.Modules.Where(m =>
        {
            var moduleIds = packageManifest.Modules.Select(i => i.Id);
            return moduleIds.Contains(m.ModuleName);
        }));
        foreach (var moduleInstall in packageManifest.Modules)
        {
            var externalModule = externalModuleCatalog.Modules.FirstOrDefault(m => m.ModuleName == moduleInstall.Id);
            if (externalModule == null)
            {
                ControlFlow.Fail($"No module {moduleInstall.Id} found");
            }
            var (githubUser, repoName) = GithubManager.GetRepoFromUrl(externalModule.Ref);
            var githubRelease = await GithubManager.GetModuleRelease(GitHubToken, repoName, moduleInstall.Version);
            var releaseAsset = githubRelease.Assets.First();
            var currentModule = modules.First(m => m.ModuleName == moduleInstall.Id);
            currentModule.Ref = releaseAsset.BrowserDownloadUrl;
        }
        var progress = new Progress<ProgressMessage>(m => Logger.Info(m.Message));
        if (!SkipDependencySolving)
        {
            modules = externalModuleCatalog.CompleteListWithDependencies(modules).ToList();
        }
        var moduleManifests = modules.OfType<ManifestModuleInfo>();
        moduleInstaller.Install(moduleManifests, progress);
        localModuleCatalog.Reload();
    });

    Target Uninstall => _ => _
    .Executes(() =>
    {
        var discoveryPath = GetDiscoveryPath();
        var packageManifest = PackageManager.FromFile(PackageManifestPath);
        var localModulesCatalog = LocalModuleCatalog.GetCatalog(discoveryPath, ProbingPath);
        var externalModuleCatalog = ExtModuleCatalog.GetCatalog(GitHubToken, localModulesCatalog, packageManifest.ModuleSources);
        FileSystemTasks.DeleteDirectory(ProbingPath);
        Module.ForEach(m => FileSystemTasks.DeleteDirectory(Path.Combine(discoveryPath, m)));
        packageManifest.Modules.RemoveAll(m => Module.Contains(m.Id));
        PackageManager.ToFile(packageManifest);
        localModulesCatalog.Load();
    });

    Target Update => _ => _
    .Triggers(InstallPlatform, InstallModules)
    .Executes(async () =>
    {
        var packageManifest = PackageManager.FromFile(PackageManifestPath);
        var platformRelease = await GithubManager.GetPlatformRelease(GitHubToken, PlatformVersion);
        packageManifest.PlatformVersion = platformRelease.TagName;
        packageManifest.PlatformAssetUrl = platformRelease.Assets.First().BrowserDownloadUrl;
        foreach(var module in packageManifest.Modules)
        {
            var localModuleCatalog = LocalModuleCatalog.GetCatalog(GetDiscoveryPath(), ProbingPath);
            var externalCatalog = ExtModuleCatalog.GetCatalog(GitHubToken, localModuleCatalog, packageManifest.ModuleSources);
            var moduleInfo = externalCatalog.Items.OfType<ManifestModuleInfo>().Where(m => m.Id == module.Id).FirstOrDefault(m => m.Ref.Contains("github.com"));
            if(moduleInfo == null)
            {
                ControlFlow.Fail($"No module {module.Id} found");
            }
            var ownerRepo = GithubManager.GetRepoFromUrl(moduleInfo.Ref);
            var moduleRelease = await GithubManager.GetModuleRelease(GitHubToken, ownerRepo.Item2, null);
            module.Version = moduleRelease.TagName;
        }
        PackageManager.ToFile(packageManifest);
    });
}
