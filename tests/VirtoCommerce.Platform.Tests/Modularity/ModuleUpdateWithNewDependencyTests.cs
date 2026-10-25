using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.PushNotifications;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Jobs;
using VirtoCommerce.Platform.Web.Modularity;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// VCST-6516: updating an installed module to a version that declares a NEW required dependency
/// (not installed yet) silently did nothing: the admin UI got no dependencies to offer, the install
/// service skipped the update without reporting why, and the job ended with "Updating finished.".
/// <para>
/// Scenario: Test.ModuleA 1.0.0 is installed. The catalog offers Test.ModuleA 1.1.0, which depends on
/// Test.ModuleB 1.0.0. Test.ModuleB is available in the catalog but not installed.
/// </para>
/// </summary>
[Collection("Modularity")]
public class ModuleUpdateWithNewDependencyTests : IDisposable
{
    private const string ModuleA = "Test.ModuleA";
    private const string ModuleB = "Test.ModuleB";
    private const string InstalledVersion = "1.0.0";
    private const string UpdateVersion = "1.1.0";

    private const string ManifestUrl = "https://modules.example.test/modules_v3.json";

    private const string ExternalManifestJson = """
        [
          {
            "Id": "Test.ModuleA",
            "Title": "Module A",
            "Versions": [
              { "Version": "1.0.0", "PlatformVersion": "3.0.0", "PackageUrl": "https://modules.example.test/Test.ModuleA_1.0.0.zip", "Dependencies": [] },
              { "Version": "1.1.0", "PlatformVersion": "3.0.0", "PackageUrl": "https://modules.example.test/Test.ModuleA_1.1.0.zip",
                "Dependencies": [ { "Id": "Test.ModuleB", "Version": "1.0.0" } ] }
            ]
          },
          {
            "Id": "Test.ModuleB",
            "Title": "Module B",
            "Versions": [
              { "Version": "1.0.0", "PlatformVersion": "3.0.0", "PackageUrl": "https://modules.example.test/Test.ModuleB_1.0.0.zip", "Dependencies": [] }
            ]
          }
        ]
        """;

    private readonly ModuleBootstrapper _previousBootstrapper;
    private readonly SemanticVersion _previousPlatformVersion;
    private readonly string _discoveryPath;
    private readonly LocalStorageModuleCatalogOptions _localOptions;
    private readonly ModuleManagementService _moduleManagementService;

    public ModuleUpdateWithNewDependencyTests()
    {
        _previousPlatformVersion = PlatformVersion.CurrentVersion;
        PlatformVersion.CurrentVersion = SemanticVersion.Parse("3.999.0");

        _discoveryPath = Path.Combine(Path.GetTempPath(), "vc-module-update-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_discoveryPath);

        _localOptions = new LocalStorageModuleCatalogOptions
        {
            DiscoveryPath = _discoveryPath,
            RefreshProbingFolderOnStart = true,
        };

        // Installed modules come from ModuleBootstrapper.Instance (the local probing state)
        _previousBootstrapper = ModuleBootstrapper.Instance;
        var bootstrapper = new ModuleBootstrapper(NullLoggerFactory.Instance, _localOptions);
        bootstrapper.RegisterModulesInternal([CreateInstalledModule(ModuleA, InstalledVersion)]);
        ModuleBootstrapper.Instance = bootstrapper;

        // Available modules come from the external manifest
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient(new StubManifestHandler()));

        _moduleManagementService = new ModuleManagementService(
            httpClientFactory.Object,
            Options.Create(_localOptions),
            Options.Create(new ExternalModuleCatalogOptions { ModulesManifestUrl = new Uri(ManifestUrl) }),
            NullLogger<ModuleManagementService>.Instance);
    }

    public void Dispose()
    {
        ModuleBootstrapper.Instance = _previousBootstrapper;
        PlatformVersion.CurrentVersion = _previousPlatformVersion;

        try
        {
            Directory.Delete(_discoveryPath, recursive: true);
        }
        catch (IOException)
        {
            // Best-effort cleanup of the temp discovery folder
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Precondition_CatalogOffersUpdateWithNewDependency()
    {
        var modules = _moduleManagementService.GetModules();

        modules.Should().Contain(x => x.Id == ModuleA && x.Version.ToString() == InstalledVersion && x.IsInstalled);
        modules.Should().Contain(x => x.Id == ModuleA && x.Version.ToString() == UpdateVersion && !x.IsInstalled);
        modules.Should().Contain(x => x.Id == ModuleB && x.Version.ToString() == "1.0.0" && !x.IsInstalled);
    }

    /// <summary>
    /// Root cause: the admin UI asks this endpoint which modules must be installed together with the
    /// selected update. It must resolve dependencies of the requested (new) version, not the installed one.
    /// </summary>
    [Fact]
    public void GetMissingDependencies_UpdateToVersionWithNewDependency_ReturnsNewDependency()
    {
        // Arrange — the admin UI posts the NOT installed (target) version for an update
        var controller = CreateController();
        var request = new[] { new ModuleDescriptor { Id = ModuleA, Version = UpdateVersion } };

        // Act
        var actionResult = controller.GetMissingDependencies(request);

        // Assert
        var result = (actionResult.Result as OkObjectResult)?.Value as ModuleDescriptor[];
        result.Should().NotBeNull();
        result.Select(x => x.Id).Should().BeEquivalentTo([ModuleB]);
    }

    /// <summary>
    /// The install service must never skip a requested update silently: when it refuses to proceed
    /// because a dependency is not installed (and not requested), it must report which one.
    /// </summary>
    [Fact]
    public void InstallModules_UpdateRequiresNotInstalledDependency_ReportsMissingDependency()
    {
        // Arrange
        var progress = new SynchronousProgress();

        // Act
        _moduleManagementService.InstallModules([new ModuleInstallRequest(ModuleA, UpdateVersion)], progress);

        // Assert
        IsInstalled(ModuleA, UpdateVersion).Should().BeFalse("the update cannot proceed without its dependency");
        progress.Messages
            .Where(x => x.Level == ProgressMessageLevel.Error)
            .Should().Contain(x => x.Message.Contains(ModuleB),
                "the user must see which dependency blocks the update instead of a silent no-op");
    }

    /// <summary>
    /// The background job must not report "Updating finished." when the update was not applied
    /// (this is exactly what the admin UI showed: "Starting update..." then "Updating finished.").
    /// </summary>
    [Fact]
    public async Task ModuleBackgroundJob_UpdateNotApplied_DoesNotReportUpdatingFinished()
    {
        // Arrange
        var pushedDescriptions = new ConcurrentQueue<string>();
        var pushNotifier = new Mock<IPushNotificationManager>();
        pushNotifier
            .Setup(x => x.Send(It.IsAny<PushNotification>()))
            .Callback<PushNotification>(x => pushedDescriptions.Enqueue(x.Description));

        var handler = new ModuleBackgroundJobHandler(
            _moduleManagementService,
            pushNotifier.Object,
            CreateSettingsManager(),
            Options.Create(_localOptions));

        var payload = new ModuleBackgroundJobPayload
        {
            Action = ModuleAction.Update,
            Modules = [new ModuleInstallRequest(ModuleA, UpdateVersion)],
            NotificationId = Guid.NewGuid().ToString(),
            Creator = "admin",
            Title = "Update Module",
            TotalCount = 1,
        };

        // Act
        await handler.Execute(payload, Mock.Of<IJobExecutionContext>(), CancellationToken.None);

        // Assert
        IsInstalled(ModuleA, UpdateVersion).Should().BeFalse("the update cannot proceed without its dependency");
        pushedDescriptions.Should().NotContain("Updating finished.",
            "a skipped update must not be reported to the user as successfully finished");
        pushedDescriptions.Last().Should().Be("Update failed.");
    }

    private bool IsInstalled(string id, string version)
    {
        return _moduleManagementService.GetModules()
            .Any(x => x.Id == id && x.Version.ToString() == version && x.IsInstalled);
    }

    private ModulesController CreateController() => new(
        Mock.Of<IModuleService>(),
        _moduleManagementService,
        Mock.Of<IPushNotificationManager>(),
        Mock.Of<IUserNameResolver>(),
        CreateSettingsManager(),
        Options.Create(new PlatformOptions()),
        Options.Create(new ExternalModuleCatalogOptions()),
        Options.Create(_localOptions),
        Mock.Of<IPlatformRestarter>(),
        Mock.Of<IBackgroundJobHandler<ModuleBackgroundJobPayload>>(),
        Mock.Of<ILogger<ModulesController>>(),
        Mock.Of<IBackgroundJob>());

    private static ISettingsManager CreateSettingsManager()
    {
        var settingsManager = new Mock<ISettingsManager>();
        settingsManager
            .Setup(x => x.GetObjectSettingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string name, string _, string _) => new ObjectSettingEntry { Name = name });
        settingsManager
            .Setup(x => x.SaveObjectSettingsAsync(It.IsAny<IEnumerable<ObjectSettingEntry>>()))
            .Returns(Task.CompletedTask);

        return settingsManager.Object;
    }

    private static ManifestModuleInfo CreateInstalledModule(string id, string version)
    {
        var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
        module.LoadFromManifest(new ModuleManifest
        {
            Id = id,
            Version = version,
            PlatformVersion = "3.0.0",
        });
        module.IsInstalled = true;

        return module;
    }

    /// <summary>
    /// Records progress synchronously (<see cref="Progress{T}"/> posts to the thread pool).
    /// </summary>
    private sealed class SynchronousProgress : IProgress<ProgressMessage>
    {
        public List<ProgressMessage> Messages { get; } = [];

        public void Report(ProgressMessage value)
        {
            Messages.Add(value);
        }
    }

    /// <summary>
    /// Serves the external modules manifest; any other URL (package downloads) is not found.
    /// </summary>
    private sealed class StubManifestHandler : HttpMessageHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri?.ToString() == ManifestUrl)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(ExternalManifestJson, Encoding.UTF8, "application/json"),
                };
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request, cancellationToken));
        }
    }
}
