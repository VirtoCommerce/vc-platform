using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security
{
    public static class ApplicationBuilderExtensions
    {
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
                        var result = await userManager.CreateAsync(admin);
                    }
                }

            }
            return appBuilder;
        }
    }
}
