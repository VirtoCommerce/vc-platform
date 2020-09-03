using System.IO;
using VirtoCommerce.Platform.Data.TransactionFileManager;
using Xunit;

namespace VirtoCommerce.Platform.Tests.IntegrationTests
{
    [Trait("Category", "IntegrationTest")]
    public class FileManagerIntegrationTests
    {
        private readonly TransactionFileManager _fileManager;

        public FileManagerIntegrationTests()
        {
            _fileManager = new TransactionFileManager();
        }

        [Fact]
        public void CreateDirectory_CreateTestDirectory()
        {
            //Arrange
            var path = Path.GetFullPath("Test");

            //Act
            _fileManager.CreateDirectory(path);

            //Assert
            Assert.True(Directory.Exists(path));
        }

        [Fact]
        public void Delete_DeleteTestDirectory()
        {
            //Arrange
            var path = Path.GetFullPath("Test");

            //Act
            _fileManager.Delete(path);

            //Assert
            Assert.False(Directory.Exists(path));
        }

        [Fact]
        public void SafeDelete_CreateAndSafeDeleteDirectory()
        {
            //Arrange
            var testDir = Path.GetFullPath("TestWithSub");
            var subDir = Path.GetFullPath("TestWithSub\\Sub");

            //Act
            _fileManager.CreateDirectory(testDir);
            _fileManager.CreateDirectory(subDir);
            _fileManager.SafeDelete(testDir);

            //Assert
            Assert.False(Directory.Exists(subDir));
            Assert.False(Directory.Exists(testDir));
        }


    }
}
