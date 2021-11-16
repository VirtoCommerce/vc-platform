using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Assets.Abstractions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Web.Licensing;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Licenseing
{
    public class LicenseProviderTests
    {
        private readonly Mock<IOptions<PlatformOptions>> _platformOptionsMock = new Mock<IOptions<PlatformOptions>>();
        private readonly Mock<ICommonBlobProvider> _blobProvider = new Mock<ICommonBlobProvider>();
        private readonly PlatformOptions platformOptions = new PlatformOptions();
        private readonly LicenseProvider _licenseProvider;

        public LicenseProviderTests()
        {
            _platformOptionsMock.SetupGet(x => x.Value).Returns(platformOptions);

            _licenseProvider = new LicenseProvider(_platformOptionsMock.Object, _blobProvider.Object);
        }

        [Fact]
        public async Task GetLicenseAsync_LicenceFound_ReadMethodCalled()
        {
            // Arrange
            _blobProvider.Setup(x => x.GetAbsoluteUrl(It.Is<string>(s => s.EqualsInvariant(platformOptions.LicenseBlobPath)))).Returns("url");
            _blobProvider.Setup(x => x.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _blobProvider.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MockStream());

            // Act
            _ = await _licenseProvider.GetLicenseAsync();

            // Assert
            _blobProvider.Verify(x => x.GetAbsoluteUrl(It.Is<string>(s => s.EqualsInvariant(platformOptions.LicenseBlobPath))), Times.Once);
            _blobProvider.Verify(x => x.OpenRead(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetLicenseAsync_LicenceNotFound_ReadMethodNotCalled()
        {
            // Arrange
            _blobProvider.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(new MockStream());

            // Act
            _ = await _licenseProvider.GetLicenseAsync();

            // Assert
            _blobProvider.Verify(x => x.GetAbsoluteUrl(It.Is<string>(s => s.EqualsInvariant(platformOptions.LicenseBlobPath))), Times.Once);
            _blobProvider.Verify(x => x.OpenRead(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SaveLicense_WriteMethodCalled()
        {
            // Arrange
            var license = new License();

            var mockSteam = new Mock<MockStream>();
            mockSteam.SetupGet(x => x.CanWrite).Returns(true);
            _blobProvider.Setup(x => x.OpenWriteAsync(It.IsAny<string>())).ReturnsAsync(mockSteam.Object);
            _blobProvider.Setup(x => x.GetAbsoluteUrl(It.Is<string>(s => s.EqualsInvariant(platformOptions.LicenseBlobPath)))).Returns("url");

            // Act
            await _licenseProvider.SaveLicenseAsync(license);

            // Assert
            _blobProvider.Verify(x => x.GetAbsoluteUrl(It.Is<string>(s => s.EqualsInvariant(platformOptions.LicenseBlobPath))), Times.Once);
            _blobProvider.Verify(x => x.OpenWriteAsync(It.IsAny<string>()), Times.Once);
            mockSteam.Verify(x => x.Close(), Times.Once);
        }

        public class MockStream : Stream
        {
            public override bool CanRead => true;
            public override bool CanSeek => true;
            public override bool CanWrite => true;
            public override long Length { get; }
            public override long Position { get; set; }
            
            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return 0;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return 0;
            }

            public override void SetLength(long value)
            {
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
            }

            public override void Close()
            {
            }
        }
    }
}
