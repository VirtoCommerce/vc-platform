using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Json;

namespace VirtoCommerce.Platform.Web.Json;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddOutputJsonSerializerSettings(
        this IMvcBuilder builder,
        Action<OutputJsonSerializerSettings, MvcNewtonsoftJsonOptions> setupAction)
    {
        builder.Services.AddSingleton(setupAction);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<OutputJsonSerializerSettings>, ConfigureOutputJsonSerializerSettings>());
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>());

        return builder;
    }
}
