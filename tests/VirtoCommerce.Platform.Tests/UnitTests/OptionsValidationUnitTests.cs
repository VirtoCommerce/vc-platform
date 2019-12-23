using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Assets.AzureBlobStorage;
using VirtoCommerce.Platform.Assets.FileSystem;
using VirtoCommerce.Platform.Core;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class OptionsValidationUnitTests
    {
        [Fact]
        public void PlatformOptions_CanValidateDataAnnotations()
        {
            //Arrange
            var services = new ServiceCollection();
            services.AddOptions<PlatformOptions>()
                .Configure(o =>
                {
                    o.LocalUploadFolderPath = null;
                    o.LicenseActivationUrl = "wrong url";
                    o.SampleDataUrl = "wrong url";
                })
                .ValidateDataAnnotations();

            //Act
            var sp = services.BuildServiceProvider();

            //Assert
            var error = Assert.Throws<OptionsValidationException>(() => sp.GetRequiredService<IOptions<PlatformOptions>>().Value);
            ValidateFailure<PlatformOptions>(error, Options.DefaultName, 3,
                $"DataAnnotation validation failed for members: '{nameof(PlatformOptions.LocalUploadFolderPath)}' with the error: 'The {nameof(PlatformOptions.LocalUploadFolderPath)} field is required.'.",
                $"DataAnnotation validation failed for members: '{nameof(PlatformOptions.LicenseActivationUrl)}' with the error: 'The {nameof(PlatformOptions.LicenseActivationUrl)} field is not a valid fully-qualified http, https, or ftp URL.",
                $"DataAnnotation validation failed for members: '{nameof(PlatformOptions.SampleDataUrl)}' with the error: 'The {nameof(PlatformOptions.SampleDataUrl)} field is not a valid fully-qualified http, https, or ftp URL.");
        }

        [Fact]
        public void FileSystemBlobOptions_CanValidateDataAnnotations()
        {
            //Arrange
            var services = new ServiceCollection();
            services.AddOptions<FileSystemBlobOptions>()
                .Configure(o =>
                {
                    o.RootPath = null;
                    o.PublicUrl = "wrong url";
                })
                .ValidateDataAnnotations();

            //Act
            var sp = services.BuildServiceProvider();

            //Assert
            var error = Assert.Throws<OptionsValidationException>(() => sp.GetRequiredService<IOptions<FileSystemBlobOptions>>().Value);
            ValidateFailure<FileSystemBlobOptions>(error, Options.DefaultName, 2,
                $"DataAnnotation validation failed for members: '{nameof(FileSystemBlobOptions.RootPath)}' with the error: 'The {nameof(FileSystemBlobOptions.RootPath)} field is required.'.",
                $"DataAnnotation validation failed for members: '{nameof(FileSystemBlobOptions.PublicUrl)}' with the error: 'The {nameof(FileSystemBlobOptions.PublicUrl)} field is not a valid fully-qualified http, https, or ftp URL.");
        }

        [Fact]
        public void AzureBlobOptions_CanValidateDataAnnotations()
        {
            //Arrange
            var services = new ServiceCollection();
            services.AddOptions<AzureBlobOptions>()
                .Configure(o =>
                {
                    o.ConnectionString = null;
                })
                .ValidateDataAnnotations();

            //Act
            var sp = services.BuildServiceProvider();

            //Assert
            var error = Assert.Throws<OptionsValidationException>(() => sp.GetRequiredService<IOptions<AzureBlobOptions>>().Value);
            ValidateFailure<AzureBlobOptions>(error, Options.DefaultName, 1,
                $"DataAnnotation validation failed for members: '{nameof(AzureBlobOptions.ConnectionString)}' with the error: 'The {nameof(AzureBlobOptions.ConnectionString)} field is required.'.");
        }


        private void ValidateFailure<TOptions>(OptionsValidationException ex, string name = "", int count = 1, params string[] errorsToMatch)
        {
            Assert.Equal(typeof(TOptions), ex.OptionsType);
            Assert.Equal(name, ex.OptionsName);
            if (errorsToMatch.Length == 0)
            {
                errorsToMatch = new string[] { "A validation error has occured." };
            }
            Assert.Equal(count, ex.Failures.Count());
            // Check for the error in any of the failures
            foreach (var error in errorsToMatch)
            {
                Assert.True(ex.Failures.FirstOrDefault(f => f.Contains(error)) != null, "Did not find: " + error);
            }
        }
    }
}
