using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSignalR(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices;
            var hubJsonOptions = services.GetService<IOptions<NewtonsoftJsonHubProtocolOptions>>().Value;
            var mvcJsonOptions = services.GetService<IOptions<MvcNewtonsoftJsonOptions>>().Value;
            mvcJsonOptions.SerializerSettings.CopyTo(hubJsonOptions.PayloadSerializerSettings);
            hubJsonOptions.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            hubJsonOptions.PayloadSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            return appBuilder;
        }
    }
}
