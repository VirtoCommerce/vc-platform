using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Handlers;
using VirtoCommerce.Platform.Security.Services;

namespace VirtoCommerce.Platform.Security.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services, Action<AuthorizationOptions> setupAction = null)
        {
            services.AddScoped<IUserNameResolver, HttpContextUserResolver>();
            services.AddSingleton<IPermissionsRegistrar, DefaultPermissionProvider>();
            services.AddScoped<IRoleSearchService, RoleSearchService>();
            //Register as singleton because this abstraction can be used as dependency in singleton services
            services.AddSingleton<IUserSearchService>(provider => new UserSearchService(provider.CreateScope().ServiceProvider.GetService<Func<UserManager<ApplicationUser>>>()));

            //Identity dependencies override
            services.TryAddScoped<RoleManager<Role>, CustomRoleManager>();
            services.TryAddScoped<UserManager<ApplicationUser>, CustomUserManager>();
            services.AddSingleton<Func<UserManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>());
            //Use custom ClaimsPrincipalFactory to add system roles claims for user principal
            services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton(provider => new LogChangesUserChangedEventHandler(provider.CreateScope().ServiceProvider.GetService<IChangeLogService>()));

            var providerSnapshot = services.BuildServiceProvider();
            var inProcessBus = providerSnapshot.GetService<IHandlerRegistrar>();
            inProcessBus.RegisterHandler<UserChangedEvent>(async (message, token) => await providerSnapshot.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserPasswordChangedEvent>(async (message, token) => await providerSnapshot.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserResetPasswordEvent>(async (message, token) => await providerSnapshot.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserLoginEvent>(async (message, token) => await providerSnapshot.GetService<LogChangesUserChangedEventHandler>().Handle(message));
            inProcessBus.RegisterHandler<UserLogoutEvent>(async (message, token) => await providerSnapshot.GetService<LogChangesUserChangedEventHandler>().Handle(message));

            return services;
        }
    }
}
