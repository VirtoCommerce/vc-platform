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
using System.Web.Optimization;
using System.Web.Routing;
using CacheManager.Core;
using CacheManager.Core.Configuration;
using CacheManager.Redis;
using Common.Logging;
using Hangfire;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Web.Common;
using VirtoCommerce.Platform.Data.Assets;
using VirtoCommerce.Platform.Data.Azure;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.ExportImport;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Notifications;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Handlers;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Data.Serialization;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Web;
using VirtoCommerce.Platform.Web.BackgroundJobs;
using VirtoCommerce.Platform.Web.Cache;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Hangfire;
using VirtoCommerce.Platform.Web.Modularity;
using VirtoCommerce.Platform.Web.PushNotifications;
using VirtoCommerce.Platform.Web.SignalR;
using VirtoCommerce.Platform.Web.Swagger;
using WebGrease.Extensions;
using AuthenticationOptions = VirtoCommerce.Platform.Core.Security.AuthenticationOptions;
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

        public void Configuration(IAppBuilder app, string virtualRoot, string routePrefix)
        {
            VirtualRoot = virtualRoot;

            _assembliesPath = HostingEnvironment.MapPath(VirtualRoot + "/App_Data/Modules");
            HostingEnvironment.MapPath(VirtualRoot).EnsureEndSeparator();
            var modulesVirtualPath = VirtualRoot + "/Modules";
            var modulesPhysicalPath = HostingEnvironment.MapPath(modulesVirtualPath).EnsureEndSeparator();

            // Try to add modules directory to shadow copy directories
            try
            {
#pragma warning disable 618
                AppDomain.CurrentDomain.SetShadowCopyPath(AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories + ";" + _assembliesPath);
#pragma warning restore 618
            }
            catch (Exception)
            {
                // Ignore any errors, because method AppDomain.CurrentDomain.SetShadowCopyPath is obsolete
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            //Modules initialization
            var bootstrapper = new VirtoCommercePlatformWebBootstrapper(modulesVirtualPath, modulesPhysicalPath, _assembliesPath);
            bootstrapper.Run();

            SetupContainer(app, bootstrapper.Container, new HostingEnvironmentPathMapper(), virtualRoot, routePrefix, modulesPhysicalPath);
        }

        public static void SetupContainer(IAppBuilder app, IUnityContainer container, IPathMapper pathMapper,
            string virtualRoot, string routePrefix, string modulesPhysicalPath)
        {
            container.RegisterInstance(app);

            var moduleInitializerOptions = (ModuleInitializerOptions)container.Resolve<IModuleInitializerOptions>();
            moduleInitializerOptions.VirtualRoot = virtualRoot;
            moduleInitializerOptions.RoutePrefix = routePrefix;

            //Initialize Platform dependencies
            var connectionString = ConfigurationHelper.GetConnectionStringValue("VirtoCommerce");

            var hangfireOptions = new HangfireOptions
            {
                StartServer = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Jobs.Enabled", true),
                JobStorageType = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Jobs.StorageType", "Memory"),
                DatabaseConnectionString = connectionString,
                WorkerCount = ConfigurationHelper.GetNullableAppSettingsValue("VirtoCommerce:Jobs.WorkerCount", (int?)null)
            };
            var hangfireLauncher = new HangfireLauncher(hangfireOptions);

            var authenticationOptions = new AuthenticationOptions
            {
                AllowOnlyAlphanumericUserNames = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AllowOnlyAlphanumericUserNames", false),
                RequireUniqueEmail = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:RequireUniqueEmail", false),

                PasswordRequiredLength = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Password.RequiredLength", 5),
                PasswordRequireNonLetterOrDigit = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Password.RequireNonLetterOrDigit", false),
                PasswordRequireDigit = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Password.RequireDigit", false),
                PasswordRequireLowercase = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Password.RequireLowercase", false),
                PasswordRequireUppercase = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Password.RequireUppercase", false),

                UserLockoutEnabledByDefault = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:UserLockoutEnabledByDefault", true),
                DefaultAccountLockoutTimeSpan = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:DefaultAccountLockoutTimeSpan", TimeSpan.FromMinutes(5)),
                MaxFailedAccessAttemptsBeforeLockout = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:MaxFailedAccessAttemptsBeforeLockout", 5),

                DefaultTokenLifespan = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:DefaultTokenLifespan", TimeSpan.FromDays(1)),

                CookiesEnabled = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookies.Enabled", true),
                CookiesValidateInterval = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookies.ValidateInterval", TimeSpan.FromDays(1)),

                BearerTokensEnabled = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:BearerTokens.Enabled", true),
                AccessTokenExpireTimeSpan = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:BearerTokens.AccessTokenExpireTimeSpan", TimeSpan.FromMinutes(30)),
                RefreshTokenExpireTimeSpan = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:BearerTokens.RefreshTokenExpireTimeSpan", TimeSpan.FromDays(30)),
                BearerAuthorizationLimitedCookiePermissions = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:BearerTokens.LimitedCookiePermissions", string.Empty),

                HmacEnabled = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Hmac.Enabled", true),
                HmacSignatureValidityPeriod = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Hmac.SignatureValidityPeriod", TimeSpan.FromMinutes(20)),

                ApiKeysEnabled = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:ApiKeys.Enabled", true),
                ApiKeysHttpHeaderName = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:ApiKeys.HttpHeaderName", "api_key"),
                ApiKeysQueryStringParameterName = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:ApiKeys.QueryStringParameterName", "api_key"),

                AuthenticationMode = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:AuthenticationMode", AuthenticationMode.Active),
                AuthenticationType = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:AuthenticationType", DefaultAuthenticationTypes.ApplicationCookie),
                CookieDomain = ConfigurationHelper.GetAppSettingsValue<string>("VirtoCommerce:Authentication:Cookie:Domain", null),
                CookieHttpOnly = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:HttpOnly", true),
                CookieName = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:Name", CookieAuthenticationDefaults.CookiePrefix + DefaultAuthenticationTypes.ApplicationCookie),
                CookiePath = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:Path", "/"),
                CookieSecure = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:Secure", CookieSecureOption.SameAsRequest),
                ExpireTimeSpan = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:ExpireTimeSpan", TimeSpan.FromDays(14)),
                LoginPath = new PathString(ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:LoginPath", string.Empty)),
                LogoutPath = new PathString(ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:LogoutPath", string.Empty)),
                ReturnUrlParameter = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:ReturnUrlParameter", CookieAuthenticationDefaults.ReturnUrlParameter),
                SlidingExpiration = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:Cookie:SlidingExpiration", true),

                AzureAdAuthenticationEnabled = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.Enabled", false),
                AzureAdAuthenticationType = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.AuthenticationType", OpenIdConnectAuthenticationDefaults.AuthenticationType),
                AzureAdAuthenticationCaption = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.Caption", OpenIdConnectAuthenticationDefaults.Caption),
                AzureAdApplicationId = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.ApplicationId", string.Empty),
                AzureAdTenantId = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.TenantId", string.Empty),
                AzureAdInstance = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.Instance", string.Empty),
                AzureAdDefaultUserType = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Authentication:AzureAD.DefaultUserType", "Manager")
            };

            container.RegisterInstance(authenticationOptions);

            InitializePlatform(app, container, pathMapper, connectionString, hangfireLauncher, modulesPhysicalPath, moduleInitializerOptions);

            var moduleManager = container.Resolve<IModuleManager>();
            var moduleCatalog = container.Resolve<IModuleCatalog>();

            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.EnsureEndSeparator();

            // Register URL rewriter for platform scripts
            var scriptsPhysicalPath = pathMapper.MapPath(VirtualRoot + "/Scripts").EnsureEndSeparator();
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

            container.RegisterInstance(GlobalConfiguration.Configuration);

            // Ensure all modules are loaded
            foreach (var module in moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.State == ModuleState.NotStarted))
            {
                moduleManager.LoadModule(module.ModuleName);
            }

            SwaggerConfig.RegisterRoutes(container);

            // Post-initialize

            // Register MVC areas unless running in the Web Platform Installer mode
            if (IsApplication)
            {
                AreaRegistration.RegisterAllAreas();
            }

            // Register other MVC resources
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            if (IsApplication)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Security OWIN configuration
            OwinConfig.Configure(app, container);

            hangfireLauncher.ConfigureOwin(app, container);

            RecurringJob.AddOrUpdate<SendNotificationsJobs>("SendNotificationsJob", x => x.Process(), "*/1 * * * *");

            var notificationManager = container.Resolve<INotificationManager>();
            var assembly = typeof(LiquidNotificationTemplateResolver).Assembly;

            notificationManager.RegisterNotificationType(() => new RegistrationEmailNotification(container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Registration notification",
                Description = "This notification is sent by email to a client when he finishes registration",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.RegistrationNotificationTemplateBody.html").ReadToString(),
                    Subject = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.RegistrationNotificationTemplateSubject.html").ReadToString(),
                    Language = "en-US",
                }
            });

            notificationManager.RegisterNotificationType(() => new ResetPasswordEmailNotification(container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Reset password email notification",
                Description = "This notification is sent by email to a client upon reset password request",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.ResetPasswordNotificationTemplateBody.html").ReadToString(),
                    Subject = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.ResetPasswordNotificationTemplateSubject.html").ReadToString(),
                    Language = "en-US",
                }
            });

            notificationManager.RegisterNotificationType(() => new TwoFactorEmailNotification(container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Two factor authentication",
                Description = "This notification contains a security token for two factor authentication",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.TwoFactorNotificationTemplateBody.html").ReadToString(),
                    Subject = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.TwoFactorNotificationTemplateSubject.html").ReadToString(),
                    Language = "en-US",
                }
            });

            notificationManager.RegisterNotificationType(() => new TwoFactorSmsNotification(container.Resolve<ISmsNotificationSendingGateway>())
            {
                DisplayName = "Two factor authentication",
                Description = "This notification contains a security token for two factor authentication",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.TwoFactorNotificationTemplateBody.html").ReadToString(),
                    Subject = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.TwoFactorNotificationTemplateSubject.html").ReadToString(),
                    Language = "en-US",
                }
            });

            notificationManager.RegisterNotificationType(() => new ResetPasswordSmsNotification(container.Resolve<ISmsNotificationSendingGateway>())
            {
                DisplayName = "Reset password sms notification",
                Description = "This notification is sent by sms to a client upon reset password request",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.ResetPasswordSmsNotificationTemplateBody.html").ReadToString(),
                    Subject = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.ResetPasswordSmsNotificationTemplateSubject.html").ReadToString(),
                    Language = "en-US",
                }
            });

            notificationManager.RegisterNotificationType(() => new ChangePhoneNumberSmsNotification(container.Resolve<ISmsNotificationSendingGateway>())
            {
                DisplayName = "Change phone number sms notification",
                Description = "This notification is sent by sms to a client upon change phone number request",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.ChangePhoneNumberSmsNotificationTemplateBody.html").ReadToString(),
                    Subject = assembly.GetManifestResourceStream("VirtoCommerce.Platform.Data.Notifications.Templates.ChangePhoneNumberSmsNotificationTemplateSubject.html").ReadToString(),
                    Language = "en-US",
                }
            });

            //Get initialized modules list sorted by dependency order
            var postInitializeModules = moduleCatalog.CompleteListWithDependencies(moduleCatalog.Modules.OfType<ManifestModuleInfo>())
                .Where(m => m.ModuleInstance != null && m.State == ModuleState.Initialized)
                .ToArray();

            foreach (var module in postInitializeModules)
            {
                moduleManager.PostInitializeModule(module);
            }

            // Initialize InstrumentationKey from EnvironmentVariable
            var applicationInsightsInstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
            if (!string.IsNullOrEmpty(applicationInsightsInstrumentationKey))
            {
                TelemetryConfiguration.Active.InstrumentationKey = applicationInsightsInstrumentationKey;
            }

            // https://docs.microsoft.com/en-us/azure/application-insights/app-insights-live-stream#secure-the-control-channel
            // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/733#issuecomment-349752497
            // https://github.com/Azure/azure-webjobs-sdk/issues/1349
            var applicationInsightsAuthApiKey = Environment.GetEnvironmentVariable("APPINSIGHTS_QUICKPULSEAUTHAPIKEY");
            if (!string.IsNullOrEmpty(applicationInsightsAuthApiKey))
            {
                var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().Single();
                if (module != null)
                {
                    module.AuthenticationApiKey = applicationInsightsAuthApiKey;
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

        private static void InitializePlatform(
            IAppBuilder app,
            IUnityContainer container,
            IPathMapper pathMapper,
            string connectionString,
            HangfireLauncher hangfireLauncher,
            string modulesPath,
            ModuleInitializerOptions moduleInitializerOptions)
        {
            container.RegisterType<ICurrentUser, CurrentUser>(new HttpContextLifetimeManager());
            container.RegisterType<IUserNameResolver, UserNameResolver>();

            #region Setup database

            using (var db = new SecurityDbContext(connectionString))
            {
                new IdentityDatabaseInitializer().InitializeDatabase(db);
            }

            using (var context = new PlatformRepository(connectionString, container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor()))
            {
                new PlatformDatabaseInitializer().InitializeDatabase(context);
            }

            hangfireLauncher.ConfigureDatabase();

            #endregion

            Func<IPlatformRepository> platformRepositoryFactory = () => new PlatformRepository(connectionString, container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor());
            container.RegisterType<IPlatformRepository>(new InjectionFactory(c => platformRepositoryFactory()));
            container.RegisterInstance(platformRepositoryFactory);
            var moduleCatalog = container.Resolve<IModuleCatalog>();

            #region Caching

            //Cure for System.Runtime.Caching.MemoryCache freezing 
            //https://www.zpqrtbnk.net/posts/appdomains-threads-cultureinfos-and-paracetamol
            app.SanitizeThreadCulture();
            ICacheManager<object> cacheManager = null;

            var redisConnectionString = ConfigurationHelper.GetConnectionStringValue("RedisConnectionString");

            //Try to load cache configuration from web.config first
            //Should be aware to using Web cache cache handle because it not worked in native threads. (Hangfire jobs)
            if (ConfigurationManager.GetSection(CacheManagerSection.DefaultSectionName) is CacheManagerSection cacheManagerSection)
            {
                CacheManagerConfiguration configuration = null;

                var defaultCacheManager = cacheManagerSection.CacheManagers.FirstOrDefault(p => p.Name.EqualsInvariant("platformCache"));
                if (defaultCacheManager != null)
                {
                    configuration = ConfigurationBuilder.LoadConfiguration(defaultCacheManager.Name);
                }

                var redisCacheManager = cacheManagerSection.CacheManagers.FirstOrDefault(p => p.Name.EqualsInvariant("redisPlatformCache"));
                if (redisConnectionString != null && redisCacheManager != null)
                {
                    configuration = ConfigurationBuilder.LoadConfiguration(redisCacheManager.Name);
                    configuration.BackplaneChannelName = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Cache:Redis:ChannelName");
                }

                if (configuration != null)
                {
                    configuration.LoggerFactoryType = typeof(CacheManagerLoggerFactory);
                    configuration.LoggerFactoryTypeArguments = new object[] { container.Resolve<ILog>() };
                    cacheManager = CacheFactory.FromConfiguration<object>(configuration);
                }
            }

            // Create a default cache manager if there is no any others
            if (cacheManager == null)
            {
                cacheManager = CacheFactory.Build("platformCache", settings =>
                {
                    settings.WithUpdateMode(CacheUpdateMode.Up)
                            .WithSystemRuntimeCacheHandle("memCacheHandle")
                            .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(5));
                });
            }

            container.RegisterInstance(cacheManager);

            #endregion

            #region Settings

            var platformModuleManifest = new ModuleManifest
            {
                Id = "VirtoCommerce.Platform",
                Version = PlatformVersion.CurrentVersion.ToString(),
                PlatformVersion = PlatformVersion.CurrentVersion.ToString(),
                Settings = new[]
                {
                    new ModuleSettingsGroup
                    {
                        Name = "Platform|Notifications|SendGrid",
                        Settings = new []
                        {
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.Notifications.SendGrid.ApiKey",
                                ValueType = ModuleSetting.TypeSecureString,
                                Title = "SendGrid API key",
                                Description = "Your SendGrid API key"
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
                                ValueType = ModuleSetting.TypeSecureString,
                                Title = "Smtp server password",
                                Description = "Smtp server password"
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.Notifications.SmptClient.UseSsl",
                                ValueType = ModuleSetting.TypeBoolean,
                                Title = "Use SSL",
                                Description = "Use secure connection"
                            },
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
                    },
                    new ModuleSettingsGroup
                    {
                        Name = "Platform|User Profile",
                        Settings = new[]
                        {
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.MainMenu.State",
                                ValueType = ModuleSetting.TypeJson,
                                Title = "Persisted state of main menu"
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.Language",
                                ValueType = ModuleSetting.TypeString,
                                Title = "Language",
                                Description = "Default language (two letter code from ISO 639-1, case-insensitive). Example: en, de",
                                DefaultValue = "en"
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.RegionalFormat",
                                ValueType = ModuleSetting.TypeString,
                                Title = "Regional format",
                                Description = "Default regional format (CLDR locale code, with dash or underscore as delemiter, case-insensitive). Example: en, en_US, sr_Cyrl, sr_Cyrl_RS",
                                DefaultValue = "en"
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.TimeZone",
                                ValueType = ModuleSetting.TypeString,
                                Title = "Time zone",
                                Description = "Default time zone (IANA time zone name [tz database], exactly as in database, case-sensitive). Examples: America/New_York, Europe/Moscow"
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.ShowMeridian",
                                ValueType = ModuleSetting.TypeBoolean,
                                Title = "Meridian labels based on user preferences",
                                Description = "When set to true (by default), system will display time in format like '12 hour format' when possible",
                                DefaultValue = true.ToString()
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.UseTimeAgo",
                                ValueType = ModuleSetting.TypeBoolean,
                                Title = "Use time ago format when is possible",
                                Description = "When set to true (by default), system will display date in format like 'a few seconds ago' when possible",
                                DefaultValue = true.ToString()
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.FullDateThreshold",
                                ValueType = ModuleSetting.TypeInteger,
                                Title = "Full date threshold",
                                Description = "Number of units after time ago format will be switched to full date format"
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.FullDateThresholdUnit",
                                ValueType = ModuleSetting.TypeString,
                                Title = "Full date threshold unit",
                                Description = "Unit of full date threshold",
                                DefaultValue = "Never",
                                AllowedValues = new[]
                                {
                                    "Never",
                                    "Seconds",
                                    "Minutes",
                                    "Hours",
                                    "Days",
                                    "Weeks",
                                    "Months",
                                    "Quarters",
                                    "Years"
                                }
                            },
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.FourDecimalsInMoney",
                                ValueType = ModuleSetting.TypeBoolean,
                                Title = "Show 4 decimal digits for money",
                                Description = "Set to true to show 4 decimal digits for money. By default - false, 2 decimal digits are shown.",
                                DefaultValue = "false",
                            },
                        }
                    },
                    new ModuleSettingsGroup
                    {
                        Name = "Platform|User Interface",
                        Settings = new[]
                        {
                            new ModuleSetting
                            {
                                Name = "VirtoCommerce.Platform.UI.Customization",
                                ValueType = ModuleSetting.TypeJson,
                                Title = "Customization",
                                Description = "JSON contains personalization settings of manager UI",
                                DefaultValue = "{\n" +
                                               "  \"title\": \"Virto Commerce\",\n" +
                                               "  \"logo\": \"Content/themes/main/images/logo.png\",\n" +
                                               "  \"contrast_logo\": \"Content/themes/main/images/contrast-logo.png\",\n" +
                                               "  \"favicon\": \"favicon.ico\"\n" +
                                               "}"
                            }
                        }
                    }
                }
            };

            var settingsManager = new SettingsManager(moduleCatalog, platformRepositoryFactory, cacheManager, new[] { new ManifestModuleInfo(platformModuleManifest) });
            container.RegisterInstance<ISettingsManager>(settingsManager);

            #endregion

            #region Dynamic Properties

            container.RegisterType<IDynamicPropertyService, DynamicPropertyService>(new ContainerControlledLifetimeManager());

            #endregion

            #region Notifications

            // Redis
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                // Cache
                RedisConfigurations.AddConfiguration(new RedisConfiguration("redisConnectionString", redisConnectionString));

                // SignalR
                // https://stackoverflow.com/questions/29885470/signalr-scaleout-on-azure-rediscache-connection-issues
                GlobalHost.DependencyResolver.UseRedis(new RedisScaleoutConfiguration(redisConnectionString, "VirtoCommerce.Platform.SignalR"));
            }

            // SignalR 
            var tempCounterManager = new TempPerformanceCounterManager();
            GlobalHost.DependencyResolver.Register(typeof(IPerformanceCounterManager), () => tempCounterManager);
            var hubConfiguration = new HubConfiguration { EnableJavaScriptProxies = false };
            app.MapSignalR("/" + moduleInitializerOptions.RoutePrefix + "signalr", hubConfiguration);

            var hubSignalR = GlobalHost.ConnectionManager.GetHubContext<ClientPushHub>();
            var notifier = new InMemoryPushNotificationManager(hubSignalR);
            container.RegisterInstance<IPushNotificationManager>(notifier);

            var resolver = new LiquidNotificationTemplateResolver();
            container.RegisterInstance<INotificationTemplateResolver>(resolver);

            var notificationTemplateService = new NotificationTemplateServiceImpl(platformRepositoryFactory);
            container.RegisterInstance<INotificationTemplateService>(notificationTemplateService);

            var notificationManager = new NotificationManager(resolver, platformRepositoryFactory, notificationTemplateService);
            container.RegisterInstance<INotificationManager>(notificationManager);

            IEmailNotificationSendingGateway emailNotificationSendingGateway = null;

            var emailNotificationSendingGatewayName = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:Gateway", "Default");

            if (string.Equals(emailNotificationSendingGatewayName, "Default", StringComparison.OrdinalIgnoreCase))
            {
                emailNotificationSendingGateway = new DefaultSmtpEmailNotificationSendingGateway(settingsManager);
            }
            else if (string.Equals(emailNotificationSendingGatewayName, "SendGrid", StringComparison.OrdinalIgnoreCase))
            {
                emailNotificationSendingGateway = new SendGridEmailNotificationSendingGateway(settingsManager);
            }

            if (emailNotificationSendingGateway != null)
            {
                container.RegisterInstance(emailNotificationSendingGateway);
            }

            ISmsNotificationSendingGateway smsNotificationSendingGateway = null;
            var smsNotificationSendingGatewayName = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway", "Default");

            if (smsNotificationSendingGatewayName.EqualsInvariant("Twilio"))
            {
                smsNotificationSendingGateway = new TwilioSmsNotificationSendingGateway(new TwilioSmsGatewayOptions
                {
                    AccountId = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:AccountId"),
                    AccountPassword = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:AccountPassword"),
                    Sender = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:Sender"),
                });
            }
            else if (smsNotificationSendingGatewayName.EqualsInvariant("ASPSMS"))
            {
                smsNotificationSendingGateway = new AspsmsSmsNotificationSendingGateway(new AspsmsSmsGatewayOptions
                {
                    AccountId = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:AccountId"),
                    AccountPassword = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:AccountPassword"),
                    Sender = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:Sender"),
                    JsonApiUri = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:ASPSMS:JsonApiUri"),
                });
            }
            else
            {
                smsNotificationSendingGateway = new DefaultSmsNotificationSendingGateway();
            }

            container.RegisterInstance(smsNotificationSendingGateway);

            #endregion

            #region Assets

            var blobConnectionString = BlobConnectionString.Parse(ConfigurationHelper.GetConnectionStringValue("AssetsConnectionString"));

            if (string.Equals(blobConnectionString.Provider, FileSystemBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
            {
                var fileSystemBlobProvider = new FileSystemBlobProvider(NormalizePath(pathMapper, blobConnectionString.RootPath), blobConnectionString.PublicUrl);

                container.RegisterInstance<IBlobStorageProvider>(fileSystemBlobProvider);
                container.RegisterInstance<IBlobUrlResolver>(fileSystemBlobProvider);
            }
            else if (string.Equals(blobConnectionString.Provider, AzureBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
            {
                var azureBlobProvider = new AzureBlobProvider(blobConnectionString.ConnectionString, blobConnectionString.CdnUrl);
                container.RegisterInstance<IBlobStorageProvider>(azureBlobProvider);
                container.RegisterInstance<IBlobUrlResolver>(azureBlobProvider);
            }

            container.RegisterType<IAssetEntryService, AssetEntryService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAssetEntrySearchService, AssetEntryService>(new ContainerControlledLifetimeManager());

            #endregion

            #region Modularity

            var modulesDataSources = ConfigurationHelper.SplitAppSettingsStringValue("VirtoCommerce:ModulesDataSources");
            var externalModuleCatalog = new ExternalManifestModuleCatalog(moduleCatalog.Modules, modulesDataSources, container.Resolve<ILog>());
            container.RegisterType<ModulesController>(new InjectionConstructor(externalModuleCatalog, new ModuleInstaller(modulesPath, externalModuleCatalog), notifier, container.Resolve<IUserNameResolver>(), settingsManager));

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
            container.RegisterType<SecurityDbContext>(new InjectionConstructor(connectionString));
            container.RegisterType<IUserStore<ApplicationUser>, ApplicationUserStore>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ApplicationSignInManager>();

            var nonEditableUsers = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:NonEditableUsers", string.Empty);
            container.RegisterInstance<ISecurityOptions>(new SecurityOptions(nonEditableUsers));

            container.RegisterType<ISecurityService, SecurityService>();

            container.RegisterType<IPasswordCheckService, PasswordCheckService>();

            #endregion

            #region ExportImport
            container.RegisterType<IPlatformExportImportManager, PlatformExportImportManager>();
            #endregion

            #region Serialization

            container.RegisterType<IExpressionSerializer, XmlExpressionSerializer>();

            #endregion

            #region Events
            var inProcessBus = new InProcessBus();
            container.RegisterInstance<IHandlerRegistrar>(inProcessBus);
            container.RegisterInstance<IEventPublisher>(inProcessBus);

            inProcessBus.RegisterHandler<UserChangedEvent>(async (message, token) => await container.Resolve<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserPasswordChangedEvent>(async (message, token) => await container.Resolve<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserResetPasswordEvent>(async (message, token) => await container.Resolve<LogChangesUserChangedEventHandler>().Handle(message));
            #endregion
        }

        private static string NormalizePath(IPathMapper pathMapper, string path)
        {
            string retVal;

            if (path.StartsWith("~"))
            {
                retVal = pathMapper.MapPath(path);
            }
            else if (Path.IsPathRooted(path))
            {
                retVal = path;
            }
            else
            {
                retVal = pathMapper.MapPath("~/");
                retVal += path;
            }

            return retVal != null ? Path.GetFullPath(retVal) : null;
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
