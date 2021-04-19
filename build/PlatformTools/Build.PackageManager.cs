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
    Target Init => _ => _
    .Executes(async () =>
    {
        var platformRelease = await GithubManager.GetPlatformRelease(PlatformVersion);
        var packageManifest = PackageManager.CreatePackageManifest(platformRelease.TagName, platformRelease.Assets.First().BrowserDownloadUrl);
        PackageManager.ToFile(packageManifest, PackageManifestPath);
    });

    Target Install => _ => _
    .Triggers(InstallModules)
    .Executes(async () => {
        var packageManifest = PackageManager.FromFile(PackageManifestPath);
        if(Module.Length > 0)
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
                    var localModuleCatalog = LocalModuleCatalog.GetCatalog(GetDiscoveryPath(), ProbingPath); 
                    var externalCatalog = ExtModuleCatalog.GetCatalog(GitHubToken, localModuleCatalog, packageManifest.ModuleSources);
                    var moduleInfo = externalCatalog.Items.OfType<ManifestModuleInfo>().Where(m => m.Id == moduleId).Where(m => m.Ref.Contains("github.com")).First();
                    var ownerRepo = GithubManager.GetRepoFromUrl(moduleInfo.Ref);
                    Octokit.Release moduleRelease = await GithubManager.GetModuleRelease(ownerRepo.Item2, null);
                    moduleVersion = moduleRelease.TagName;
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
        PackageManager.ToFile(packageManifest);
    });

    Target InstallPlatform => _ => _
    .Executes(async () =>
    {
        var packageManifest = PackageManager.FromFile(PackageManifestPath);
        var platformZip = TemporaryDirectory / "platform.zip";
        if (NeedToInstallPlatform(packageManifest.PlatformVersion))
        {
            await HttpTasks.HttpDownloadFileAsync(packageManifest.PlatformAssetUrl, platformZip);
            CompressionTasks.Uncompress(platformZip, RootDirectory);
        }
    });

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
            // Get link to certain release
            var externalModule = externalModuleCatalog.Modules.Where(m => m.ModuleName == moduleInstall.Id).First();
            var (githubUser, repoName) = GithubManager.GetRepoFromUrl(externalModule.Ref);
            // Download and unzip
            var githubRelease = await GithubManager.GetModuleRelease(repoName, moduleInstall.Version);
            var releaseAsset = githubRelease.Assets.First();
            var currentModule = modules.First(m => m.ModuleName == moduleInstall.Id).Ref = releaseAsset.BrowserDownloadUrl; ;
        }
        var progress = new Progress<ProgressMessage>(m => Logger.Info(m.Message));
        var moduleManifests = externalModuleCatalog.CompleteListWithDependencies(modules).OfType<ManifestModuleInfo>();
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
}
