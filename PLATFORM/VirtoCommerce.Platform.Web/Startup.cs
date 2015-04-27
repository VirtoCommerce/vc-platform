using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using NuGet;
using Owin;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web;
using WebGrease.Extensions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Asset;
using VirtoCommerce.Platform.Data.Packaging;
using VirtoCommerce.Platform.Data.Packaging.Repositories;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Caching;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Data.Repositories;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Notification;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Identity;

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string modulesVirtualPath = "~/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath).EnsureEndSeparator();
            var assembliesPath = HostingEnvironment.MapPath("~/App_data/Modules");

            var bootstraper = new VirtoCommercePlatformWebBootstraper(modulesVirtualPath, modulesPhysicalPath, assembliesPath);
            bootstraper.Run();

            var container = bootstraper.Container;
            container.RegisterInstance(app);

            var moduleCatalog = container.Resolve<IModuleCatalog>();

            // Register URL rewriter before modules initialization
            if (Directory.Exists(modulesPhysicalPath))
            {
                var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.EnsureEndSeparator();
                var modulesRelativePath = MakeRelativePath(applicationBase, modulesPhysicalPath);

                var urlRewriterOptions = new UrlRewriterOptions();
                var moduleInitializerOptions = (ModuleInitializerOptions)container.Resolve<IModuleInitializerOptions>();
                moduleInitializerOptions.SampleDataLevel = EnumUtility.SafeParse(ConfigurationManager.AppSettings["VirtoCommerce:SampleDataLevel"], SampleDataLevel.None);

                foreach (var module in moduleCatalog.Modules.OfType<ManifestModuleInfo>())
                {
                    var urlRewriteKey = string.Format(CultureInfo.InvariantCulture, "/Modules/$({0})", module.ModuleName);
                    var urlRewriteValue = MakeRelativePath(modulesPhysicalPath, module.FullPhysicalPath);
                    urlRewriterOptions.Items.Add(PathString.FromUriComponent(urlRewriteKey), "/" + urlRewriteValue);

                    moduleInitializerOptions.ModuleDirectories.Add(module.ModuleName, module.FullPhysicalPath);
                }

                app.Use<UrlRewriterOwinMiddleware>(urlRewriterOptions);
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem(modulesRelativePath)
                });
            }

            //Initialize Platform dependencies
            const string connectionStringName = "VirtoCommerce";
            InitializePlatform(container, connectionStringName);

            // Ensure all modules are loaded
            var moduleManager = container.Resolve<IModuleManager>();

            foreach (var module in moduleCatalog.Modules.Where(x => x.State == ModuleState.NotStarted))
            {
                moduleManager.LoadModule(module.ModuleName);
            }

            // Post-initialize
            OwinConfig.Configure(app, container, connectionStringName);

            var postInitializeModules = moduleCatalog.CompleteListWithDependencies(moduleCatalog.Modules)
                .Select(m => m.ModuleInstance)
                .ToArray();

            foreach (var module in postInitializeModules)
            {
                module.PostInitialize();
            }

            app.MapSignalR();
        }


        private static void InitializePlatform(IUnityContainer container, string connectionStringName)
        {
            #region Setup database

            using (var db = new SecurityDbContext(connectionStringName))
            {
                new IdentityDatabaseInitializer().InitializeDatabase(db);
            }

            using (var context = new PlatformRepository(connectionStringName, new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor()))
            {
                new PlatformDatabaseInitializer().InitializeDatabase(context);
            }

            // Create Hangfire tables
            new SqlServerStorage(connectionStringName);

            #endregion

            Func<IPlatformRepository> platformRepositoryFactory = () => new PlatformRepository(connectionStringName, new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor());
            container.RegisterType<IPlatformRepository>(new InjectionFactory(c => platformRepositoryFactory()));
            var manifestProvider = container.Resolve<IModuleManifestProvider>();

            #region Caching

            var cacheProvider = new HttpCacheProvider();
            container.RegisterInstance<ICacheProvider>(cacheProvider);

            var cacheSettings = new[] 
			{
				new CacheSettings(CacheGroups.Settings, TimeSpan.FromDays(1)),
				new CacheSettings(CacheGroups.Security, TimeSpan.FromMinutes(1)),
			};

            var cacheManager = new CacheManager(x => cacheProvider, group => cacheSettings.FirstOrDefault(s => s.Group == group));

            #endregion

            #region Notifications
            var hubSignalR = GlobalHost.ConnectionManager.GetHubContext<ClientPushHub>();
            var notifier = new InMemoryNotifierImpl(hubSignalR);
            container.RegisterInstance<INotifier>(notifier);

            #endregion

            #region Assets

            var assetsConnection = ConfigurationManager.ConnectionStrings["AssetsConnectionString"];

            if (assetsConnection != null)
            {
                var properties = assetsConnection.ConnectionString.ToDictionary(";", "=");
                var provider = properties["provider"];
                var assetsConnectionString = properties.ToString(";", "=", "provider");

                if (string.Equals(provider, FileSystemBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    var fileSystemBlobProvider = new FileSystemBlobProvider(assetsConnectionString);

                    container.RegisterInstance<IBlobStorageProvider>(fileSystemBlobProvider);
                    container.RegisterInstance<IBlobUrlResolver>(fileSystemBlobProvider);
                }
                else if (string.Equals(provider, AzureBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    var azureBlobProvider = new AzureBlobProvider(assetsConnectionString);

                    container.RegisterInstance<IBlobStorageProvider>(azureBlobProvider);
                    container.RegisterInstance<IBlobUrlResolver>(azureBlobProvider);
                }
            }

            #endregion

            #region Packaging

            var sourcePath = HostingEnvironment.MapPath("~/App_Data/SourcePackages");
            var packagesPath = HostingEnvironment.MapPath("~/App_Data/InstalledPackages");

            var modulesPath = manifestProvider.RootPath;

            var projectSystem = new WebsiteProjectSystem(modulesPath);

            var nugetProjectManager = new ProjectManager(
                new WebsiteLocalPackageRepository(sourcePath),
                new DefaultPackagePathResolver(modulesPath),
                projectSystem,
                new ManifestPackageRepository(manifestProvider, new WebsitePackageRepository(packagesPath, projectSystem))
            );

            var packageService = new PackageService(nugetProjectManager);

            container.RegisterType<ModulesController>(new InjectionConstructor(packageService, sourcePath));

            #endregion

            #region Settings

            var settingsManager = new SettingsManager(manifestProvider, platformRepositoryFactory, cacheManager);
            container.RegisterInstance<ISettingsManager>(settingsManager);

            #endregion

            #region Security

            var permissionService = new PermissionService(platformRepositoryFactory, manifestProvider, cacheManager);
            container.RegisterInstance<IPermissionService>(permissionService);

            container.RegisterType<IRoleManagementService, RoleManagementService>(new ContainerControlledLifetimeManager());

            var apiAccountProvider = new ApiAccountProvider(platformRepositoryFactory, cacheManager);
            container.RegisterInstance<IApiAccountProvider>(apiAccountProvider);

            container.RegisterType<IClaimsIdentityProvider, ApplicationClaimsIdentityProvider>(new ContainerControlledLifetimeManager());

            container.RegisterType<ApplicationSignInManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>()));
            container.RegisterType<ApplicationUserManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()));
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            #endregion
        }

        private static string MakeRelativePath(string rootPath, string fullPath)
        {
            var rootUri = new Uri(rootPath);
            var fullUri = new Uri(fullPath);
            var relativePath = rootUri.MakeRelativeUri(fullUri).ToString();
            return relativePath;
        }
    }
}
