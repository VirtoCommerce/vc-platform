using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage.Tests
{
    public class AzureBlobStorageProviderTests
    {
        private readonly IOptions<AzureBlobOptions> _options;

        public AzureBlobStorageProviderTests()
        {
            _options = new OptionsWrapper<AzureBlobOptions>(new AppConfiguration().GetApplicationConfiguration<AzureBlobOptions>());
        }

        /// <summary>
        /// `OpenWrite` method should return write-only stream.
        /// </summary>
        /// <remarks>
        /// Broken -> https://github.com/VirtoCommerce/vc-platform/pull/2254/checks?check_run_id=2551785684
        /// </remarks>
        [Fact(Skip = "Test is broken on CI")]
        public void StreamWritePermissionsTest()
        {
            // Arrange
            var provider = new AzureBlobProvider(_options, new OptionsWrapper<PlatformOptions>(new PlatformOptions()), null);
            var fileName = "file-write.tmp";
            var fileUrl = $"tmpfolder/{fileName}";

            // Act
            using var actualStream = provider.OpenWrite(fileUrl);

            // Assert
            Assert.True(actualStream.CanWrite, "'OpenWrite' stream should be writable.");
            Assert.False(actualStream.CanRead, "'OpenWrite' stream should be write-only.");
            Assert.Equal(0, actualStream.Position);
        }
    }
}
