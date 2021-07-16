using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Assets.AzureBlobStorage;
using VirtoCommerce.Platform.Assets.AzureBlobStorage.Extensions;
using VirtoCommerce.Platform.Assets.FileSystem;
using VirtoCommerce.Platform.Assets.FileSystem.Extensions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.Hangfire.Extensions;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Infrastructure;
using VirtoCommerce.Platform.Web.Licensing;
using VirtoCommerce.Platform.Web.Middleware;
using VirtoCommerce.Platform.Web.Migrations;
using VirtoCommerce.Platform.Web.PushNotifications;
using VirtoCommerce.Platform.Web.Redis;
using VirtoCommerce.Platform.Web.Swagger;
using VirtoCommerce.Platform.Web.Telemetry;
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
            // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1#forward-the-scheme-for-linux-and-non-iis-reverse-proxies
            if (string.Equals(
                Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"),
                "true", StringComparison.OrdinalIgnoreCase))
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }

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
            //Get platform version from GetExecutingAssembly
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);

            services.AddPlatformServices(Configuration);
            services.AddSingleton<LicenseProvider>();

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
                    //Next line allow to use polymorph types as parameters in API controller methods
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
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


            // Support commonly used forwarded headers
            // X-Forwarded-For - Holds Client IP (optionally port number) across proxies and ends up in HttpContext.Connection.RemoteIpAddress
            // X-Forwarded-Proto - Holds original scheme (HTTP or HTTPS) even if call traversed proxies and changed and ends up in HttpContext.Request.Scheme
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Clear();
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor;
            });


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

            // The following line enables Application Insights telemetry collection.
            // CAUTION: It is important to keep the adding AI telemetry in the end of ConfigureServices method in order to avoid of multiple
            // AI modules initialization https://virtocommerce.atlassian.net/browse/VP-6653 and  https://github.com/microsoft/ApplicationInsights-dotnet/issues/2114 until we don't
            // get rid of calling IServiceCollection.BuildServiceProvider from the platform and modules code, each BuildServiceProvider call leads to the running the
            // extra AI module and causes the hight CPU utilization and telemetry data flood on production env.
            services.AddAppInsightsTelemetry(Configuration);

            services.AddHealthChecks();
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
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(WebHostEnvironment.MapPath("~/js")),
            //    RequestPath = new PathString($"/$(Platform)/Scripts")
            //});

            var localModules = app.ApplicationServices.GetRequiredService<ILocalModuleCatalog>().Modules;
            foreach (var module in localModules.OfType<ManifestModuleInfo>())
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(module.FullPhysicalPath),
                    RequestPath = new PathString($"/modules/$({ module.ModuleName })")
                });
                //TODO: DC load aliases from manifest
                if(module.ModuleName == "VirtoCommerce.Manager")
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(module.FullPhysicalPath, "scripts", "js")),
                        RequestPath = new PathString($"/$(Platform)/Scripts")
                    });

                }
            }

            app.UseDefaultFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ExecuteSynhronized(() =>
            {
                // This method contents will run inside of critical section of instance distributed lock.
                // Main goal is to apply the migrations (Platform, Hangfire, modules) sequentially instance by instance.
                // This ensures only one active EF-migration ran simultaneously to avoid DB-related side-effects.

                // Apply platform migrations
                app.UsePlatformMigrations();

                app.UseDbTriggers();

                // Register platform settings
                app.UsePlatformSettings();

                // Complete hangfire init and apply Hangfire migrations
                app.UseHangfire(Configuration);

                // Complete modules startup and apply their migrations
                app.UseModules();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                //Setup SignalR hub
                endpoints.MapHub<PushNotificationHub>("/pushNotificationHub");

                endpoints.MapHealthChecks();
            });

       
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Use app insights telemetry
            app.UseAppInsightsTelemetry();

            var mvcJsonOptions = app.ApplicationServices.GetService<IOptions<MvcNewtonsoftJsonOptions>>();

            //Json converter that resolve a meta-data for all incoming objects of  DynamicObjectProperty type
            //in order to be able pass { name: "dynPropName", value: "myVal" } in the incoming requests for dynamic properties, and do not care about meta-data loading. see more details: PT-48
            mvcJsonOptions.Value.SerializerSettings.Converters.Add(new DynamicObjectPropertyJsonConverter(app.ApplicationServices.GetService<IDynamicPropertyMetaDataResolver>()));

            //The converter is responsible for the materialization of objects, taking into account the information on overriding
            mvcJsonOptions.Value.SerializerSettings.Converters.Add(new PolymorphJsonConverter());
            PolymorphJsonConverter.RegisterTypeForDiscriminator(typeof(PermissionScope), nameof(PermissionScope.Type));
        }
    }
}
