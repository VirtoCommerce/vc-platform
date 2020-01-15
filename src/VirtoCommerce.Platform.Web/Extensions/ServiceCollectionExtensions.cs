using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.External;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Services;
using VirtoCommerce.Platform.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VirtoCommerce.Platform.Security.Handlers;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Web.Security;

namespace VirtoCommerce.Platform.Modules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModules(this IServiceCollection services, IMvcBuilder mvcBuilder, Action<LocalStorageModuleCatalogOptions> setupAction = null)
        {
            services.AddSingleton(services);

            services.AddSingleton<IModuleInitializer, ModuleInitializer>();
            // Cannot inject IHostingEnvironment to LoadContextAssemblyResolver as IsDevelopment() is an extension method (means static) and cannot be mocked by Moq in tests
            services.AddSingleton<IAssemblyResolver, LoadContextAssemblyResolver>(provider =>
                new LoadContextAssemblyResolver(provider.GetService<ILogger<LoadContextAssemblyResolver>>(), provider.GetService<IWebHostEnvironment>().IsDevelopment()));
            services.AddSingleton<IModuleManager, ModuleManager>();
            services.AddSingleton<ILocalModuleCatalog, LocalStorageModuleCatalog>();
            services.AddSingleton<IModuleCatalog>(provider => provider.GetService<ILocalModuleCatalog>());

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            var providerSnapshot = services.BuildServiceProvider();

            var manager = providerSnapshot.GetRequiredService<IModuleManager>();
            var moduleCatalog = providerSnapshot.GetRequiredService<ILocalModuleCatalog>();

            manager.Run();
            // Ensure all modules are loaded
            foreach (var module in moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.State == ModuleState.NotStarted).ToArray())
            {
                manager.LoadModule(module.ModuleName);
                if (module.Assembly != null)
                {
                    // Register API controller from modules
                    mvcBuilder.AddApplicationPart(module.Assembly);
                }
            }

            services.AddSingleton(moduleCatalog);
            return services;
        }

        public static IServiceCollection AddExternalModules(this IServiceCollection services, Action<ExternalModuleCatalogOptions> setupAction = null)
        {
            services.AddSingleton<IExternalModulesClient, ExternalModulesClient>();
            services.AddSingleton<IExternalModuleCatalog, ExternalModuleCatalog>();
            services.AddSingleton<IModuleInstaller, ModuleInstaller>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton<IPlatformRestarter, ProcessPlatformRestarter>();

            return services;
        }

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
