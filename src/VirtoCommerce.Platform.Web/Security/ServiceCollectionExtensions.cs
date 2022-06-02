using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Exceptions;
using VirtoCommerce.Platform.Security.Handlers;
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

            services.AddScoped<IUserApiKeyService, UserApiKeyService>();
            services.AddScoped<IUserApiKeySearchService, UserApiKeySearchService>();

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
            services.AddSingleton<Func<RoleManager<Role>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<RoleManager<Role>>());
            services.AddSingleton<Func<UserManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>());
            services.AddSingleton<Func<SignInManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<SignInManager<ApplicationUser>>());
            services.AddSingleton<IUserPasswordHasher, DefaultUserPasswordHasher>();
            //Use custom ClaimsPrincipalFactory to add system roles claims for user principal
            services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton(provider => new LogChangesUserChangedEventHandler(provider.CreateScope().ServiceProvider.GetService<IChangeLogService>()));
            services.AddSingleton(provider => new UserApiKeyActualizeEventHandler(provider.CreateScope().ServiceProvider.GetService<IUserApiKeyService>()));

            services.AddTransient<ICrudService<ServerCertificate>, ServerCertificateService>();

            return services;
        }

        /// <summary>
        /// Get currently installed certificate from the storage
        /// or create new self-signed
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ServerCertificate GetServerCertificate(this IConfiguration configuration)
        {
            var result = SecurityRepository.LoadCurrentlySetServerCertificate(configuration);

            if (result.SerialNumber.EqualsInvariant(ServerCertificate.SerialNumberOfVirtoPredefined) || result.Expired)
            {
                result = ServerCertificateService.CreateSelfSigned();
            }

            return result;
        }

        /// <summary>
        /// Save newly generated (if it so) server certificate to the shared storage.
        /// The call of this method should be under distributed lock between platform instances
        /// to synchronize old certs checks
        /// </summary>
        /// <param name="app"></param>
        /// <param name="currentCert"></param>
        /// <exception cref="PlatformException"></exception>
        public static void UpdateServerCertificateIfNeed(this IApplicationBuilder app, ServerCertificate currentCert)
        {
            var configuration = app.ApplicationServices.GetService<IConfiguration>();
            var certificateService = app.ApplicationServices.GetService<ICrudService<ServerCertificate>>();
            var possiblyOldCert = SecurityRepository.LoadCurrentlySetServerCertificate(configuration); // Previously stored cert (possibly old, default or just created by another platform instance)
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
                    certificateService.DeleteAsync(new string[] { possiblyOldCert.Id }).GetAwaiter().GetResult();
                }

                certificateService.SaveChangesAsync(new ServerCertificate[] { currentCert }).GetAwaiter().GetResult();
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
    }
}
