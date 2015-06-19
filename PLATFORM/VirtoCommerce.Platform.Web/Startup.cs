using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Asset;
using VirtoCommerce.Platform.Data.Caching;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Notification;
using VirtoCommerce.Platform.Data.Packaging;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Web;
using VirtoCommerce.Platform.Web.Controllers.Api;
using WebGrease.Extensions;

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        private static readonly string _assembliesPath = HostingEnvironment.MapPath("~/App_data/Modules");

        public void Configuration(IAppBuilder app)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            const string modulesVirtualPath = "~/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath).EnsureEndSeparator();

            var bootstraper = new VirtoCommercePlatformWebBootstraper(modulesVirtualPath, modulesPhysicalPath, _assembliesPath);
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
            var authenticationOptions = new AuthenticationOptions
            {
                CookiesEnabled = GetAppSettingsValue("VirtoCommerce:Authentication:Cookies.Enabled", true),
                CookiesValidateInterval = GetAppSettingsValue("VirtoCommerce:Authentication:Cookies.ValidateInterval", TimeSpan.FromHours(24)),
                BearerTokensEnabled = GetAppSettingsValue("VirtoCommerce:Authentication:BearerTokens.Enabled", true),
                BearerTokensExpireTimeSpan = GetAppSettingsValue("VirtoCommerce:Authentication:BearerTokens.AccessTokenExpireTimeSpan", TimeSpan.FromHours(1)),
                HmacEnabled = GetAppSettingsValue("VirtoCommerce:Authentication:Hmac.Enabled", true),
                HmacSignatureValidityPeriod = GetAppSettingsValue("VirtoCommerce:Authentication:Hmac.SignatureValidityPeriod", TimeSpan.FromMinutes(20)),
                ApiKeysEnabled = GetAppSettingsValue("VirtoCommerce:Authentication:ApiKeys.Enabled", true),
                ApiKeysHttpHeaderName = GetAppSettingsValue("VirtoCommerce:Authentication:ApiKeys.HttpHeaderName", "api_key"),
                ApiKeysQueryStringParameterName = GetAppSettingsValue("VirtoCommerce:Authentication:ApiKeys.QueryStringParameterName", "api_key"),
            };

            OwinConfig.Configure(app, container, connectionStringName, authenticationOptions);

            var postInitializeModules = moduleCatalog.CompleteListWithDependencies(moduleCatalog.Modules)
                .Where(m => m.ModuleInstance != null)
                .ToArray();

            foreach (var module in postInitializeModules)
            {
                moduleManager.PostInitializeModule(module);
            }

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableJavaScriptProxies = false;
            app.MapSignalR(hubConfiguration);
        }


        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            Debug.WriteLine(string.Format("Resolving assembly '{0}'", args.Name));

            var name = new AssemblyName(args.Name);
            var fileName = name.Name + ".dll";
            var filePath = Path.Combine(_assembliesPath, fileName);

            if (File.Exists(filePath))
            {
                Debug.WriteLine(string.Format("Loading assembly from '{0}'", filePath));
                assembly = Assembly.LoadFrom(filePath);
            }

            return assembly;
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
            container.RegisterInstance<Func<IPlatformRepository>>(platformRepositoryFactory);
            var manifestProvider = container.Resolve<IModuleManifestProvider>();

            #region Caching

            var cacheProvider = new HttpCacheProvider();
            var cacheSettings = new[] 
			{
				new CacheSettings(CacheGroups.Settings, TimeSpan.FromDays(1)),
				new CacheSettings(CacheGroups.Security, TimeSpan.FromMinutes(1)),
			};

            var cacheManager = new CacheManager(cacheProvider, cacheSettings);
            container.RegisterInstance<CacheManager>(cacheManager);

            #endregion

            #region Notifications

            var hubSignalR = GlobalHost.ConnectionManager.GetHubContext<ClientPushHub>();
            var notifier = new InMemoryNotifierImpl(hubSignalR);
            container.RegisterInstance<INotifier>(notifier);

			var resolver = new LiquidNotificationTemplateResolver();
			var notificationTemplateService = new NotificationTemplateServiceImpl(platformRepositoryFactory);
			var notificationManager = new NotificationManager(resolver, platformRepositoryFactory, notificationTemplateService);
			var defaultEmailNotificationSendingGateway = new DefaultEmailNotificationSendingGateway();

			container.RegisterInstance<INotificationTemplateService>(notificationTemplateService);
			container.RegisterInstance<INotificationManager>(notificationManager);
			container.RegisterInstance<IEmailNotificationSendingGateway>(defaultEmailNotificationSendingGateway);

			notificationManager.RegisterNotificationType(
				() => new RegistrationEmailNotification(defaultEmailNotificationSendingGateway)
				{
					NotificationTemplate = new NotificationTemplate
					{
						Body = @"<p> Dear {{ context.first_name }} {{ context.last_name }}, you has registered on our site</p> <p> Your e-mail  - {{ context.email }} </p>",
						Subject = @"<p> Thanks for registration {{ context.first_name }} {{ context.last_name }}!!! </p>",
						NotificationTypeId = "RegistrationEmailNotification",
						ObjectId = "Platform"
					}
				}
			);

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

            var packageService = new ZipPackageService(manifestProvider, packagesPath, sourcePath);

            container.RegisterType<ModulesController>(new InjectionConstructor(packageService, sourcePath));

            #endregion

            #region Settings

            var settingsManager = new SettingsManager(manifestProvider, platformRepositoryFactory, cacheManager);
            container.RegisterInstance<ISettingsManager>(settingsManager);

            #endregion

            #region ChangeLogging

            var changeLogService = new ChangeLogService(platformRepositoryFactory);
            container.RegisterInstance<IChangeLogService>(changeLogService);

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

        private static T GetAppSettingsValue<T>(string name, T defaultValue)
        {
            var result = defaultValue;

            var valueType = typeof(T);
            var stringValue = ConfigurationManager.AppSettings[name];

            if (valueType == typeof(string))
            {
                if (stringValue != null)
                {
                    result = (T)(object)stringValue;
                }
            }
            else if (valueType == typeof(bool))
            {
                bool value;
                if (bool.TryParse(stringValue, out value))
                {
                    result = (T)(object)value;
                }
            }
            else if (valueType == typeof(TimeSpan))
            {
                TimeSpan value;
                if (TimeSpan.TryParse(stringValue, CultureInfo.InvariantCulture, out value))
                {
                    result = (T)(object)value;
                }
            }

            return result;
        }
    }

    public class AuthenticationOptions
    {
        public bool CookiesEnabled { get; set; }
        public TimeSpan CookiesValidateInterval { get; set; }

        public bool BearerTokensEnabled { get; set; }
        public TimeSpan BearerTokensExpireTimeSpan { get; set; }

        public bool HmacEnabled { get; set; }
        public TimeSpan HmacSignatureValidityPeriod { get; set; }

        public bool ApiKeysEnabled { get; set; }
        public string ApiKeysHttpHeaderName { get; set; }
        public string ApiKeysQueryStringParameterName { get; set; }
    }

    public static class HtmlHelperExtensions
    {
        private static MvcHtmlString _version;
        /// <summary>
        /// Versions the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString Version(this HtmlHelper html)
        {
            if (_version == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                _version = new MvcHtmlString(String.Format("{0}.{1}", assembly.GetInformationalVersion(), assembly.GetFileVersion()));
            }

            return _version;
        }
    }
}
