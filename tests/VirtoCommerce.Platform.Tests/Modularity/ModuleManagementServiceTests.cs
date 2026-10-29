using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

[Collection(ModularityCollection.Name)]
public sealed class ModuleManagementServiceTests : IDisposable
{
    private const string _externalManifestJson = """
        [
          { "Id": "A", "Versions": [ { "Version": "3.2.0", "PlatformVersion": "3.0.0", "Dependencies": [ { "Id": "B", "Version": "3.2.0" } ] } ] },
          { "Id": "B", "Versions": [ { "Version": "3.2.0", "PlatformVersion": "3.0.0" } ] }
        ]
        """;

    private readonly ModuleBootstrapper _originalBootstrapper = ModuleBootstrapper.Instance;
    private readonly SemanticVersion _originalPlatformVersion = PlatformVersion.CurrentVersion;

    [Theory]
    [InlineData("A:3.2.0", new[] { "A:3.1.0" }, new[] { "A:3.2.0", "B:3.2.0" })]
    [InlineData("A:3.2.0", new[] { "A:3.1.0", "B:3.1.0" }, new[] { "A:3.2.0", "B:3.2.0" })]
    [InlineData("A:3.2.0", new[] { "A:3.1.0", "B:3.2.0" }, new[] { "A:3.2.0" })]
    public void GetDependencies_NewerVersionRequested_ReturnsModulesToInstall(string requestedModule, string[] installedModules, string[] expectedModulesToInstall)
    {
        // Arrange
        var request = CreateRequest(requestedModule);
        var service = CreateService(installedModules);

        // Act
        var result = service.GetDependencies([request]);

        // Assert
        result
            .Where(x => !x.IsInstalled)
            .Select(x => x.ToString())
            .Should()
            .BeEquivalentTo(expectedModulesToInstall);
    }

    private static ModuleInstallRequest CreateRequest(string module)
    {
        var parts = module.Split(':');

        return new ModuleInstallRequest(parts[0], parts.ElementAtOrDefault(1));
    }

    private static ModuleManagementService CreateService(string[] installedModules)
    {
        PlatformVersion.CurrentVersion = SemanticVersion.Parse("3.0.0");

        ModuleBootstrapper.Instance = new ModuleBootstrapper(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());
        ModuleBootstrapper.Instance.RegisterModulesInternal(installedModules.Select(CreateInstalledModule).ToList());

        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .Protected()
            .Setup<HttpResponseMessage>("Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns(() => new HttpResponseMessage { Content = new StringContent(_externalManifestJson) });

        var httpClientFactory = new Mock<IHttpClientFactory>();

        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient(httpMessageHandler.Object));

        var externalOptions = new ExternalModuleCatalogOptions { ModulesManifestUrl = new Uri("https://localhost/modules.json") };

        return new ModuleManagementService(
            httpClientFactory.Object,
            Options.Create(new LocalStorageModuleCatalogOptions()),
            Options.Create(externalOptions),
            NullLogger<ModuleManagementService>.Instance);
    }

    private static ManifestModuleInfo CreateInstalledModule(string module)
    {
        var parts = module.Split(':');

        var installedModule = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        installedModule.LoadFromManifest(new ModuleManifest { Id = parts[0], Version = parts[1], PlatformVersion = "3.0.0" });
        installedModule.IsInstalled = true;

        return installedModule;
    }

    public void Dispose()
    {
        ModuleBootstrapper.Instance = _originalBootstrapper;
        PlatformVersion.CurrentVersion = _originalPlatformVersion;
    }
}
