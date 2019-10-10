using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
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
    }

}
