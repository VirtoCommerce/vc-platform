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
using CacheManager.Core;
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
using VirtoCommerce.Platform.Web.Hangfire;
using VirtoCommerce.Platform.Web.Resources;
using VirtoCommerce.Platform.Web.SignalR;
using WebGrease.Extensions;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

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
            var platformPath = HostingEnvironment.MapPath(VirtualRoot).EnsureEndSeparator();
            var modulesVirtualPath = VirtualRoot + "/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath).EnsureEndSeparator();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            //Modules initialization
            var bootstrapper = new VirtoCommercePlatformWebBootstrapper(modulesVirtualPath, modulesPhysicalPath, _assembliesPath, platformPath);
            bootstrapper.Run();

            var container = bootstrapper.Container;
            container.RegisterInstance(app);

            var moduleInitializerOptions = (ModuleInitializerOptions)container.Resolve<IModuleInitializerOptions>();
            moduleInitializerOptions.VirtualRoot = virtualRoot;
            moduleInitializerOptions.RoutePrefix = routPrefix;

            //Initialize Platform dependencies
            const string connectionStringName = "VirtoCommerce";

            var hangfireOptions = new HangfireOptions
            {
                StartServer = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Jobs.Enabled", true),
                JobStorageType = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Jobs.StorageType", "Memory"),
                DatabaseConnectionStringName = connectionStringName,
            };
            var hangfireLauncher = new HangfireLauncher(hangfireOptions);

            InitializePlatform(app, container, connectionStringName, hangfireLauncher);

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
            var authenticationOptions = new Core.Security.AuthenticationOptions
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
            OwinConfig.Configure(app, container, authenticationOptions);

            hangfireLauncher.ConfigureOwin(app, container);

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

            notificationManager.RegisterNotificationType(() => new ResetPasswordEmailNotification(container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Reset password notification",
                Description = "This notification sends by email to client when he want to reset his password",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = PlatformNotificationResource.ResetPasswordNotificationBody,
                    Subject = PlatformNotificationResource.ResetPasswordNotificationSubject,
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
            var hubConfiguration = new HubConfiguration { EnableJavaScriptProxies = false };
            app.MapSignalR("/" + moduleInitializerOptions.RoutePrefix + "signalr", hubConfiguration);

            //Start background sample data installation if in config set concrete zip path (need for demo)
            var settingManager = container.Resolve<ISettingsManager>();
            if (!settingManager.GetValue("VirtoCommerce:SampleDataInstalled", false))
            {
                var sampleDataUrl = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:SampleDataUrl", string.Empty);
                if (!string.IsNullOrEmpty(sampleDataUrl) && sampleDataUrl.EndsWith(".zip"))
                {
                    var exportImportController = container.Resolve<PlatformExportImportController>();
                    exportImportController.TryToImportSampleData(sampleDataUrl);
                }
            }
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Resolving assembly '{0}'", args.Name));

            var name = new AssemblyName(args.Name);
            var fileName = name.Name + ".dll";
            var filePath = Path.Combine(_assembliesPath, fileName);

            if (File.Exists(filePath))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Loading assembly from '{0}'", filePath));
                assembly = Assembly.LoadFrom(filePath);
            }

            return assembly;
        }

        private static void InitializePlatform(IAppBuilder app, IUnityContainer container, string connectionStringName, HangfireLauncher hangfireLauncher)
        {
            container.RegisterType<ICurrentUser, CurrentUser>(new HttpContextLifetimeManager());
            container.RegisterType<IUserNameResolver, UserNameResolver>();

            #region Setup database

            using (var db = new SecurityDbContext(connectionStringName))
            {
                new IdentityDatabaseInitializer().InitializeDatabase(db);
            }

            using (var context = new PlatformRepository(connectionStringName, container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor()))
            {
                new PlatformDatabaseInitializer().InitializeDatabase(context);
            }

            hangfireLauncher.ConfigureDatabase();

            #endregion


            Func<IPlatformRepository> platformRepositoryFactory = () => new PlatformRepository(connectionStringName, container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor());
            container.RegisterType<IPlatformRepository>(new InjectionFactory(c => platformRepositoryFactory()));
            container.RegisterInstance(platformRepositoryFactory);
            var moduleCatalog = container.Resolve<IModuleCatalog>();
            var manifestProvider = container.Resolve<IModuleManifestProvider>();

            #region Caching
            var cacheManager = CacheFactory.Build("platformCache", settings =>
            {
                //Should be aware to using Web cache cache handle because it not worked in native threads. (Hangfire jobs)
                settings
                    .WithUpdateMode(CacheUpdateMode.Up)
                    .WithSystemRuntimeCacheHandle("memCacheHandle")
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromDays(1));
            });
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
                            Name = "Platform|Notifications|SmtpClient",
                            Settings = new []
                            {
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SmptClient.Host",
                                    ValueType = ModuleSetting.TypeString,
                                    Title = "Smtp server host",
                                    Description = "Smtp server host"
                                },
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SmptClient.Port",
                                    ValueType = ModuleSetting.TypeInteger,
                                    Title = "Smtp server port",
                                    Description = "Smtp server port"
                                },
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SmptClient.Login",
                                    ValueType = ModuleSetting.TypeString,
                                    Title = "Smtp server login",
                                    Description = "Smtp server login"
                                },
                                new ModuleSetting
                                {
                                    Name = "VirtoCommerce.Platform.Notifications.SmptClient.Password",
                                    ValueType = ModuleSetting.TypeString,
                                    Title = "Smtp server password",
                                    Description = "Smtp server password"
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

            container.RegisterType<IDynamicPropertyService, DynamicPropertyService>(new ContainerControlledLifetimeManager());

            #endregion

            #region Notifications

            var hubSignalR = GlobalHost.ConnectionManager.GetHubContext<ClientPushHub>();
            var notifier = new InMemoryPushNotificationManager(hubSignalR);
            container.RegisterInstance<IPushNotificationManager>(notifier);

            var resolver = new LiquidNotificationTemplateResolver();
            var notificationTemplateService = new NotificationTemplateServiceImpl(platformRepositoryFactory);
            var notificationManager = new NotificationManager(resolver, platformRepositoryFactory, notificationTemplateService);

            //var emailNotificationSendingGateway = new DefaultEmailNotificationSendingGateway(settingsManager);
            var emailNotificationSendingGateway = new DefaultSmtpEmailNotificationSendingGateway(settingsManager);

            var defaultSmsNotificationSendingGateway = new DefaultSmsNotificationSendingGateway();

            container.RegisterInstance<INotificationTemplateService>(notificationTemplateService);
            container.RegisterInstance<INotificationManager>(notificationManager);
            container.RegisterInstance<INotificationTemplateResolver>(resolver);
            container.RegisterInstance<IEmailNotificationSendingGateway>(emailNotificationSendingGateway);
            container.RegisterInstance<ISmsNotificationSendingGateway>(defaultSmsNotificationSendingGateway);


            #endregion

            #region Assets

            var blobConnectionString = BlobConnectionString.Parse(ConfigurationManager.ConnectionStrings["AssetsConnectionString"].ConnectionString);

            if (string.Equals(blobConnectionString.Provider, FileSystemBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
            {
                var fileSystemBlobProvider = new FileSystemBlobProvider(NormalizePath(blobConnectionString.RootPath), blobConnectionString.PublicUrl);

                container.RegisterInstance<IBlobStorageProvider>(fileSystemBlobProvider);
                container.RegisterInstance<IBlobUrlResolver>(fileSystemBlobProvider);
            }
            else if (string.Equals(blobConnectionString.Provider, AzureBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
            {
                var azureBlobProvider = new AzureBlobProvider(blobConnectionString.ConnectionString);
                container.RegisterInstance<IBlobStorageProvider>(azureBlobProvider);
                container.RegisterInstance<IBlobUrlResolver>(azureBlobProvider);
            }


            #endregion

            #region Packaging

            var packagesPath = HostingEnvironment.MapPath(VirtualRoot + "/App_Data/InstalledPackages");
            var packageService = new ZipPackageService(moduleCatalog, manifestProvider, packagesPath);
            container.RegisterInstance<IPackageService>(packageService);

            var uploadsPath = HostingEnvironment.MapPath(VirtualRoot + "/App_Data/Uploads");
            container.RegisterType<ModulesController>(new InjectionConstructor(packageService, uploadsPath, notifier, container.Resolve<IUserNameResolver>()));

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

        private static string NormalizePath(string path)
        {
            var retVal = path;
            if (path.StartsWith("~"))
            {
                retVal = HostingEnvironment.MapPath(path);
            }
            else if (Path.IsPathRooted(path))
            {
                retVal = path;
            }
            else
            {
                retVal = HostingEnvironment.MapPath("~/");
                retVal += path;
            }
            return Path.GetFullPath(retVal);
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
