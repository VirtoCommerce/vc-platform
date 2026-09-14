using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// Appends "&lt;name&gt;:&lt;hook&gt;" to a shared log, so a test can assert what ran, in which order, and
/// how many times, from one sequence rather than from per-instance counters.
/// </summary>
public class RecordingPlatformStartup : IPlatformStartup
{
    private readonly string _name;
    private readonly IList<string> _log;

    public RecordingPlatformStartup(string name, IList<string> log)
    {
        _name = name;
        _log = log;
    }

    public void ConfigureAfterRouting(IApplicationBuilder app, IConfiguration config)
    {
        _log.Add($"{_name}:routing");
    }

    public void ConfigureAfterAuthentication(IApplicationBuilder app, IConfiguration config)
    {
        _log.Add($"{_name}:authentication");
    }
}
