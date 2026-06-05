using System;
using System.IO;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api
{
    [Trait("Category", "Unit")]
    public class GetSafeFullPathTests : PlatformWebMockHelper
    {
        private readonly PlatformBackupRestoreController _controller;

        public GetSafeFullPathTests()
        {
            var options = Options.Create(new PlatformOptions
            {
                DefaultExportFolder = Path.Combine(Path.GetTempPath(), "vc-test-exports"),
                LocalUploadFolderPath = Path.Combine(Path.GetTempPath(), "vc-test-uploads"),
            });

            _controller = new PlatformBackupRestoreController(
                Mock.Of<IPlatformExportImportManager>(),
                Mock.Of<IPushNotificationManager>(),
                Mock.Of<IUserNameResolver>(),
                options,
                Mock.Of<IDataProtectionProvider>(),
                Mock.Of<ILogger<PlatformBackupRestoreController>>());
        }

        [Theory]
        [InlineData("../../../etc/passwd")]
        [InlineData("..\\..\\windows\\system32\\cmd.exe")]
        [InlineData("/etc/passwd")]
        [InlineData("subdir/file.zip")]
        [InlineData("   ")]
        public void DownloadExportFile_MaliciousPath_ThrowsPlatformException(string maliciousFileName)
        {
            // Act
            var act = () => _controller.DownloadExportFile(maliciousFileName);

            // Assert
            act.Should().Throw<PlatformException>();
        }

        [Fact]
        public void DownloadExportFile_ValidFileName_PassesPathValidation()
        {
            // Act
            var act = () => _controller.DownloadExportFile("export.zip");

            // Assert — GetSafeFullPath passes; the subsequent File.Open fails because the file doesn't exist.
            act.Should().Throw<Exception>().Which.Should().NotBeOfType<PlatformException>();
        }
    }
}
