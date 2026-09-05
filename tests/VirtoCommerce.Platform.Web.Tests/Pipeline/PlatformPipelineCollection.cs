using System;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// Sets the variable AddForwardedHeaders reads at ConfigureServices time, for every class in the
/// collection, and puts the previous value back when the collection finishes.
/// </summary>
public sealed class ForwardedHeadersEnvironmentFixture : IDisposable
{
    public const string VariableName = "ASPNETCORE_FORWARDEDHEADERS_ENABLED";

    private readonly string _previousValue;

    public ForwardedHeadersEnvironmentFixture()
    {
        _previousValue = Environment.GetEnvironmentVariable(VariableName);
        Environment.SetEnvironmentVariable(VariableName, "true");
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(VariableName, _previousValue);
    }
}

// Every class that touches ASPNETCORE_FORWARDEDHEADERS_ENABLED or ModuleBootstrapper.Instance joins
// this collection. DisableParallelization is what makes that mean something: an ordinary collection
// serializes only its own members and still runs concurrently with every other collection in the
// assembly, so a process-global set here would be visible to whatever ran alongside it.
[CollectionDefinition(Name, DisableParallelization = true)]
public class PlatformPipelineCollection : ICollectionFixture<ForwardedHeadersEnvironmentFixture>
{
    public const string Name = "PlatformPipeline";
}
