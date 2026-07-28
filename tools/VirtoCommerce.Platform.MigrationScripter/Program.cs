using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;

namespace VirtoCommerce.Platform.MigrationScripter
{
    /// <summary>
    /// Companion tool that reuses the real platform host to grab the EF Core migrations that startup would
    /// apply (platform + security + every installed module), without applying anything or starting a web server.
    /// <para>
    /// It replicates the platform's module-loading bootstrap, then builds the real host up to — but not past —
    /// <c>Build()</c>. Building runs <c>Startup.ConfigureServices</c>, which registers the platform and security
    /// DbContexts and calls each module's <c>IModule.Initialize</c> (where module <c>AddDbContext</c> lives).
    /// <c>Configure</c> (the migration apply path) and Kestrel never run, so nothing is applied.
    /// </para>
    /// <para>
    /// Referencing <c>Platform.Web</c> is required so the platform assemblies are in this process's Trusted
    /// Platform Assemblies set; the module loader then loads them by name (like the running platform) instead of
    /// each module's bundled copy, avoiding "assembly already loaded" conflicts.
    /// </para>
    /// </summary>
    public static class Program
    {
        public static int Main(string[] args)
        {
            var options = MigrationScriptOptions.Parse(args);
            if (options.ShowHelp)
            {
                PrintHelp();
                return 0;
            }

            var platformPath = Path.GetFullPath(
                string.IsNullOrEmpty(options.PlatformPath) ? Directory.GetCurrentDirectory() : options.PlatformPath);

            if (!Directory.Exists(platformPath))
            {
                Console.Error.WriteLine($"Platform path not found: {platformPath}");
                return 1;
            }

            // Align current directory with the platform folder so appsettings.json, ./modules and ./app_data
            // resolve exactly as they would for a real start.
            Directory.SetCurrentDirectory(platformPath);

            var outputPath = Path.GetFullPath(
                string.IsNullOrEmpty(options.OutputPath) ? Path.Combine(platformPath, "migration-scripts") : options.OutputPath);

            using var loggerFactory = LoggerFactory.Create(builder =>
                builder.SetMinimumLevel(LogLevel.Information)
                       .AddSimpleConsole(o => o.SingleLine = true));

            var logger = loggerFactory.CreateLogger("MigrationScripter");

            try
            {
                LoadModules(platformPath, loggerFactory, logger);

                logger.LogInformation("Building platform host (no web server will start, no migrations will be applied)...");
                using var host = Web.Program.CreateHostBuilder([]).Build();

                var generator = new MigrationScriptGenerator(host.Services, ModuleBootstrapper.Instance, logger);
                generator.Generate(outputPath, options.IncludeEmpty);

                logger.LogInformation("Done. Migration scripts written to {OutputPath}", outputPath);
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Migration script generation failed.");
                return 1;
            }
        }

        /// <summary>
        /// Replicates <c>VirtoCommerce.Platform.Web.Program.LoadModules()</c> using public APIs so that
        /// <see cref="ModuleBootstrapper.Instance"/> is populated (discovered, validated, copied, loaded)
        /// before the host is built. The platform version is taken from the deployed Platform.Web assembly so
        /// modules validate against the target platform's version.
        /// </summary>
        private static void LoadModules(string platformPath, ILoggerFactory loggerFactory, ILogger logger)
        {
            var platformWebDll = Path.Combine(platformPath, "VirtoCommerce.Platform.Web.dll");
            var versionSource = File.Exists(platformWebDll) ? platformWebDll : typeof(Web.Startup).Assembly.Location;

            PlatformVersion.CurrentVersion = SemanticVersion.Parse(
                FileVersionInfo.GetVersionInfo(versionSource).ProductVersion);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environment))
            {
                environment = Environments.Production;
            }

            var bootConfig = new ConfigurationBuilder()
                .SetBasePath(platformPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var moduleOptions = bootConfig.GetSection("VirtoCommerce").Get<LocalStorageModuleCatalogOptions>()
                ?? new LocalStorageModuleCatalogOptions();
            moduleOptions.DiscoveryPath = Path.GetFullPath(moduleOptions.DiscoveryPath);
            moduleOptions.ProbingPath = Path.GetFullPath(moduleOptions.ProbingPath);

            var isDevelopment = string.Equals(environment, Environments.Development, StringComparison.OrdinalIgnoreCase);

            logger.LogInformation(
                "Platform {Version}. Loading modules from {DiscoveryPath} (probing {ProbingPath})...",
                PlatformVersion.CurrentVersion, moduleOptions.DiscoveryPath, moduleOptions.ProbingPath);

            ModuleBootstrapper.Instance = new ModuleBootstrapper(loggerFactory, moduleOptions)
                .Discover()
                .Validate(PlatformVersion.CurrentVersion)
                .Copy(RuntimeInformation.ProcessArchitecture)
                .Load(isDevelopment);
        }

        private static void PrintHelp()
        {
            Console.WriteLine(
@"VirtoCommerce Platform Migration Scripter

Grabs the EF Core migrations that platform startup would apply (platform + security + all installed
modules) and writes them to .sql files, WITHOUT applying anything or starting a web server.

Usage:
  VirtoCommerce.Platform.MigrationScripter [options]

Options:
  -p, --platform-path <path>   Deployed platform folder (appsettings.json, ./modules, ./app_data).
                               Defaults to the current directory.
  -o, --output <path>          Output folder for .sql files.
                               Defaults to <platform-path>/migration-scripts.
      --include-empty          Also write files for contexts with no pending migrations.
                               By default, up-to-date contexts are skipped (no empty files).
  -h, --help                   Show this help.

Notes:
  * Provider and connection string are read from the platform's appsettings.json.
    Override without editing files via environment variables:
      DatabaseProvider, ConnectionStrings__VirtoCommerce
  * Pending-only delta scripts are produced per context. If the database is unreachable or
    has no migration history, an idempotent full script is produced instead (with a warning).
  * Build/run this tool from a platform version close to the target folder's version.");
        }
    }
}
