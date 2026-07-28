using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.MySql;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.SqlServer;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.MigrationScripter
{
    /// <summary>
    /// Companion tool that grabs the EF Core migrations startup would apply (platform + security + every
    /// installed module) and writes them to .sql files, without applying anything or starting a web server.
    /// <para>
    /// It replicates the platform's module-loading bootstrap, registers the platform + security DbContexts,
    /// runs each module's <c>IModule.Initialize</c> (where module <c>AddDbContext</c> lives) into a service
    /// collection, then scripts every registered DbContext. It depends only on the platform data / security /
    /// modules assemblies (not Platform.Web), which is enough to keep those assemblies in the process's Trusted
    /// Platform Assemblies set so modules' bundled copies load by name.
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
                var environment = ResolveEnvironmentName();
                var configuration = BuildConfiguration(platformPath, environment);

                LoadModules(platformPath, configuration, environment, loggerFactory, logger);

                var serviceProvider = BuildServiceProvider(configuration, environment, platformPath, loggerFactory, logger);

                var generator = new MigrationScriptGenerator(serviceProvider, ModuleBootstrapper.Instance, logger);
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

        private static string ResolveEnvironmentName()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return string.IsNullOrEmpty(environment) ? Environments.Production : environment;
        }

        private static IConfigurationRoot BuildConfiguration(string platformPath, string environment)
        {
            return new ConfigurationBuilder()
                .SetBasePath(platformPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Replicates the platform's module-loading bootstrap using public APIs: discover, validate, copy, load.
        /// </summary>
        private static void LoadModules(string platformPath, IConfiguration configuration, string environment, ILoggerFactory loggerFactory, ILogger logger)
        {
            var platformWebDll = Path.Combine(platformPath, "VirtoCommerce.Platform.Web.dll");
            var versionSource = File.Exists(platformWebDll) ? platformWebDll : typeof(ModuleBootstrapper).Assembly.Location;

            PlatformVersion.CurrentVersion = SemanticVersion.Parse(
                FileVersionInfo.GetVersionInfo(versionSource).ProductVersion);

            var moduleOptions = configuration.GetSection("VirtoCommerce").Get<LocalStorageModuleCatalogOptions>()
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

        /// <summary>
        /// Builds a service collection with the platform + security DbContexts (mirroring the web host's
        /// registrations) and every module's own registrations, then builds the provider.
        /// </summary>
        private static IServiceProvider BuildServiceProvider(
            IConfiguration configuration, string environment, string platformPath, ILoggerFactory loggerFactory, ILogger logger)
        {
            var services = new ServiceCollection();

            services.AddSingleton(configuration);
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            var databaseProvider = configuration.GetValue("DatabaseProvider", "SqlServer");

            services.AddDbContext<PlatformDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("VirtoCommerce");
                UseDatabase(options, databaseProvider, connectionString, configuration);
            });

            services.AddDbContext<SecurityDbContext>(options =>
            {
                var connectionString = configuration["Auth:ConnectionString"] ?? configuration.GetConnectionString("VirtoCommerce");
                UseDatabase(options, databaseProvider, connectionString, configuration);
                options.UseOpenIddict<VirtoOpenIddictEntityFrameworkCoreApplication,
                                      VirtoOpenIddictEntityFrameworkCoreAuthorization,
                                      VirtoOpenIddictEntityFrameworkCoreScope,
                                      VirtoOpenIddictEntityFrameworkCoreToken,
                                      string>();
            });

            // Base platform services (lives in Platform.Data, not Platform.Web) — improves the chance that
            // module DbContexts with extra constructor dependencies can be resolved. Best-effort.
            try
            {
                services.AddPlatformServices(configuration);
            }
            catch (Exception ex)
            {
                logger.LogWarning("AddPlatformServices skipped: {Message}", ex.Message);
            }

            var hostEnvironment = new SimpleHostEnvironment
            {
                EnvironmentName = environment,
                ApplicationName = "VirtoCommerce.Platform.MigrationScripter",
                ContentRootPath = platformPath,
                ContentRootFileProvider = new PhysicalFileProvider(platformPath),
            };

            logger.LogInformation("Initializing modules to collect DbContext registrations...");
            ModuleBootstrapper.Instance.RunConfigureServices(services, configuration);
            ModuleBootstrapper.Instance.InitializeModules(services, configuration, hostEnvironment);

            return services.BuildServiceProvider();
        }

        private static void UseDatabase(DbContextOptionsBuilder options, string databaseProvider, string connectionString, IConfiguration configuration)
        {
            switch (databaseProvider)
            {
                case "MySql":
                    options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), configuration);
                    break;
                case "PostgreSql":
                    options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), configuration);
                    break;
                default:
                    options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), configuration);
                    break;
            }
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
