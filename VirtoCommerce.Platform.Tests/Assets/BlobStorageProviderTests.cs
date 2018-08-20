using System;
using System.IO;
using VirtoCommerce.Platform.Data.Assets;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Assets
{
    [Trait("Category", "Unit")]
    public class BlobStorageProviderTests
    {
        /// <summary>
        /// `OpenWrite` method should return write-only stream.
        /// </summary>
        [Fact]
        public void FileSystemBlobProviderStreamWritePermissionsTest()
        {
            var tempPath = Path.GetTempPath();
            var tempDirectory = Path.Combine(tempPath, "FileSystemBlobProviderTests");
            Directory.CreateDirectory(tempDirectory);
            try
            {
                var fsbProvider = new FileSystemBlobProvider(tempDirectory);

                using (var actualStream = fsbProvider.OpenWrite("file.tmp"))
                {
                    Assert.True(actualStream.CanWrite, "'OpenWrite' stream should be writable.");
                    Assert.False(actualStream.CanRead, "'OpenWrite' stream should be write-only.");
                }
                
            }
            catch (Exception e)
            {
                Assert.True(false, $"Test failed with unexpected exception '{e.Message}'");
            }
            finally
            {
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, recursive: true);
                }
            }
        }
    }
}
