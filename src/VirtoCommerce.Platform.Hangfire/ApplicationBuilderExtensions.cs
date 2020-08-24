using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;
using VirtoCommerce.Platform.Hangfire.Suspend;

namespace VirtoCommerce.Platform.Hangfire
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuthorizationHandler() } });

            var mvcJsonOptions = appBuilder.ApplicationServices.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
            GlobalConfiguration.Configuration.UseSerializerSettings(mvcJsonOptions.Value.SerializerSettings);

            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            var recurringJobManager = appBuilder.ApplicationServices.GetService<IRecurringJobManager>();
            var settingsManager = appBuilder.ApplicationServices.GetService<ISettingsManager>();
            inProcessBus.RegisterHandler<ObjectSettingChangedEvent>(async (message, token) => await recurringJobManager.HandleSettingChangeAsync(settingsManager, message));

            appBuilder.ApplicationServices.GetRequiredService<IHangfireStartSuspend>().Suspend = false; // Resume hangfire jobs working

            return appBuilder;
        }
    }
}
