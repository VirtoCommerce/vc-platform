using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Extensions;
using VirtoCommerce.Platform.Core.Security;
using Xunit;
using static VirtoCommerce.Platform.Web.Tests.Security.PlatformWebMockHelper;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class UserManagerExtensionsTests
{
    [Theory]
    [InlineData("6e3ccdc3-48f0-4e53-8675-a9419fcfa1db", "6e3ccdc3-48f0-4e53-8675-a9419fcfa1db")]
    [InlineData("test-user+2@domain.com", "test-user+2@domain.com")]
    [InlineData("bad\nguy<script>alert(1);</script>@domain.com", "bad?guy?script?alert?1????script?@domain.com")]
    public void SanitizeUserName_KeepOnlyAllowedCharacters(string idOrName, string expectedSanitizedUserName)
    {
        // Arrange
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        var options = new IdentityOptions();
        var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, identityOptions: options);

        // Act
        var actualSanitizedUserName = userManager.SanitizeUserName(idOrName);

        // Assert
        Assert.Equal(expectedSanitizedUserName, actualSanitizedUserName);
    }
}
