using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePlatformSettings(this IApplicationBuilder appBuilder)
        {
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(PlatformConstants.Settings.AllSettings, "Platform");
            settingsRegistrar.RegisterSettingsForType(PlatformConstants.Settings.UserProfile.AllSettings,
                typeof(PlatformConstants.Settings.UserProfile).Name);

            return appBuilder;
        }

        public static IApplicationBuilder UseModules(this IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var moduleManager = serviceScope.ServiceProvider.GetRequiredService<IModuleManager>();
                var modules = GetInstalledModules(serviceScope.ServiceProvider);
                foreach (var module in modules)
                {
                    moduleManager.PostInitializeModule(module, appBuilder);
                }
            }
            return appBuilder;
        }

        private static IEnumerable<ManifestModuleInfo> GetInstalledModules(IServiceProvider serviceProvider)
        {
            var moduleCatalog = serviceProvider.GetRequiredService<ILocalModuleCatalog>();
            var allModules = moduleCatalog.Modules.OfType<ManifestModuleInfo>()
                                          .Where(x => x.State == ModuleState.Initialized && !x.Errors.Any())
                                          .ToArray();

            return moduleCatalog.CompleteListWithDependencies(allModules)
                .OfType<ManifestModuleInfo>()
                .Where(x => x.State == ModuleState.Initialized && !x.Errors.Any())
                .ToArray();
        }

        public static IApplicationBuilder UsePlatformPermissions(this IApplicationBuilder appBuilder)
        {
            //Register PermissionScope type itself to prevent Json serialization error due that fact that will be taken from other derived from PermissionScope type (first in registered types list) in PolymorphJsonContractResolver
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScope>();

            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(PlatformConstants.Security.Permissions.AllPermissions.Select(x => new Permission() { GroupName = "Platform", Name = x }).ToArray());
            return appBuilder;
        }


        public static async Task<IApplicationBuilder> UseDefaultUsersAsync(this IApplicationBuilder appBuilder)
        {
            using (var scope = appBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                if (await userManager.FindByNameAsync("admin") == null)
                {
                    var admin = new ApplicationUser
                    {
                        Id = "1eb2fa8ac6574541afdb525833dadb46",
                        IsAdministrator = true,
                        UserName = "admin",
                        PasswordHash = "AHQSmKnSLYrzj9vtdDWWnUXojjpmuDW2cHvWloGL9UL3TC9UCfBmbIuR2YCyg4BpNg==",
                        PasswordExpired = true
                    };
                    var adminUser = await userManager.FindByIdAsync(admin.Id);
                    if (adminUser == null)
                    {
                        await userManager.CreateAsync(admin);
                    }
                }

            }
            return appBuilder;
        }
    }

}
