using System;
using System.IO;
using VirtoCommerce.Platform.Data.Assets;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Assets
{
    [Trait("Category", "Unit")]
    public class BlobStorageProviderTests : IDisposable
    {
        private readonly string _tempDirectory;

        public BlobStorageProviderTests()
        {
            var tempPath = Path.GetTempPath();
            _tempDirectory = Path.Combine(tempPath, "FileSystemBlobProviderTests");
            Directory.CreateDirectory(_tempDirectory);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }

        /// <summary>
        /// `OpenWrite` method should return write-only stream.
        /// </summary>
        [Fact]
        public void FileSystemBlobProviderStreamWritePermissionsTest()
        {
            var fsbProvider = new FileSystemBlobProvider(_tempDirectory);

            using (var actualStream = fsbProvider.OpenWrite("file-write.tmp"))
            {
                Assert.True(actualStream.CanWrite, "'OpenWrite' stream should be writable.");
                Assert.False(actualStream.CanRead, "'OpenWrite' stream should be write-only.");
            }
        }

        /// <summary>
        /// `OpenRead` method should return read-only stream.
        /// </summary>
        [Fact]
        public void FileSystemBlobProviderStreamReadPermissionsTest()
        {
            var fsbProvider = new FileSystemBlobProvider(_tempDirectory);
            const string fileForRead = "file-read.tmp";

            // Creating empty file.
            File.WriteAllText(Path.Combine(_tempDirectory, fileForRead), string.Empty);
            
            using (var actualStream = fsbProvider.OpenRead(fileForRead))
            {
                Assert.True(actualStream.CanRead, "'OpenRead' stream should be readable.");
                Assert.False(actualStream.CanWrite, "'OpenRead' stream should be read-only.");
            }
        }
    }
}
