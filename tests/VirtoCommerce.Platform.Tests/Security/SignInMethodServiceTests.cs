using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInMethods;
using VirtoCommerce.Platform.Security.SignInMethods;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class SignInMethodServiceTests
{
    private readonly ApplicationUser _user = new() { Id = "user-id" };

    [Fact]
    public async Task GetSignInMethodsAsync_Should_OrderByPriorityThenDisplayName()
    {
        // Arrange
        var provider = CreateProvider(
            CreateMethod("Google", priority: 1),
            CreateMethod("Password", priority: 10),
            CreateMethod("Facebook", priority: 1));

        var service = new SignInMethodService([provider.Object]);

        // Act
        var result = await service.GetSignInMethodsAsync(_user);

        // Assert
        result.Select(x => x.Name).Should().Equal("Password", "Facebook", "Google");
    }

    [Fact]
    public async Task DeleteKeyAsync_Should_Fail_When_TheKeyIsTheLastOne()
    {
        // Arrange
        var provider = CreateProvider(CreateMethod("Password", keyIds: ["password"]), CreateMethod("Google"));
        var service = new SignInMethodService([provider.Object]);

        // Act
        var result = await service.DeleteKeyAsync(_user, "Password", "password");

        // Assert
        result.Succeeded.Should().BeFalse();
        provider.Verify(x => x.DeleteKeyAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteKeyAsync_Should_CountTheKeysOfAllProviders()
    {
        // Arrange
        var passwordProvider = CreateProvider(CreateMethod("Password", keyIds: ["password"]));
        var passkeyProvider = CreateProvider(CreateMethod("Passkey", keyIds: ["passkey-1"]));
        passwordProvider.Setup(x => x.DeleteKeyAsync(_user, "Password", "password")).ReturnsAsync(IdentityResult.Success);

        var service = new SignInMethodService([passwordProvider.Object, passkeyProvider.Object]);

        // Act
        var result = await service.DeleteKeyAsync(_user, "Password", "password");

        // Assert
        result.Succeeded.Should().BeTrue();
        passwordProvider.Verify(x => x.DeleteKeyAsync(_user, "Password", "password"), Times.Once);
        passkeyProvider.Verify(x => x.DeleteKeyAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task DeleteKeyAsync_Should_PassTheRegisteredMethodName_When_TheRequestedNameDiffersInCase()
    {
        // Arrange
        var provider = CreateProvider(CreateMethod("Password", keyIds: ["password"]), CreateMethod("Google", keyIds: ["google-key"]));
        provider.Setup(x => x.DeleteKeyAsync(_user, "Google", "google-key")).ReturnsAsync(IdentityResult.Success);

        var service = new SignInMethodService([provider.Object]);

        // Act
        var result = await service.DeleteKeyAsync(_user, "google", "google-key");

        // Assert
        result.Succeeded.Should().BeTrue();
        provider.Verify(x => x.DeleteKeyAsync(_user, "Google", "google-key"), Times.Once);
    }

    [Fact]
    public async Task DeleteKeyAsync_Should_Fail_When_TheKeyBelongsToAnotherMethod()
    {
        // Arrange
        var provider = CreateProvider(CreateMethod("Password", keyIds: ["password"]), CreateMethod("Google", keyIds: ["google-key"]));
        var service = new SignInMethodService([provider.Object]);

        // Act
        var result = await service.DeleteKeyAsync(_user, "Password", "google-key");

        // Assert
        result.Succeeded.Should().BeFalse();
        provider.Verify(x => x.DeleteKeyAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RenameKeyAsync_Should_CallTheProviderThatOwnsTheKey()
    {
        // Arrange
        var passwordProvider = CreateProvider(CreateMethod("Password", keyIds: ["password"]));
        var passkeyProvider = CreateProvider(CreateMethod("Passkey", keyIds: ["passkey-1"]));
        passkeyProvider.Setup(x => x.RenameKeyAsync(_user, "Passkey", "passkey-1", "Laptop")).ReturnsAsync(IdentityResult.Success);

        var service = new SignInMethodService([passwordProvider.Object, passkeyProvider.Object]);

        // Act
        var result = await service.RenameKeyAsync(_user, "passkey", "passkey-1", "Laptop");

        // Assert
        result.Succeeded.Should().BeTrue();
        passkeyProvider.Verify(x => x.RenameKeyAsync(_user, "Passkey", "passkey-1", "Laptop"), Times.Once);
        passwordProvider.Verify(x => x.RenameKeyAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    private static Mock<ISignInMethodProvider> CreateProvider(params SignInMethod[] signInMethods)
    {
        var provider = new Mock<ISignInMethodProvider>();
        provider.Setup(x => x.GetSignInMethodsAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(signInMethods);

        return provider;
    }

    private static SignInMethod CreateMethod(string name, int priority = 0, string[] keyIds = null)
    {
        return new SignInMethod
        {
            Name = name,
            DisplayName = name,
            Priority = priority,
            Keys = (keyIds ?? []).Select(id => new SignInKey { Id = id }).ToList(),
        };
    }
}
