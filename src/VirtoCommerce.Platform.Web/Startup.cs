using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Assets.AzureBlobStorage;
using VirtoCommerce.Platform.Assets.AzureBlobStorage.Extensions;
using VirtoCommerce.Platform.Assets.FileSystem;
using VirtoCommerce.Platform.Assets.FileSystem.Extensions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Hangfire.Extensions;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Security.Authorization;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using VirtoCommerce.Platform.Web.Azure;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Infrastructure;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Middleware;
using VirtoCommerce.Platform.Web.PushNotifications;
using VirtoCommerce.Platform.Web.Redis;
using VirtoCommerce.Platform.Web.Security;
using VirtoCommerce.Platform.Web.Security.Authentication;
using VirtoCommerce.Platform.Web.Security.Authorization;
using VirtoCommerce.Platform.Web.SignalR;
using VirtoCommerce.Platform.Web.Swagger;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace VirtoCommerce.Platform.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // This custom provider allows able to use just [Authorize] instead of having to define [Authorize(AuthenticationSchemes = "Bearer")] above every API controller
            // without this Bearer authorization will not work
            services.AddSingleton<IAuthenticationSchemeProvider, CustomAuthenticationSchemeProvider>();

            services.AddRedis(Configuration);

            services.AddSignalR().AddPushNotifications(Configuration);

            services.AddOptions<PlatformOptions>().Bind(Configuration.GetSection("VirtoCommerce")).ValidateDataAnnotations();
            services.AddOptions<TranslationOptions>().Configure(options =>
            {
                options.PlatformTranslationFolderPath = WebHostEnvironment.MapPath(options.PlatformTranslationFolderPath);
            });
            //Get platform version from GetExecutingAssembly
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);

            services.AddPlatformServices(Configuration);
            services.AddSecurityServices();
            services.AddSingleton<LicenseProvider>();

            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<IgnoreSignalRTelemetryProcessor>();

            var mvcBuilder = services.AddMvc(mvcOptions =>
                {
                    //Disable 204 response for null result. https://github.com/aspnet/AspNetCore/issues/8847
                    var noContentFormatter = mvcOptions.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
                    if (noContentFormatter != null)
                    {
                        noContentFormatter.TreatNullValueAsNoContent = false;
                    }
                }
            )
            .AddNewtonsoftJson(options =>
                {
                    //Next line needs to represent custom derived types in the resulting swagger doc definitions. Because default SwaggerProvider used global JSON serialization settings
                    //we should register this converter globally.
                    options.SerializerSettings.ContractResolver = new PolymorphJsonContractResolver();
                    //Next line allow to use polymorph types as parameters in API controller methods
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.Converters.Add(new PolymorphJsonConverter());
                    options.SerializerSettings.Converters.Add(new ModuleIdentityJsonConverter());
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.None;
                }
            );

            services.AddSingleton(js =>
            {
                var serv = js.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
                return JsonSerializer.Create(serv.Value.SerializerSettings);
            });

            services.AddDbContext<SecurityDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("VirtoCommerce"));
                options.UseDatabaseProviderSwitcher(Configuration).SetConnectionName(Configuration, "VirtoCommerce");
                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });

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
                                      .AddCookie();

            services.AddSecurityServices(options =>
            {
            });

            services.AddIdentity<ApplicationUser, Role>(options => options.Stores.MaxLengthForKeys = 128)
                    .AddEntityFrameworkStores<SecurityDbContext>()
                    .AddDefaultTokenProviders();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Support commonly used forwarded headers
            // X-Forwarded-For - Holds Client IP (optionally port number) across proxies and ends up in HttpContext.Connection.RemoteIpAddress
            // X-Forwarded-Proto - Holds original scheme (HTTP or HTTPS) even if call traversed proxies and changed and ends up in HttpContext.Request.Scheme
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Clear();
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor;
            });

            //Create backup of token handler before default claim maps are cleared
            var defaultTokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            authBuilder.AddJwtBearer(options =>
                    {
                        options.Authority = Configuration["Auth:Authority"];
                        options.Audience = Configuration["Auth:Audience"];

                        if (WebHostEnvironment.IsDevelopment())
                        {
                            options.RequireHttpsMetadata = false;
                        }

                        options.IncludeErrorDetails = true;

                        X509SecurityKey publicKey = null;
                        if (!Configuration["Auth:PublicCertPath"].IsNullOrEmpty())
                        {
                            var publicCert = new X509Certificate2(Configuration["Auth:PublicCertPath"]);
                            publicKey = new X509SecurityKey(publicCert);
                        }

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            NameClaimType = OpenIdConnectConstants.Claims.Subject,
                            RoleClaimType = OpenIdConnectConstants.Claims.Role,
                            ValidateIssuer = !string.IsNullOrEmpty(options.Authority),
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = publicKey
                        };
                    });

            var azureAdSection = Configuration.GetSection("AzureAd");

            if (azureAdSection.GetChildren().Any())
            {
                var options = new AzureAdOptions();
                azureAdSection.Bind(options);

                if (options.Enabled)
                {
                    //TODO: Need to check how this influence to OpennIddict Reference tokens activated by this line below  AddValidation(options => options.UseReferenceTokens());
                    authBuilder.AddOpenIdConnect(options.AuthenticationType, options.AuthenticationCaption,
                        openIdConnectOptions =>
                        {
                            openIdConnectOptions.ClientId = options.ApplicationId;
                            openIdConnectOptions.Authority = $"{options.AzureAdInstance}{options.TenantId}";
                            openIdConnectOptions.UseTokenLifetime = true;
                            openIdConnectOptions.RequireHttpsMetadata = false;
                            openIdConnectOptions.SignInScheme = IdentityConstants.ExternalScheme;
                            openIdConnectOptions.SecurityTokenValidator = defaultTokenHandler;
                        });
                }
            }

            services.AddOptions<Core.Security.AuthorizationOptions>().Bind(Configuration.GetSection("Authorization")).ValidateDataAnnotations();
            var authorizationOptions = Configuration.GetSection("Authorization").Get<Core.Security.AuthorizationOptions>();
            var platformOptions = Configuration.GetSection("VirtoCommerce").Get<PlatformOptions>();
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
                    options.UseMvc();

                    // Enable the authorization, logout, token and userinfo endpoints.
                    options.EnableTokenEndpoint("/connect/token")
                        .EnableUserinfoEndpoint("/api/security/userinfo");

                    // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
                    // can enable the other flows if you need to support implicit or client credentials.
                    options.AllowPasswordFlow()
                        .AllowRefreshTokenFlow()
                        .AllowClientCredentialsFlow();

                    options.SetRefreshTokenLifetime(authorizationOptions?.RefreshTokenLifeTime);
                    options.SetAccessTokenLifetime(authorizationOptions?.AccessTokenLifeTime);

                    options.AcceptAnonymousClients();

                    // Configure Openiddict to issues new refresh token for each token refresh request.
                    options.UseRollingTokens();

                    // Make the "client_id" parameter mandatory when sending a token request.
                    //options.RequireClientIdentification();

                    // When request caching is enabled, authorization and logout requests
                    // are stored in the distributed cache by OpenIddict and the user agent
                    // is redirected to the same page with a single parameter (request_id).
                    // This allows flowing large OpenID Connect requests even when using
                    // an external authentication provider like Google, Facebook or Twitter.
                    options.EnableRequestCaching();

                    options.DisableScopeValidation();

                    // During development or when you explicitly run the platform in production mode without https, need to disable the HTTPS requirement.
                    if (WebHostEnvironment.IsDevelopment() || platformOptions.AllowInsecureHttp || !Configuration.IsHttpsServerUrlSet())
                    {
                        options.DisableHttpsRequirement();
                    }

                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    options.UseJsonWebTokens();

                    var bytes = File.ReadAllBytes(Configuration["Auth:PrivateKeyPath"]);
                    X509Certificate2 privateKey;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        // https://github.com/dotnet/corefx/blob/release/2.2/Documentation/architecture/cross-platform-cryptography.md
                        // macOS cannot load certificate private keys without a keychain object, which requires writing to disk. Keychains are created automatically for PFX loading, and are deleted when no longer in use. Since the X509KeyStorageFlags.EphemeralKeySet option means that the private key should not be written to disk, asserting that flag on macOS results in a PlatformNotSupportedException.
                        privateKey = new X509Certificate2(bytes, Configuration["Auth:PrivateKeyPassword"], X509KeyStorageFlags.MachineKeySet);
                    }
                    else
                    {
                        privateKey = new X509Certificate2(bytes, Configuration["Auth:PrivateKeyPassword"], X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
                    }
                    options.AddSigningCertificate(privateKey);
                });

            services.Configure<IdentityOptions>(Configuration.GetSection("IdentityOptions"));

            //always  return 401 instead of 302 for unauthorized  requests
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                //We need this policy because it is a single way to implicitly use the two schema (JwtBearer and ApiKey)  authentication for resource based authorization.
                var mutipleSchemaAuthPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationOptions.DefaultScheme)
                                                                              .RequireAuthenticatedUser()
                                                                              .Build();
                //The good article is described the meaning DefaultPolicy and FallbackPolicy
                //https://scottsauber.com/2020/01/20/globally-require-authenticated-users-by-default-using-fallback-policies-in-asp-net-core/
                options.DefaultPolicy = mutipleSchemaAuthPolicy;
            });
            // register the AuthorizationPolicyProvider which dynamically registers authorization policies for each permission defined in module manifest
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            //Platform authorization handler for policies based on permissions
            services.AddSingleton<IAuthorizationHandler, DefaultPermissionAuthorizationHandler>();
            // Default password validation service implementation
            services.AddScoped<IPasswordCheckService, PasswordCheckService>();

            services.AddOptions<LocalStorageModuleCatalogOptions>().Bind(Configuration.GetSection("VirtoCommerce"))
                    .PostConfigure(options =>
                     {
                         options.DiscoveryPath = Path.GetFullPath(options.DiscoveryPath ?? "modules");
                     })
                    .ValidateDataAnnotations();
            services.AddModules(mvcBuilder);

            services.AddOptions<ExternalModuleCatalogOptions>().Bind(Configuration.GetSection("ExternalModules")).ValidateDataAnnotations();
            services.AddExternalModules();

            //Assets
            var assetsProvider = Configuration.GetSection("Assets:Provider").Value;
            if (assetsProvider.EqualsInvariant(AzureBlobProvider.ProviderName))
            {
                services.AddOptions<AzureBlobOptions>().Bind(Configuration.GetSection("Assets:AzureBlobStorage")).ValidateDataAnnotations();
                services.AddAzureBlobProvider();
            }
            else
            {
                services.AddOptions<FileSystemBlobOptions>().Bind(Configuration.GetSection("Assets:FileSystem"))
                      .PostConfigure(options =>
                      {
                          options.RootPath = WebHostEnvironment.MapPath(options.RootPath);
                      }).ValidateDataAnnotations();

                services.AddFileSystemBlobProvider();
            }

            //HangFire
            services.AddHangfire(Configuration);

            // Register the Swagger generator
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
#if DEBUG
                TelemetryDebugWriter.IsTracingDisabled = true;
#endif
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //Return all errors as Json response
            app.UseMiddleware<ApiErrorWrappingMiddleware>();

            // Engages the forwarded header support in the pipeline  (see description above)
            app.UseForwardedHeaders();

            app.UseHttpsRedirection();

            // Add default MimeTypes with additional bindings
            var fileExtensionsBindings = new Dictionary<string, string>()
            {
                { ".liquid", "text/html"},
                { ".md", "text/html"}
            };

            // Create default provider (with default Mime types)
            var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

            // Add custom bindings
            foreach (var binding in fileExtensionsBindings)
            {
                fileExtensionContentTypeProvider.Mappings[binding.Key] = binding.Value;
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = fileExtensionContentTypeProvider
            });

            app.UseRouting();
            app.UseCookiePolicy();

            //Handle all requests like a $(Platform) and Modules/$({ module.ModuleName }) as static files in correspond folder
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(WebHostEnvironment.MapPath("~/js")),
                RequestPath = new PathString($"/$(Platform)/Scripts")
            });

            var localModules = app.ApplicationServices.GetRequiredService<ILocalModuleCatalog>().Modules;
            foreach (var module in localModules.OfType<ManifestModuleInfo>())
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(module.FullPhysicalPath),
                    RequestPath = new PathString($"/modules/$({ module.ModuleName })")
                });
            }

            app.UseDefaultFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Force migrations
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var platformDbContext = serviceScope.ServiceProvider.GetRequiredService<PlatformDbContext>();
                platformDbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName("Platform"));
                platformDbContext.Database.MigrateIfRelationalDatabase();

                var securityDbContext = serviceScope.ServiceProvider.GetRequiredService<SecurityDbContext>();
                securityDbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName("Security"));
                securityDbContext.Database.MigrateIfRelationalDatabase();
            }

            app.UseDbTriggers();
            //Register platform settings
            app.UsePlatformSettings();

            // Complete hangfire init
            app.UseHangfire(Configuration);

            app.UseModules();

            //Register platform permissions
            app.UsePlatformPermissions();

            //Setup SignalR hub
            app.UseEndpoints(routes =>
            {
                routes.MapHub<PushNotificationHub>("/pushNotificationHub");
            });

            //Seed default users
            app.UseDefaultUsersAsync().GetAwaiter().GetResult();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
        }
    }
}
