using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Logger;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.ExternalSignIn;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.MySql;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.MySql.HealthCheck;
using VirtoCommerce.Platform.Data.PostgreSql;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql.HealthCheck;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.SqlServer;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;
using VirtoCommerce.Platform.Data.SqlServer.HealthCheck;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.Hangfire.Extensions;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.Local;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Authorization;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Infrastructure;
using VirtoCommerce.Platform.Web.Infrastructure.HealthCheck;
using VirtoCommerce.Platform.Web.Json;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Middleware;
using VirtoCommerce.Platform.Web.Migrations;
using VirtoCommerce.Platform.Web.PushNotifications;
using VirtoCommerce.Platform.Web.Redis;
using VirtoCommerce.Platform.Web.Security;
using VirtoCommerce.Platform.Web.Security.Authentication;
using VirtoCommerce.Platform.Web.Security.Authorization;
using VirtoCommerce.Platform.Web.Swagger;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using MsTokens = Microsoft.IdentityModel.Tokens;


namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = hostingEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public ServerCertificate ServerCertificate { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConsoleLog.BeginOperation("Virto Commerce is loading");

            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");

            ConsoleLog.EndOperation();

            // Optional Modules Dependecy Resolving
            services.Add(ServiceDescriptor.Singleton(typeof(IOptionalDependency<>), typeof(OptionalDependencyManager<>)));

            services.AddForwardedHeaders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // This custom provider allows able to use just [Authorize] instead of having to define [Authorize(AuthenticationSchemes = "Bearer")] above every API controller
            // without this Bearer authorization will not work
            services.AddSingleton<IAuthenticationSchemeProvider, CustomAuthenticationSchemeProvider>();

            services.AddRedis(Configuration);

            services.AddSignalR().AddPushNotifications(Configuration);

            services.AddOptions<PlatformOptions>().Bind(Configuration.GetSection("VirtoCommerce")).ValidateDataAnnotations();
            services.AddOptions<DistributedLockOptions>().Bind(Configuration.GetSection("DistributedLock"));
            services.AddOptions<TranslationOptions>().Configure(options =>
            {
                options.PlatformTranslationFolderPath = WebHostEnvironment.MapPath(options.PlatformTranslationFolderPath);
            });
            services.AddOptions<SecurityHeadersOptions>().Bind(Configuration.GetSection("SecurityHeaders")).ValidateDataAnnotations();

            //Get platform version from GetExecutingAssembly
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);

            services.AddSingleton<IFileCopyPolicy, FileCopyPolicy>();
            services.AddSingleton<IFileMetadataProvider, FileMetadataProvider>();

            services.AddDbContext<PlatformDbContext>((provider, options) =>
            {
                var connectionString = Configuration.GetConnectionString("VirtoCommerce");

                switch (databaseProvider)
                {
                    case "MySql":
                        options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), Configuration);
                        break;
                    case "PostgreSql":
                        options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), Configuration);
                        break;
                    default:
                        options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), Configuration);
                        break;
                }
            });

            services.AddPlatformServices(Configuration);

            services.AddSingleton<LicenseProvider>();

            var platformOptions = Configuration.GetSection("VirtoCommerce").Get<PlatformOptions>();

            var mvcBuilder = services.AddMvc(mvcOptions =>
            {
                //Disable 204 response for null result. https://github.com/aspnet/AspNetCore/issues/8847
                var noContentFormatter = mvcOptions.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
                if (noContentFormatter != null)
                {
                    noContentFormatter.TreatNullValueAsNoContent = false;
                }
            })
            .AddNewtonsoftJson(options =>
            {
                //Next line needs to represent custom derived types in the resulting swagger doc definitions. Because default SwaggerProvider used global JSON serialization settings
                //we should register this converter globally.
                options.SerializerSettings.ContractResolver = new PolymorphJsonContractResolver();
                //Next line allow to use polymorphic types as parameters in API controller methods
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Converters.Add(new ModuleIdentityJsonConverter());
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.None;
            })
            .AddOutputJsonSerializerSettings((settings, jsonOptions) =>
            {
                settings.CopyFrom(jsonOptions.SerializerSettings);

                if (platformOptions.IncludeOutputNullValues)
                {
                    settings.NullValueHandling = NullValueHandling.Include;
                }
            });

            services.AddSingleton(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
                return JsonSerializer.Create(options.Value.SerializerSettings);
            });

            services.AddDbContext<SecurityDbContext>(options =>
            {
                var connectionString = Configuration["Auth:ConnectionString"] ??
                    Configuration.GetConnectionString("VirtoCommerce");

                switch (databaseProvider)
                {
                    case "MySql":
                        options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), Configuration);
                        break;
                    case "PostgreSql":
                        options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), Configuration);
                        break;
                    default:
                        options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), Configuration);
                        break;
                }

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });

            if (platformOptions.UseResponseCompression)
            {
                services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });
            }

            // Enable synchronous IO if using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // Enable synchronous IO if using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var authBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //Add the second ApiKey auth schema to handle api_key in query string
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options => { })
                //Add the third BasicAuth auth schema
                .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicAuthenticationOptions.DefaultScheme, options => { })
                .AddCookie();

            services.AddSecurityServices(options =>
            {
            });

            services.AddIdentity<ApplicationUser, Role>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders()
                .AddUserValidator<CustomUserValidator>();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            });

            services.ConfigureOptions<ConfigureSecurityStampValidatorOptions>();

            // Load server certificate (from DB or file) and register it as a global singleton
            // to allow the platform hosting under the cert
            ICertificateLoader certificateLoader;
            switch (databaseProvider)
            {
                case "MySql":
                    certificateLoader = new MySqlCertificateLoader(Configuration);
                    services.AddSingleton<ICertificateLoader>(s => { return certificateLoader; });
                    break;
                case "PostgreSql":
                    certificateLoader = new PostgreSqlCertificateLoader(Configuration);
                    services.AddSingleton<ICertificateLoader>(s => { return certificateLoader; });
                    break;
                default:
                    certificateLoader = new SqlServerCertificateLoader(Configuration);
                    services.AddSingleton<ICertificateLoader>(s => { return certificateLoader; });
                    break;
            }

            ConsoleLog.BeginOperation("Getting server certificate");
            ServerCertificate = GetServerCertificate(certificateLoader);
            ConsoleLog.EndOperation();

            //Create backup of token handler before default claim maps are cleared
            var defaultTokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            // register it as a singleton to use in external login providers
            services.AddSingleton(defaultTokenHandler);

            authBuilder.AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth:Authority"];
                options.Audience = Configuration["Auth:Audience"];

                if (WebHostEnvironment.IsDevelopment())
                {
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                }

                MsTokens.X509SecurityKey publicKey = null;

                var publicCert = ServerCertificate.X509Certificate;
                publicKey = new MsTokens.X509SecurityKey(publicCert);
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new MsTokens.TokenValidationParameters
                {
                    NameClaimType = OpenIddictConstants.Claims.Subject,
                    RoleClaimType = OpenIddictConstants.Claims.Role,
                    ValidateIssuer = !string.IsNullOrEmpty(options.Authority),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = publicKey
                };
            });

            services.AddOptions<Core.Security.AuthorizationOptions>().Bind(Configuration.GetSection("Authorization")).ValidateDataAnnotations();
            var authorizationOptions = Configuration.GetSection("Authorization").Get<Core.Security.AuthorizationOptions>();

            // Register the OpenIddict services.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<SecurityDbContext>();
                }).AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    var builder = options.UseAspNetCore().
                        EnableTokenEndpointPassthrough().
                        EnableAuthorizationEndpointPassthrough();

                    // Enable the authorization, logout, token and userinfo endpoints.
                    options.SetTokenEndpointUris("connect/token");
                    options.SetUserinfoEndpointUris("api/security/userinfo");

                    // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
                    // can enable the other flows if you need to support implicit or client credentials.
                    options.AllowPasswordFlow()
                        .AllowRefreshTokenFlow()
                        .AllowClientCredentialsFlow()
                        .AllowCustomFlow(PlatformConstants.Security.GrantTypes.Impersonate)
                        .AllowCustomFlow(PlatformConstants.Security.GrantTypes.ExternalSignIn);

                    options.SetRefreshTokenLifetime(authorizationOptions?.RefreshTokenLifeTime);
                    options.SetAccessTokenLifetime(authorizationOptions?.AccessTokenLifeTime);

                    options.AcceptAnonymousClients();

                    // Configure Openiddict to issues new refresh token for each token refresh request.
                    // Enabled by default, to disable use options.DisableRollingRefreshTokens()

                    // Make the "client_id" parameter mandatory when sending a token request.
                    //options.RequireClientIdentification()

                    // When request caching is enabled, authorization and logout requests
                    // are stored in the distributed cache by OpenIddict and the user agent
                    // is redirected to the same page with a single parameter (request_id).
                    // This allows flowing large OpenID Connect requests even when using
                    // an external authentication provider like Google, Facebook or Twitter.
                    builder.EnableAuthorizationRequestCaching();
                    builder.EnableLogoutRequestCaching();

                    options.DisableScopeValidation();

                    // During development or when you explicitly run the platform in production mode without https, need to disable the HTTPS requirement.
                    if (WebHostEnvironment.IsDevelopment() || platformOptions.AllowInsecureHttp || !Configuration.IsHttpsServerUrlSet())
                    {
                        builder.DisableTransportSecurityRequirement();
                    }

                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    options.DisableAccessTokenEncryption();

                    X509Certificate2 privateKey;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        // https://github.com/dotnet/corefx/blob/release/2.2/Documentation/architecture/cross-platform-cryptography.md
                        // macOS cannot load certificate private keys without a keychain object, which requires writing to disk.
                        // Keychains are created automatically for PFX loading, and are deleted when no longer in use.
                        // Since the X509KeyStorageFlags.EphemeralKeySet option means that the private key should not be written to disk, asserting that flag on macOS results in a PlatformNotSupportedException.
                        privateKey = new X509Certificate2(ServerCertificate.PrivateKeyCertBytes, ServerCertificate.PrivateKeyCertPassword, X509KeyStorageFlags.MachineKeySet);
                    }
                    else
                    {
                        privateKey = new X509Certificate2(ServerCertificate.PrivateKeyCertBytes, ServerCertificate.PrivateKeyCertPassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
                    }
                    options.AddSigningCertificate(privateKey);
                    options.AddEncryptionCertificate(privateKey);
                });

            services.Configure<IdentityOptions>(Configuration.GetSection("IdentityOptions"));
            services.Configure<PasswordOptionsExtended>(Configuration.GetSection("IdentityOptions:Password"));
            services.Configure<LockoutOptionsExtended>(Configuration.GetSection("IdentityOptions:Lockout"));
            services.Configure<PasswordLoginOptions>(Configuration.GetSection("PasswordLogin"));
            services.Configure<UserOptionsExtended>(Configuration.GetSection("IdentityOptions:User"));
            services.Configure<DataProtectionTokenProviderOptions>(Configuration.GetSection("IdentityOptions:DataProtection"));
            services.Configure<FixedSettings>(Configuration.GetSection("PlatformSettings"));

            //always  return 401 instead of 302 for unauthorized  requests
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".VirtoCommerce.Identity.Application";

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                //We need this policy because it is a single way to implicitly use the three schemas (JwtBearer, ApiKey and Basic) authentication for resource based authorization.
                var multipleSchemaAuthPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationOptions.DefaultScheme, BasicAuthenticationOptions.DefaultScheme)
                    .RequireAuthenticatedUser()
                    // Customer user can get token, but can't use any API where auth is needed
                    .RequireAssertion(context =>
                        authorizationOptions.AllowApiAccessForCustomers ||
                        !context.User.HasClaim(OpenIddictConstants.Claims.Role, PlatformConstants.Security.SystemRoles.Customer))
                    .Build();
                //The good article is described the meaning DefaultPolicy and FallbackPolicy
                //https://scottsauber.com/2020/01/20/globally-require-authenticated-users-by-default-using-fallback-policies-in-asp-net-core/
                options.DefaultPolicy = multipleSchemaAuthPolicy;
            });
            // register the AuthorizationPolicyProvider which dynamically registers authorization policies for each permission defined in module manifest
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            //Platform authorization handler for policies based on permissions
            services.AddSingleton<IAuthorizationHandler, DefaultPermissionAuthorizationHandler>();

            services.AddTransient<IExternalSignInService, ExternalSignInService>();
            services.AddTransient<IExternalSigninService, ExternalSignInService>();

            services.AddOptions<LocalStorageModuleCatalogOptions>().Bind(Configuration.GetSection("VirtoCommerce"))
                    .PostConfigure(options =>
                    {
                        options.DiscoveryPath = Path.GetFullPath(options.DiscoveryPath ?? "modules");
                    })
                    .ValidateDataAnnotations();

            services.AddOptions<ModuleSequenceBoostOptions>().Bind(Configuration.GetSection("VirtoCommerce"));

            services.AddModules(mvcBuilder);

            services.AddOptions<ExternalModuleCatalogOptions>().Bind(Configuration.GetSection("ExternalModules")).ValidateDataAnnotations();
            services.AddExternalModules();

            //HangFire
            services.AddHangfire(Configuration);

            // Register the Swagger generator
            services.AddSwagger(Configuration, platformOptions.UseAllOfToExtendReferenceSchemas);

            var healthBuilder = services.AddHealthChecks()
                .AddCheck<ModulesHealthChecker>("Modules health",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "Modules" })
                .AddCheck<CacheHealthChecker>("Cache health",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "Cache" })
                .AddCheck<RedisHealthCheck>("Redis health",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "Cache" });

            var connectionString = Configuration.GetConnectionString("VirtoCommerce");
            switch (databaseProvider)
            {
                case "MySql":
                    healthBuilder.AddMySql(connectionString,
                        name: "MySql health",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "Database" });
                    break;
                case "PostgreSql":
                    healthBuilder.AddNpgSql(connectionString,
                        name: "PostgreSql health",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "Database" });
                    break;
                default:
                    healthBuilder.AddSqlServer(connectionString,
                        name: "SQL Server health",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "Database" });
                    break;
            }

            // Platform UI options
            services.AddOptions<PlatformUIOptions>().Bind(Configuration.GetSection("VirtoCommerce:PlatformUI"));

            // Add login page UI options
            var loginPageUIOptions = Configuration.GetSection("LoginPageUI");
            services.AddOptions<LoginPageUIOptions>().Bind(loginPageUIOptions);
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddHttpClient();

            if (Configuration.TryGetAzureAppConfigurationConnectionString(out _))
            {
                services.AddAzureAppConfiguration();
            }
        }

        public static ServerCertificate GetServerCertificate(ICertificateLoader certificateLoader)
        {
            var result = certificateLoader.Load();

            if (result.SerialNumber.EqualsInvariant(ServerCertificate.SerialNumberOfVirtoPredefined) ||
                result.Expired)
            {
                result = ServerCertificateService.CreateSelfSigned();
            }

            return result;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCustomSecurityHeaders();

            //Return all errors as Json response
            app.UseMiddleware<ApiErrorWrappingMiddleware>();

            // Engages the forwarded header support in the pipeline  (see description above)
            app.UseForwardedHeaders();

            app.UseHttpsRedirection();

            if (Configuration.TryGetAzureAppConfigurationConnectionString(out _))
            {
                app.UseAzureAppConfiguration();
            }

            // Add default MimeTypes with additional bindings
            var fileExtensionsBindings = new Dictionary<string, string>
            {
                { ".liquid", "text/html"}, // Allow liquid templates
                { ".page", "text/html"}, // Allow page builder pages
                { ".md", "text/html"} // Allow Markdown documents
            };

            // Create default provider (with default Mime types)
            var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

            // Add custom bindings
            foreach (var binding in fileExtensionsBindings)
            {
                fileExtensionContentTypeProvider.Mappings[binding.Key] = binding.Value;
            }

            var platformOptions = app.ApplicationServices.GetService<IOptions<PlatformOptions>>().Value;

            if (platformOptions.UseResponseCompression)
            {
                app.UseResponseCompression();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = fileExtensionContentTypeProvider
            });

            app.UseRouting();
            app.UseCookiePolicy();

            //Handle all requests like a $(Platform) and Modules/$({ module.ModuleName }) as static files in correspond folder
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(WebHostEnvironment.MapPath("~/js")),
                RequestPath = new PathString("/$(Platform)/Scripts")
            });

            // Enables static file serving with the module and apps options
            app.UseModulesAndAppsFiles();

            app.UseDefaultFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ExecuteSynchronized(() =>
            {
                // This method contents will run inside of critical section of instance distributed lock.
                // Main goal is to apply the migrations (Platform, Hangfire, modules) sequentially instance by instance.
                // This ensures only one active EF-migration ran simultaneously to avoid DB-related side-effects.

                // Apply platform migrations
                app.UsePlatformMigrations(Configuration);

                app.UpdateServerCertificateIfNeed(ServerCertificate);

                app.UseDbTriggers();

                // Register platform settings
                app.UsePlatformSettings();

                // Complete hangfire init and apply Hangfire migrations
                app.UseHangfire(Configuration);

                // Register platform permissions
                app.UsePlatformPermissions();
                app.UseSecurityHandlers();
                app.UsePruneExpiredTokensJob();

                var options = app.ApplicationServices.GetService<IOptions<LockoutOptionsExtended>>();

                app.UseAutoAccountsLockoutJob(options.Value);

                // Complete modules startup and apply their migrations
                ConsoleLog.BeginOperation("Post initializing modules");

                app.UseModules();

                ConsoleLog.EndOperation();
            });

            app.UseEndpoints(SetupEndpoints);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            var mvcJsonOptions = app.ApplicationServices.GetService<IOptions<MvcNewtonsoftJsonOptions>>();

            //Json converter that resolve a meta-data for all incoming objects of  DynamicObjectProperty type
            //in order to be able pass { name: "dynPropName", value: "myVal" } in the incoming requests for dynamic properties, and do not care about meta-data loading. see more details: PT-48
            mvcJsonOptions.Value.SerializerSettings.Converters.Add(new DynamicObjectPropertyJsonConverter(app.ApplicationServices.GetService<IDynamicPropertyMetaDataResolver>()));

            //The converter is responsible for the materialization of objects, taking into account the information on overriding
            mvcJsonOptions.Value.SerializerSettings.Converters.Add(new PolymorphJsonConverter());
            PolymorphJsonConverter.RegisterTypeForDiscriminator(typeof(PermissionScope), nameof(PermissionScope.Type));

            WriteFailedModulesToLog(app, logger);

            logger.LogInformation("Welcome to Virto Commerce {PlatformVersion}!", typeof(Startup).Assembly.GetName().Version);
        }

        private static void WriteFailedModulesToLog(IApplicationBuilder app, ILogger<Startup> logger)
        {
            var localModuleCatalog = app.ApplicationServices.GetService<ILocalModuleCatalog>();

            var failedModules = localModuleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => !x.Errors.IsNullOrEmpty())
                .Select(x => new { x.Id, x.Version, ErrorMessage = string.Join(";", x.Errors) });

            foreach (var failedModule in failedModules)
            {
                logger.LogError("Could not load module {ModuleId} {ModuleVersion}. Error: {ErrorMessage}", failedModule.Id, failedModule.Version, failedModule.ErrorMessage);
            }
        }

        private static void SetupEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            //Setup SignalR hub
            endpoints.MapHub<PushNotificationHub>("/pushNotificationHub");

            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8";

                    var reportJson =
                        JsonConvert.SerializeObject(report.Entries, Formatting.Indented, new StringEnumConverter());
                    await context.Response.WriteAsync(reportJson);
                }
            });
        }
    }
}
