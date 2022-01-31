using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
                })
                .ValidateDataAnnotations();

            //Act
            var sp = services.BuildServiceProvider();

            //Assert
            var error = Assert.Throws<OptionsValidationException>(() => sp.GetRequiredService<IOptions<PlatformOptions>>().Value);
            ValidateFailure<PlatformOptions>(error, Options.DefaultName, 2,
                $"DataAnnotation validation failed for 'PlatformOptions' members: '{nameof(PlatformOptions.LocalUploadFolderPath)}' with the error: 'The {nameof(PlatformOptions.LocalUploadFolderPath)} field is required.'.",
                $"DataAnnotation validation failed for 'PlatformOptions' members: '{nameof(PlatformOptions.LicenseActivationUrl)}' with the error: 'The {nameof(PlatformOptions.LicenseActivationUrl)} field is not a valid fully-qualified http, https, or ftp URL.");
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
