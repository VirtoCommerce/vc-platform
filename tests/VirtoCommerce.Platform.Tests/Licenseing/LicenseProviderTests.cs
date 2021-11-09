using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Web.Licensing;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Licenseing
{
    public class LicenseProviderTests
    {
        private readonly Mock<IOptions<PlatformOptions>> _platformOptionsMock = new Mock<IOptions<PlatformOptions>>();
        private readonly Mock<IOptions<LicenceProviderBlobOptions>> _blobOptionsMock = new Mock<IOptions<LicenceProviderBlobOptions>>();
        private readonly PlatformOptions platformOptions = new PlatformOptions();
        private readonly LicenceProviderBlobOptions blobOptions = new LicenceProviderBlobOptions();
        private readonly LicenseProvider _licenseProvider;

        public LicenseProviderTests()
        {
            _platformOptionsMock.SetupGet(x => x.Value).Returns(platformOptions);
            _blobOptionsMock.SetupGet(x => x.Value).Returns(blobOptions);

            _licenseProvider = new LicenseProvider(_platformOptionsMock.Object, _blobOptionsMock.Object);
        }

        [Fact]
        public async Task GetLicenseAsync_LicenceNotFound_ReadMethodNotCalled()
        {
            // Act
            var licence = await _licenseProvider.GetLicenseAsync();

            // Assert
            licence.Should().BeNull();
        }

        [Fact]
        public async Task SaveLicense_WriteMethodNotCalled()
        {
            // Arrange
            var license = new License();

            // Act
            Func<Task> act = () => _licenseProvider.SaveLicenseAsync(license);
            await act
                .Should()
                .ThrowAsync<PlatformException>()
                .WithMessage("File system not supported for licence. Use Azure Blob Storage.");
        }
    }
}
