using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Json;

namespace VirtoCommerce.Platform.Web.Json;

public class ConfigureOutputJsonSerializerSettings : IConfigureOptions<OutputJsonSerializerSettings>
{
    private readonly IOptions<MvcNewtonsoftJsonOptions> _jsonOptions;
    private readonly Action<OutputJsonSerializerSettings, MvcNewtonsoftJsonOptions> _setupAction;

    public ConfigureOutputJsonSerializerSettings(
        IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
        Action<OutputJsonSerializerSettings, MvcNewtonsoftJsonOptions> setupAction)
    {
        _jsonOptions = jsonOptions;
        _setupAction = setupAction;
    }

    public void Configure(OutputJsonSerializerSettings options)
    {
        _setupAction(options, _jsonOptions.Value);
    }
}
