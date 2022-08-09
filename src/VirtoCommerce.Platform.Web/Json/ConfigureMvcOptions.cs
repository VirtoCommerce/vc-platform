using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Json;

namespace VirtoCommerce.Platform.Web.Json;

public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
{
    private readonly IOptions<OutputJsonSerializerSettings> _outputSerializerSettings;
    private readonly IOptions<MvcNewtonsoftJsonOptions> _jsonOptions;
    private readonly ArrayPool<char> _charPool;

    public ConfigureMvcOptions(
        IOptions<OutputJsonSerializerSettings> outputSerializerSettings,
        IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
        ArrayPool<char> charPool)
    {
        _outputSerializerSettings = outputSerializerSettings;
        _jsonOptions = jsonOptions;
        _charPool = charPool;
    }

    public void Configure(MvcOptions options)
    {
        options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
        options.OutputFormatters.Add(new NewtonsoftJsonOutputFormatter(_outputSerializerSettings.Value, _charPool, options, _jsonOptions.Value));
    }
}
