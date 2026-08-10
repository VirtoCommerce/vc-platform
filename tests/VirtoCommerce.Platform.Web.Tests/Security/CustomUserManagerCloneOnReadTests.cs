using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;
using Xunit;
using static VirtoCommerce.Platform.Web.Tests.Security.PlatformWebMockHelper;
using CachePlatformWebMockHelper = VirtoCommerce.Platform.Web.Tests.Cache.PlatformWebMockHelper;

namespace VirtoCommerce.Platform.Web.Tests.Security
{
    /// <summary>
    /// The cached Find* methods must return detached copies of the cached user. The store returns the SAME
    /// instance for repeated reads of one user (with the EF store, the entity stays tracked by the context
    /// after the cache-miss load), so handing that instance out lets callers corrupt the cache, and passing
    /// it back into UpdateAsync makes the role diff run against the caller's own mutated instance — silently
    /// dropping role changes (found in the field: switching a Sales Rep's role was lost on a cold cache).
    /// </summary>
    public class CustomUserManagerCloneOnReadTests
    {
        private const string UserId = "id1";

        [Fact]
        public async Task FindByIdAsync_ReturnsDetachedCopy_AndStillCachesTheLoad()
        {
            //Arrange
            var storedUser = new ApplicationUser { Id = UserId, UserName = "TestUser" };
            var storeMock = CreateStoreMock(storedUser);
            var userManager = CreateUserManager(storeMock);

            //Act
            var first = await userManager.FindByIdAsync(UserId);
            var second = await userManager.FindByIdAsync(UserId);

            //Assert
            first.Should().NotBeSameAs(storedUser, "the store's (tracked) instance must never leave the manager");
            second.Should().NotBeSameAs(first, "every caller must get its own copy");
            first.Id.Should().Be(UserId);
            second.UserName.Should().Be("TestUser");

            // Still one store read: the copy is taken per call, the load stays cached.
            storeMock.Verify(x => x.FindByIdAsync(UserId, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_MutatingReturnedUser_DoesNotCorruptCache()
        {
            //Arrange
            var storedUser = new ApplicationUser { Id = UserId, UserName = "TestUser" };
            var userManager = CreateUserManager(CreateStoreMock(storedUser));

            //Act
            var first = await userManager.FindByIdAsync(UserId);
            first.UserName = "Mutated";
            first.Roles = new List<Role> { new Role { Id = "injected-role", Name = "Injected role" } };

            var second = await userManager.FindByIdAsync(UserId);

            //Assert
            second.UserName.Should().Be("TestUser");
            second.Roles.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_RoleAssignedOnUserFromColdCache_IsApplied()
        {
            //Arrange
            var storedUser = new ApplicationUser { Id = UserId, UserName = "TestUser" };
            var storeMock = CreateStoreMock(storedUser);
            var roleStoreMock = storeMock.As<IUserRoleStore<ApplicationUser>>();
            roleStoreMock
                .Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(false);
            roleStoreMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var userManager = CreateUserManager(storeMock);

            // A cold read: with the EF store this loads the user into the store's context, so a second
            // resolve of the same id inside UpdateAsync yields the same instance (identity resolution).
            var user = await userManager.FindByIdAsync(UserId);
            user.Roles = new List<Role> { new Role { Id = "new-role", Name = "New role" } };

            //Act
            var result = await userManager.UpdateAsync(user);

            //Assert
            result.Succeeded.Should().BeTrue();

            // Before clone-on-read, UpdateAsync reloaded the details of this very instance and the role diff
            // compared the database with itself — the assignment below never happened.
            roleStoreMock.Verify(
                x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesTheStoreOwnInstance_NotTheDetachedCopy()
        {
            //Arrange
            var storedUser = new ApplicationUser { Id = UserId, UserName = "TestUser" };
            var storeMock = CreateStoreMock(storedUser);
            storeMock
                .Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(IdentityResult.Success);

            var userManager = CreateUserManager(storeMock);
            var detachedCopy = await userManager.FindByIdAsync(UserId);

            //Act
            var result = await userManager.DeleteAsync(detachedCopy);

            //Assert
            result.Succeeded.Should().BeTrue();

            // The delete must target the instance the store itself resolves (with the EF store the tracked
            // one), otherwise EF rejects the detached copy: "another instance with the same key value ... is
            // already being tracked".
            storeMock.Verify(x => x.DeleteAsync(storedUser, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_MissingUser_ReturnsNull()
        {
            //Arrange
            var userManager = CreateUserManager(new Mock<IUserStore<ApplicationUser>>());

            //Act
            var result = await userManager.FindByIdAsync("missing");

            //Assert
            result.Should().BeNull();
        }

        private static Mock<IUserStore<ApplicationUser>> CreateStoreMock(ApplicationUser storedUser)
        {
            var storeMock = new Mock<IUserStore<ApplicationUser>>();

            // Always the same instance, like the EF store returns for an entity tracked by its context.
            storeMock
                .Setup(x => x.FindByIdAsync(storedUser.Id, CancellationToken.None))
                .ReturnsAsync(storedUser);

            return storeMock;
        }

        private static CustomUserManager CreateUserManager(Mock<IUserStore<ApplicationUser>> storeMock)
        {
            return SecurityMockHelper.TestCustomUserManager(
                storeMock,
                platformMemoryCache: CachePlatformWebMockHelper.MemoryCacheMockHelper.GetPlatformMemoryCache(),
                eventPublisher: new EventPublisherStub());
        }
    }
}
