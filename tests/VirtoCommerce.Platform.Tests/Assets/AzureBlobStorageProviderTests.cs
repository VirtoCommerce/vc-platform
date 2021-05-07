using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Assets.AzureBlobStorage;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Assets
{
    //[Trait("Category", "Unit")]
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
        [Fact]
        public void StreamWritePermissionsTest()
        {
            // Arrange
            var provider = new AzureBlobProvider(_options);
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
