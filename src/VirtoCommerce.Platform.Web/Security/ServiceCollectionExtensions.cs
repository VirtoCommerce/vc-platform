using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Exceptions;
using VirtoCommerce.Platform.Security.Handlers;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;

namespace VirtoCommerce.Platform.Web.Security
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services, Action<AuthorizationOptions> setupAction = null)
        {
            services.AddTransient<ISecurityRepository, SecurityRepository>();
            services.AddTransient<Func<ISecurityRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetService<ISecurityRepository>());

            services.AddSingleton<IUserApiKeyService, UserApiKeyService>();
            services.AddSingleton<IUserApiKeySearchService, UserApiKeySearchService>();

            services.AddScoped<IUserNameResolver, HttpContextUserResolver>();
            services.AddSingleton<IPermissionsRegistrar, DefaultPermissionProvider>();
            services.AddScoped<IRoleSearchService, RoleSearchService>();

            //Register as singleton because this abstraction can be used as dependency in singleton services
            services.AddSingleton<IUserSearchService>(provider =>
            {
                var scope = provider.CreateScope();
                return new UserSearchService(scope.ServiceProvider.GetService<Func<UserManager<ApplicationUser>>>(),
                                             scope.ServiceProvider.GetService<Func<RoleManager<Role>>>());
            });

            //Identity dependencies override
            services.TryAddScoped<RoleManager<Role>, CustomRoleManager>();
            services.TryAddScoped<UserManager<ApplicationUser>, CustomUserManager>();
            services.TryAddScoped<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            services.TryAddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();
            services.TryAddScoped<IUserStore<ApplicationUser>, CustomUserStore>();
            services.AddSingleton<Func<RoleManager<Role>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<RoleManager<Role>>());
            services.AddSingleton<Func<UserManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>());
            services.AddSingleton<Func<SignInManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<SignInManager<ApplicationUser>>());
            //Use custom ClaimsPrincipalFactory to add system roles claims for user principal
            services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
            services.AddTransient<ITokenRequestValidator, BaseUserSignInValidator>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton<LogChangesUserChangedEventHandler>();
            services.AddSingleton<UserApiKeyActualizeEventHandler>();

            services.AddTransient<IServerCertificateService, ServerCertificateService>();

            return services;
        }

        /// <summary>
        /// Save newly generated (if it so) server certificate to the shared storage.
        /// The call of this method should be under distributed lock between platform instances
        /// to synchronize old certs checks
        /// </summary>
        /// <param name="app"></param>
        /// <param name="currentCert"></param>
        /// <exception cref="ServerCertificateReplacedException"></exception>
        public static void UpdateServerCertificateIfNeed(this IApplicationBuilder app, ServerCertificate currentCert)
        {
            var certificateStorage = app.ApplicationServices.GetService<ICertificateLoader>();
            var certificateService = app.ApplicationServices.GetService<IServerCertificateService>();
            var possiblyOldCert = certificateStorage.Load(); // Previously stored cert (possibly old, default or just created by another platform instance)
            if (!possiblyOldCert.SerialNumber.EqualsInvariant(currentCert.SerialNumber))
            {   // Therefore, currentCert is newly generated and needs to be saved.
                // But we should check if old cert is stored
                if (possiblyOldCert.StoredInDb && !possiblyOldCert.Expired)
                {
                    throw new ServerCertificateReplacedException("The other starting up instance of the platform replaced server certificate. Current instance should be restarted to use new server certificate.");
                }

                if (!string.IsNullOrEmpty(possiblyOldCert.Id))
                {
                    // Delete old certificate in case of one exists (the Id of newly generated certs is null)
                    certificateService.DeleteAsync(new[] { possiblyOldCert.Id }).GetAwaiter().GetResult();
                }

                certificateService.SaveChangesAsync(new[] { currentCert }).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Configure Forwarded Headers to support custom load balancer
        /// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-6.0
        /// </summary>
        /// <param name="services"></param>
        public static void AddForwardedHeaders(this IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-6.0
            if (string.Equals(
                Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"),
                "true", StringComparison.OrdinalIgnoreCase))
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.All;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }
        }

        public static void UseCustomSecurityHeaders(this IApplicationBuilder app)
        {
            var policies = new HeaderPolicyCollection().AddDefaultSecurityHeaders();
            var options = app.ApplicationServices.GetService<IOptions<SecurityHeadersOptions>>().Value;

            if (options.FrameOptions.EqualsIgnoreCase("SameOrigin"))
            {
                policies.AddFrameOptionsSameOrigin();
            }
            else if (options.FrameOptions.EqualsIgnoreCase("Deny"))
            {
                policies.AddFrameOptionsDeny();
            }
            else if (!string.IsNullOrEmpty(options.FrameOptions))
            {
                policies.AddFrameOptionsSameOrigin(options.FrameOptions);
            }

            policies.AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().None();
                builder.AddFormAction().Self();

                if (options.FrameAncestors.EqualsIgnoreCase("None"))
                {
                    builder.AddFrameAncestors().None();
                }
                else if (options.FrameAncestors.EqualsIgnoreCase("Self"))
                {
                    builder.AddFrameAncestors().Self();
                }
                else if (!string.IsNullOrEmpty(options.FrameAncestors))
                {
                    builder.AddFrameAncestors().From(options.FrameAncestors);
                }
            });

            app.UseSecurityHeaders(policies);
        }
    }
}
