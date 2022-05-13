using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security;
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
            services.AddTransient<ISearchService<ServerCertificateSearchCriteria, ServerCertificateSearchResult, ServerCertificate>, ServerCertificateSearchService>();

            return services;
        }

        public static ServerCertificate AddServerCertificate(this IServiceCollection services, IConfiguration Configuration)
        {

            var authDbConnString = Configuration["Auth:ConnectionString"] ?? Configuration.GetConnectionString("VirtoCommerce");

            var result = ServerCertificateService.GetWithoutEf(authDbConnString);
            if (!result.StoredInDb)
            { // Read certificate bytes from the files
                var publicCertPath = Configuration["Auth:PublicCertPath"];
                if (string.IsNullOrEmpty(publicCertPath)) throw new PlatformException("The path to the public cert should be provided ('Auth:PublicCertPath' key in the settings)");
                var privateKeyPath = Configuration["Auth:PrivateKeyPath"];
                if (string.IsNullOrEmpty(privateKeyPath)) throw new PlatformException("The path to the private key pfx should be provided ('Auth:PrivateKeyPath' key in the settings)");
                result.PrivateKeyCertPassword = Configuration["Auth:PrivateKeyPassword"];
                result.PublicCertBytes = File.ReadAllBytes(publicCertPath);
                result.PrivateKeyCertBytes = File.ReadAllBytes(privateKeyPath);
            }

            services.AddSingleton(result);

            return result;
        }
    }
}
