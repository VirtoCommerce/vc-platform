using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Security;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// Drives <see cref="Startup.ConfigureRequestPipeline"/> — the shipped composition of the segment the
/// two IPlatformStartup hooks live in — over a TestServer, and records what a middleware registered
/// through each hook can observe.
/// </summary>
internal sealed class RequestPipelineHarness : IAsyncDisposable
{
    public const string OpenPath = "/api/test/open";
    public const string ProtectedPath = "/api/test/protected";

    public const string OpenPathBody = "harness-open";

    public const string TestScheme = "HarnessScheme";
    public const string CredentialHeader = "X-Harness-User";
    public const string PermissionClaimType = "harness-permission";
    public const string PermissionHeader = "harness-permission";
    public const string RequiredPermission = "harness:allowed";
    public const string RequiredPolicy = "HarnessPolicy";

    private readonly string _contentRoot;
    private readonly ModuleBootstrapper _previousBootstrapper;

    // Assigned once, after StartAsync has built the host: the probe startups must already hold a
    // reference to THIS instance while the host is being built, because Configure runs during
    // HostBuilder.StartAsync and registers the recording middlewares against it.
    private IHost _host;

    private RequestPipelineHarness(string contentRoot, ModuleBootstrapper previousBootstrapper)
    {
        _contentRoot = contentRoot;
        _previousBootstrapper = previousBootstrapper;
    }

    public HookObservations AfterRouting { get; } = new();

    public HookObservations AfterAuthentication { get; } = new();

    public static async Task<RequestPipelineHarness> StartAsync(IPlatformStartup extraStartup = null)
    {
        var contentRoot = Path.Combine(Path.GetTempPath(), "vc-pipeline-harness-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(contentRoot, "js"));

        var previousBootstrapper = ModuleBootstrapper.Instance;
        var bootstrapper = new ModuleBootstrapper(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());
        ModuleBootstrapper.Instance = bootstrapper;

        var harness = new RequestPipelineHarness(contentRoot, previousBootstrapper);
        bootstrapper.Startups.Add(new RecordingPlatformStartup(harness));
        if (extraStartup != null)
        {
            bootstrapper.Startups.Add(extraStartup);
        }

        try
        {
            // CreateDefaultBuilder + ConfigureWebHostDefaults, because that is what Program.cs:88 does:
            // only the *Defaults* form registers ForwardedHeadersStartupFilter, which owns the effective
            // UseForwardedHeaders() call in production. Under a plain HostBuilder.ConfigureWebHost the
            // segment's own call would become the effective one and
            // ConfigureAfterRouting_OnAForwardedRequest_SeesTheResolvedClientAddress would prove the
            // wrong pipeline.
            var host = await Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                .UseTestServer()
                .UseContentRoot(contentRoot)
                .UseWebRoot(contentRoot)
                .ConfigureServices(services =>
                {
                    services.AddOptions();
                    services.Configure<PlatformOptions>(_ => { });
                    services.AddRouting();

                    // The platform's own call, not a copy of what it configures: it reads
                    // ASPNETCORE_FORWARDEDHEADERS_ENABLED eagerly, which PlatformPipelineCollection's
                    // fixture sets for every class in the collection.
                    services.AddForwardedHeaders();

                    services.AddAuthentication(TestScheme)
                        .AddScheme<AuthenticationSchemeOptions, HarnessAuthenticationHandler>(TestScheme, _ => { });

                    services.AddAuthorizationBuilder()
                        .AddPolicy(RequiredPolicy, policy => policy.RequireClaim(PermissionClaimType, RequiredPermission));
                })
                .Configure(app =>
                {
                    Startup.ConfigureRequestPipeline(app, new ConfigurationBuilder().Build(), app.ApplicationServices.GetRequiredService<IWebHostEnvironment>());

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet(OpenPath, context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            return context.Response.WriteAsync(OpenPathBody);
                        });

                        endpoints.MapGet(ProtectedPath, context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            return Task.CompletedTask;
                        }).RequireAuthorization(RequiredPolicy);
                    });
                }))
                .StartAsync();

            harness._host = host;

            return harness;
        }
        catch
        {
            // Nothing is returned on this path, so no `await using` will ever run: the singleton and the
            // temp directory have to be released here or one failing test poisons every later one in the
            // collection - and turns one real defect into a wall of unrelated red.
            ModuleBootstrapper.Instance = previousBootstrapper;
            DeleteQuietly(contentRoot);

            throw;
        }
    }

    public Task<HttpContext> SendAsync(Action<HttpContext> configure)
    {
        return _host.GetTestServer().SendAsync(configure);
    }

    public static async Task<string> ReadBodyAsync(HttpContext context)
    {
        var body = context.Response.Body;
        if (body.CanSeek)
        {
            body.Position = 0;
        }

        using var reader = new StreamReader(body, leaveOpen: true);

        return await reader.ReadToEndAsync();
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
        }
        finally
        {
            // finally, not sequential: a throw out of StopAsync or Dispose must not leave the process
            // global pointing at this harness's bootstrapper.
            ModuleBootstrapper.Instance = _previousBootstrapper;
            DeleteQuietly(_contentRoot);
        }
    }

    private static void DeleteQuietly(string path)
    {
        try
        {
            Directory.Delete(path, recursive: true);
        }
        catch (IOException)
        {
            // A leftover temp directory is not worth failing a test over.
        }
        catch (UnauthorizedAccessException)
        {
            // Same.
        }
    }

    private sealed class RecordingPlatformStartup : IPlatformStartup
    {
        private readonly RequestPipelineHarness _harness;

        public RecordingPlatformStartup(RequestPipelineHarness harness)
        {
            _harness = harness;
        }

        public void ConfigureAppConfiguration(IConfigurationBuilder builder, IHostEnvironment env) { }

        public void ConfigureHostServices(IServiceCollection services, IConfiguration config) { }

        public void ConfigureServices(IServiceCollection services, IConfiguration config) { }

        public void Configure(IApplicationBuilder app, IConfiguration config) { }

        public void ConfigureAfterRouting(IApplicationBuilder app, IConfiguration config)
        {
            app.Use(async (context, next) =>
            {
                _harness.AfterRouting.Record(context);
                await next();
            });
        }

        public void ConfigureAfterAuthentication(IApplicationBuilder app, IConfiguration config)
        {
            app.Use(async (context, next) =>
            {
                _harness.AfterAuthentication.Record(context);
                await next();
            });
        }
    }

    private sealed class HarnessAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HarnessAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(CredentialHeader, out var userNames) || userNames.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userNames[0]) };
            if (Request.Headers.ContainsKey(PermissionHeader))
            {
                claims.Add(new Claim(PermissionClaimType, RequiredPermission));
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, TestScheme));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, TestScheme)));
        }
    }
}

/// <summary>
/// Every observation is nullable and starts null, so "the hook never ran" cannot be read as an
/// observed false.
/// </summary>
internal sealed class HookObservations
{
    public int Invocations { get; private set; }

    public bool? EndpointMatched { get; private set; }

    public bool? AuthenticateResultFeaturePresent { get; private set; }

    public bool? AuthenticateSucceeded { get; private set; }

    public IPAddress RemoteIpAddress { get; private set; }

    public void Record(HttpContext context)
    {
        Invocations++;
        EndpointMatched = context.GetEndpoint() != null;
        RemoteIpAddress = context.Connection.RemoteIpAddress;

        // IAuthenticateResultFeature, not HttpContext.User: the framework sets the feature only under
        // result.Succeeded, while context.User can be assigned by anything downstream.
        var feature = context.Features.Get<IAuthenticateResultFeature>();
        AuthenticateResultFeaturePresent = feature != null;
        AuthenticateSucceeded = feature?.AuthenticateResult?.Succeeded;
    }
}
