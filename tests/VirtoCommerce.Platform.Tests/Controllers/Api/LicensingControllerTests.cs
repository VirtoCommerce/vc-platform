using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Licensing;
using Xunit;
using static VirtoCommerce.Platform.Web.Controllers.Api.LicensingController;

namespace VirtoCommerce.Platform.Tests.Controllers.Api
{
    public class LicensingControllerTests
    {
        private readonly Mock<IOptions<PlatformOptions>> _options = new Mock<IOptions<PlatformOptions>>();
        private readonly Mock<ISettingsManager> _settingsManager = new Mock<ISettingsManager>();
        private readonly PlatformOptions platformOptions = new PlatformOptions();
        private readonly LicenseProvider _licenseProvider;

        private readonly LicensingController _controller;

        public LicensingControllerTests()
        {
            _options.SetupGet(x => x.Value).Returns(platformOptions);

            _licenseProvider = new LicenseProvider(_options.Object, null, null);

            _controller = new LicensingController(_options.Object, _settingsManager.Object, _licenseProvider);
        }

        [Theory]
        [InlineData(ExpirationTime.Unset, false)]
        [InlineData(ExpirationTime.Yesterday, false)]
        [InlineData(ExpirationTime.Tomorrow, false)]
        public async Task GetTrialExpirationDate(ExpirationTime expirationTime, bool expected)
        {
            // Arrange
            var expirationDate = GetExpirationDateByExpirationTime(expirationTime);
            _settingsManager
                .Setup(x => x.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry
                {
                    Value = expirationDate
                });

            // Act
            var result = await _controller.GetTrialExpirationDate();
            var actual = ((OkObjectResult)result.Result).Value as TrialState;

            // Assert
            actual.Should().NotBeNull();
            actual.ClientPassRegistration.Should().Be(expected);
            actual.ExpirationDate.Should().Be(expirationDate);
        }

        [Fact]
        public async Task GetTrialExpirationDate_Registered()
        {
            // Arrange
            _settingsManager
                .Setup(x => x.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry
                {
                    Value = GetExpirationDateByExpirationTime(ExpirationTime.AlreadyRegistered)
                });

            // Act
            var result = await _controller.GetTrialExpirationDate();
            var actual = ((OkObjectResult)result.Result).Value as TrialState;

            // Assert
            actual.Should().NotBeNull();
            actual.ClientPassRegistration.Should().Be(true);

            actual.ExpirationDate.Should().BeNull();
        }

        [Fact]
        public async Task ContinueTrial()
        {
            // Arrange
            var dateTime = new Fixture().Create<DateTime>();
            _settingsManager
                .Setup(x => x.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry());

            // Act
            await _controller.ContinueTrial(new TrialProlongation { NextTime = dateTime.ToString() });

            // Assert
            _settingsManager.Verify(x => x.GetObjectSettingAsync(It.Is<string>(x => x.Equals(PlatformConstants.Settings.Setup.TrialExpirationDate.Name)), null, null), Times.Once);
            _settingsManager.Verify(x => x.SaveObjectSettingsAsync(It.Is<IEnumerable<ObjectSettingEntry>>(x => x.First().Value.ToString().Equals(dateTime.ToString()))), Times.Once);
        }

        public enum ExpirationTime
        {
            Unset,
            Yesterday,
            Tomorrow,
            AlreadyRegistered
        }

        private DateTime? GetExpirationDateByExpirationTime(ExpirationTime expirationTime) => expirationTime switch
        {
            ExpirationTime.Unset => null,
            ExpirationTime.Yesterday => DateTime.UtcNow.AddDays(-1),
            ExpirationTime.Tomorrow => DateTime.UtcNow.AddDays(1),
            ExpirationTime.AlreadyRegistered => DateTime.MaxValue,
            _ => throw new ArgumentException($"{expirationTime} not allowed for this test")
        };
    }
}
