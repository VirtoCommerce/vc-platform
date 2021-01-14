using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Controllers.Api
{
    public class LicensingControllerTests
    {
        private readonly Mock<IOptions<PlatformOptions>> _options = new Mock<IOptions<PlatformOptions>>();
        private readonly Mock<ISettingsManager> _settingsManager = new Mock<ISettingsManager>();
        private readonly PlatformOptions platformOptions = new PlatformOptions();

        private readonly LicensingController _controller;

        public LicensingControllerTests()
        {
            _options.SetupGet(x => x.Value).Returns(platformOptions);

            _controller = new LicensingController(_options.Object, _settingsManager.Object);
        }

        [Theory]
        [InlineData("11:22:33", ExpirationTime.Unset, false)]
        [InlineData("00:00:00", ExpirationTime.Unset, true)]
        [InlineData("00:00:00", ExpirationTime.Yesterday, true)]
        [InlineData("00:00:00", ExpirationTime.Tomorrow, false)]
        public async Task CheckTrialExpiration(string delay, ExpirationTime expirationTime, bool expected)
        {
            // Arrange
            _settingsManager
                .Setup(x => x.GetObjectSettingAsync(PlatformConstants.Settings.Setup.TrialExpirationDate.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry
                {
                    Value = GetExpirationDateByExpirationTime(expirationTime)
                });

            _settingsManager
                .Setup(x => x.GetObjectSettingAsync(PlatformConstants.Settings.Setup.ClientPassRegistration.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry
                {
                    Value = false
                });

            platformOptions.RegistrationDelay = TimeSpan.Parse(delay);

            // Act
            var result = await _controller.CheckTrialExpiration();
            var actual = ((OkObjectResult)result.Result).Value;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task CheckTrialExpiration_ClientPassRegistration()
        {
            // Arrange
            _settingsManager
                .Setup(x => x.GetObjectSettingAsync(PlatformConstants.Settings.Setup.ClientPassRegistration.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry
                {
                    Value = true // Client pass registration
                });

            // Act
            var result = await _controller.CheckTrialExpiration();
            var actual = ((OkObjectResult)result.Result).Value;

            // Assert
            actual.Should().Be(false);
        }

        public enum ExpirationTime
        {
            Unset,
            Yesterday,
            Tomorrow
        }

        private object GetExpirationDateByExpirationTime(ExpirationTime expirationTime) => expirationTime switch
        {
            ExpirationTime.Unset => null,
            ExpirationTime.Yesterday => DateTime.UtcNow.AddDays(-1),
            ExpirationTime.Tomorrow => DateTime.UtcNow.AddDays(1),
            _ => throw new ArgumentException($"{expirationTime} not allowed for this test")
        };
    }
}
