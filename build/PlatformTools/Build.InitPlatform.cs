using System;
using System.Collections.Generic;
using System.Text;
using Nuke.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Microsoft.Extensions;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nuke.Common.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

partial class Build: NukeBuild
{
    [Parameter("Modules discovery path")] public static string DiscoveryPath;
    [Parameter("Probing path")] public static string ProbingPath = RootDirectory / "app_data" / "modules";
    [Parameter("appsettings.json path")] public static string AppsettingsPath = RootDirectory / "appsettings.json";

    Target InitPlatform => _ => _
    .Executes(() =>
    {
        var builder = new ConfigurationBuilder().SetBasePath(RootDirectory).AddJsonFile(AppsettingsPath);
        IConfiguration configuration = builder.Build();
        var moduleCatalogOptions = new LocalStorageModuleCatalogOptions()
        {
            DiscoveryPath = string.IsNullOrEmpty(DiscoveryPath) ? GetModulesDiscoveryPath(configuration) : DiscoveryPath,
            ProbingPath = ProbingPath
        };
        var options = Microsoft.Extensions.Options.Options.Create<LocalStorageModuleCatalogOptions>(moduleCatalogOptions);
        var logger = new LoggerFactory().CreateLogger<LocalStorageModuleCatalog>();
        var moduleCatalog = new LocalStorageModuleCatalog(options, logger);
        moduleCatalog.Load();
    });

    private string GetModulesDiscoveryPath(IConfiguration configuration)
    {
        var virtoSection = configuration.GetSection("VirtoCommerce");
        var result = virtoSection.GetValue<string>("DiscoveryPath", "./modules");
        return result;
    }
}
