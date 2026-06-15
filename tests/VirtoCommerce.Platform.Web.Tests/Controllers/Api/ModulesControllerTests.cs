using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Jobs;
using VirtoCommerce.Platform.Web.Modularity;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api
{
    public class ModulesControllerTests
    {
        private readonly Mock<IModuleService> _moduleService = new();
        private readonly Mock<IModuleManagementService> _moduleManagementService = new();
        private readonly Mock<IPushNotificationManager> _pushNotifier = new();
        private readonly Mock<IUserNameResolver> _userNameResolver = new();
        private readonly Mock<ISettingsManager> _settingsManager = new();
        private readonly Mock<IPlatformRestarter> _platformRestarter = new();
        private readonly Mock<IBackgroundJob> _backgroundJob = new();
        private readonly Mock<ILogger<ModulesController>> _logger = new();

        private ModulesController CreateController() => new(
            _moduleService.Object,
            _moduleManagementService.Object,
            _pushNotifier.Object,
            _userNameResolver.Object,
            _settingsManager.Object,
            Options.Create(new PlatformOptions()),
            Options.Create(new ExternalModuleCatalogOptions()),
            Options.Create(new LocalStorageModuleCatalogOptions()),
            _platformRestarter.Object,
            _backgroundJob.Object,
            Mock.Of<IBackgroundJobHandler<ModuleBackgroundJobPayload>>(),
            _logger.Object);

        private static ManifestModuleInfo CreateModule(string id, string iconUrl)
        {
            var module = new ManifestModuleInfo();
            module.LoadFromManifest(new ModuleManifest
            {
                Id = id,
                Version = "1.0.0",
                PlatformVersion = "3.0.0",
                IconUrl = iconUrl,
            });
            return module;
        }

        /// <summary>
        /// VCST-5212: A module whose management descriptor has a non-empty IconUrl (so it passes
        /// the GetModules() Where filter) but whose corresponding local module has a NULL IconUrl
        /// must NOT crash IconFileExists with a NullReferenceException. The endpoint should return
        /// 200 OK and the entry's IconUrl should be null (the caller's ": null" fallback).
        /// </summary>
        [Fact]
        public void GetModules_LocalModuleHasNullIconUrl_ReturnsOkWithNullIconUrl()
        {
            // Arrange: management descriptor has an icon URL (passes the Where guard),
            // local module for the same Id has a null IconUrl (triggers the NRE in IconFileExists).
            const string moduleId = "VirtoCommerce.TestModule";

            _moduleManagementService
                .Setup(x => x.GetModules())
                .Returns(new List<ManifestModuleInfo> { CreateModule(moduleId, "Content/logo.png") });

            _moduleService
                .Setup(x => x.GetModules())
                .Returns(new List<ManifestModuleInfo> { CreateModule(moduleId, null) });

            var controller = CreateController();

            // Act
            var result = controller.GetModules();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var modules = okResult.Value.Should().BeAssignableTo<IEnumerable<ModuleDescriptor>>().Subject;

            var descriptor = Assert.Single(modules);
            descriptor.Id.Should().Be(moduleId);
            descriptor.IconUrl.Should().BeNull();
        }
    }
}
