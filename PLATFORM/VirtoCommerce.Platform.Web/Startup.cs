#region usings

using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Packaging;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Asset;
using VirtoCommerce.Platform.Data.Caching;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.ExportImport;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Notifications;
using VirtoCommerce.Platform.Data.Packaging;
using VirtoCommerce.Platform.Data.PushNotifications;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Web;
using VirtoCommerce.Platform.Web.BackgroundJobs;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Resources;
using VirtoCommerce.Platform.Web.SignalR;
using WebGrease.Extensions;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        private static string _assembliesPath;

        public static bool IsApplication { get; private set; }
        public static string VirtualRoot { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            IsApplication = true;
            Configuration(app, "~", string.Empty);
        }

        public void Configuration(IAppBuilder app, string virtualRoot, string routPrefix)
        {
            VirtualRoot = virtualRoot;

            _assembliesPath = HostingEnvironment.MapPath(VirtualRoot + "/App_Data/Modules");
            var modulesVirtualPath = VirtualRoot + "/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath).EnsureEndSeparator();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            //Modules initialization
            var bootstrapper = new VirtoCommercePlatformWebBootstrapper(modulesVirtualPath, modulesPhysicalPath, _assembliesPath);
            bootstrapper.Run();

            var container = bootstrapper.Container;
            container.RegisterInstance(app);

            var moduleInitializerOptions = (ModuleInitializerOptions)container.Resolve<IModuleInitializerOptions>();
            moduleInitializerOptions.VirtualRoot = virtualRoot;
            moduleInitializerOptions.RoutPrefix = routPrefix;

            //Initialize Platform dependencies
            const string connectionStringName = "VirtoCommerce";
            InitializePlatform(app, container, connectionStringName);

            var moduleManager = container.Resolve<IModuleManager>();
            var moduleCatalog = container.Resolve<IModuleCatalog>();

            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.EnsureEndSeparator();

            // Register URL rewriter for platform scripts
            var scriptsPhysicalPath = HostingEnvironment.MapPath(VirtualRoot + "/Scripts").EnsureEndSeparator();
            var scriptsRelativePath = MakeRelativePath(applicationBase, scriptsPhysicalPath);
            var platformUrlRewriterOptions = new UrlRewriterOptions();
            platformUrlRewriterOptions.Items.Add(PathString.FromUriComponent("/$(Platform)/Scripts"), "");
            app.Use<UrlRewriterOwinMiddleware>(platformUrlRewriterOptions);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem(scriptsRelativePath)
            });

            // Register URL rewriter before modules initialization
            if (Directory.Exists(modulesPhysicalPath))
            {
                var modulesRelativePath = MakeRelativePath(applicationBase, modulesPhysicalPath);

                var urlRewriterOptions = new UrlRewriterOptions();

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

            // Ensure all modules are loaded
            foreach (var module in moduleCatalog.Modules.Where(x => x.State == ModuleState.NotStarted))
            {
                moduleManager.LoadModule(module.ModuleName);
            }

            // Post-initialize

            // Platform MVC configuration
            if (IsApplication)
            {
                AreaRegistration.RegisterAllAreas();
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Security OWIN configuration
            var authenticationOptions = new AuthenticationOptions
            {
                CookiesEnabled = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:Cookies.Enabled", true),
                CookiesValidateInterval = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:Cookies.ValidateInterval", TimeSpan.FromDays(1)),
                BearerTokensEnabled = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:BearerTokens.Enabled", true),
                BearerTokensExpireTimeSpan = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:BearerTokens.AccessTokenExpireTimeSpan", TimeSpan.FromHours(1)),
                HmacEnabled = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:Hmac.Enabled", true),
                HmacSignatureValidityPeriod = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:Hmac.SignatureValidityPeriod", TimeSpan.FromMinutes(20)),
                ApiKeysEnabled = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:ApiKeys.Enabled", true),
                ApiKeysHttpHeaderName = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:ApiKeys.HttpHeaderName", "api_key"),
                ApiKeysQueryStringParameterName = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Authentication:ApiKeys.QueryStringParameterName", "api_key"),
            };
            OwinConfig.Configure(app, container, connectionStringName, authenticationOptions);

            RecurringJob.AddOrUpdate<SendNotificationsJobs>("SendNotificationsJob", x => x.Process(), "*/1 * * * *");

            var notificationManager = container.Resolve<INotificationManager>();
            notificationManager.RegisterNotificationType(() => new RegistrationEmailNotification(container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Registration notification",
                Description = "This notification sends by email to client when he finish registration",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = PlatformNotificationResource.RegistrationNotificationBody,
                    Subject = PlatformNotificationResource.RegistrationNotificationSubject,
                    Language = "en-US"
                }
            });

            var postInitializeModules = moduleCatalog.CompleteListWithDependencies(moduleCatalog.Modules)
                .Where(m => m.ModuleInstance != null)
                .ToArray();

            foreach (var module in postInitializeModules)
            {
                moduleManager.PostInitializeModule(module);
            }

            // SignalR
            var tempCounterManager = new TempPerformanceCounterManager();
            GlobalHost.DependencyResolver.Register(typeof(IPerformanceCounterManager), () => tempCounterManager);
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableJavaScriptProxies = false;
            app.MapSignalR("/" + moduleInitializerOptions.RoutPrefix + "signalr", hubConfiguration);
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

        private static void InitializePlatform(IAppBuilder app, IUnityContainer container, string connectionStringName)
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
            container.RegisterInstance(platformRepositoryFactory);
            var moduleCatalog = container.Resolve<IModuleCatalog>();
            var manifestProvider = container.Resolve<IModuleManifestProvider>();

            #region Caching

            var cacheProvider = new HttpCacheProvider();
            var cacheSettings = new[]
            {
                new CacheSettings(CacheGroups.Settings, TimeSpan.FromDays(1)),
                new CacheSettings(CacheGroups.Security, TimeSpan.FromMinutes(1)),
            };

            var cacheManager = new CacheManager(cacheProvider, cacheSettings);
            container.RegisterInstance(cacheManager);

            #endregion

            #region Settings

            var platformSettings = new[]
            {
                new ModuleManifest
                {
                    Settings = new[]
                    {
                        new ModuleSettingsGroup
                        {
                            Name = "Platform|Notifications|SendGrid",
                            Settings = new []
                            {
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SendGrid.UserName",
                                    ValueType = ModuleSetting.TypeString,
                                    Title = "SendGrid UserName",
                                    Description = "Your SendGrid account username"
                                },
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SendGrid.Secret",
                                    ValueType = ModuleSetting.TypeString,
                                    Title = "SendGrid Password",
                                    Description = "Your SendGrid account password"
                                }
                            }
                        },

                        new ModuleSettingsGroup
                        {
                            Name = "Platform|Notifications|SendingJob",
                            Settings = new []
                            {
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SendingJob.TakeCount",
                                    ValueType = ModuleSetting.TypeInteger,
                                    Title = "Job Take Count",
                                    Description = "Take count for sending job"
                                }
                            }
                        },
                         new ModuleSettingsGroup
                        {
                            Name = "Platform|Security",
                            Settings = new []
                            {
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Security.AccountTypes",
                                    ValueType = ModuleSetting.TypeString,
                                    Title = "Account types",
                                    Description = "Dictionary for possible account types",
                                    IsArray = true,
                                    ArrayValues = Enum.GetNames(typeof(AccountType)),
                                    DefaultValue = AccountType.Manager.ToString()
                                }
                            }
                        }
                    }
                }
            };

            var settingsManager = new SettingsManager(manifestProvider, platformRepositoryFactory, cacheManager, platformSettings);
            container.RegisterInstance<ISettingsManager>(settingsManager);

            #endregion

            #region Dynamic Properties

            container.RegisterType<IDynamicPropertyService, DynamicPropertyService>();

            #endregion

            #region Notifications

            var hubSignalR = GlobalHost.ConnectionManager.GetHubContext<ClientPushHub>();
            var notifier = new InMemoryPushNotificationManager(hubSignalR);
            container.RegisterInstance<IPushNotificationManager>(notifier);

            var resolver = new LiquidNotificationTemplateResolver();
            var notificationTemplateService = new NotificationTemplateServiceImpl(platformRepositoryFactory);
            var notificationManager = new NotificationManager(resolver, platformRepositoryFactory, notificationTemplateService);

            var emailNotificationSendingGateway = new DefaultEmailNotificationSendingGateway(settingsManager);

            var defaultSmsNotificationSendingGateway = new DefaultSmsNotificationSendingGateway();

            container.RegisterInstance<INotificationTemplateService>(notificationTemplateService);
            container.RegisterInstance<INotificationManager>(notificationManager);
            container.RegisterInstance<INotificationTemplateResolver>(resolver);
            container.RegisterInstance<IEmailNotificationSendingGateway>(emailNotificationSendingGateway);
            container.RegisterInstance<ISmsNotificationSendingGateway>(defaultSmsNotificationSendingGateway);


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

            var packagesPath = HostingEnvironment.MapPath(VirtualRoot + "/App_Data/InstalledPackages");
            var packageService = new ZipPackageService(moduleCatalog, manifestProvider, packagesPath);
            container.RegisterInstance<IPackageService>(packageService);

            var uploadsPath = HostingEnvironment.MapPath(VirtualRoot + "/App_Data/Uploads");
            container.RegisterType<ModulesController>(new InjectionConstructor(packageService, uploadsPath, notifier));

            #endregion

            #region ChangeLogging

            var changeLogService = new ChangeLogService(platformRepositoryFactory);
            container.RegisterInstance<IChangeLogService>(changeLogService);

            #endregion

            #region Security
            container.RegisterInstance<IPermissionScopeService>(new PermissionScopeService());
            container.RegisterType<IRoleManagementService, RoleManagementService>(new ContainerControlledLifetimeManager());

            var apiAccountProvider = new ApiAccountProvider(platformRepositoryFactory, cacheManager);
            container.RegisterInstance<IApiAccountProvider>(apiAccountProvider);

            container.RegisterType<IClaimsIdentityProvider, ApplicationClaimsIdentityProvider>(new ContainerControlledLifetimeManager());

            container.RegisterInstance(app.GetDataProtectionProvider());
            container.RegisterType<SecurityDbContext>(new InjectionConstructor(connectionStringName));
            container.RegisterType<IUserStore<ApplicationUser>, ApplicationUserStore>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ApplicationSignInManager>();

            var nonEditableUsers = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:NonEditableUsers", string.Empty);
            container.RegisterInstance<ISecurityOptions>(new SecurityOptions(nonEditableUsers));

            container.RegisterType<ISecurityService, SecurityService>();

            #endregion

            #region ExportImport
            container.RegisterType<IPlatformExportImportManager, PlatformExportImportManager>();
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
