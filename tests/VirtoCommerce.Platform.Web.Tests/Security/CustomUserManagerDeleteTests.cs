using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using Xunit;
using static VirtoCommerce.Platform.Web.Tests.Security.PlatformWebMockHelper;

namespace VirtoCommerce.Platform.Web.Tests.Security
{
    public class CustomUserManagerDeleteTests
    {
        private const string UserName = "somebody@example.com";

        [Fact]
        public async Task Delete_UserIdNotFound_DoesNotResolveByUserName()
        {
            //Arrange
            //The id of an entity that was never stored still looks valid, because IdentityUser assigns one
            //in its constructor. Resolving such a delete by user name takes down the account owning the name.
            var unsaved = new ApplicationUser { UserName = UserName };

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(unsaved.Id, CancellationToken.None))
                .ReturnsAsync((ApplicationUser)null);
            userStoreMock.Setup(x => x.FindByNameAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new ApplicationUser { Id = "owner-id", UserName = UserName });
            userStoreMock.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(IdentityResult.Success);

            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, new EventPublisherStub());

            //Act
            await userManager.DeleteAsync(unsaved);

            //Assert
            userStoreMock.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task Delete_UserIdNotFound_FailsWithoutPublishingEvents()
        {
            //Arrange
            var unsaved = new ApplicationUser { UserName = UserName };

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(unsaved.Id, CancellationToken.None))
                .ReturnsAsync((ApplicationUser)null);
            userStoreMock.Setup(x => x.FindByNameAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new ApplicationUser { Id = "owner-id", UserName = UserName });
            userStoreMock.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(IdentityResult.Success);

            var eventPublisher = new EventPublisherStub();
            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, eventPublisher);

            //Act
            var result = await userManager.DeleteAsync(unsaved);

            //Assert
            //A refused delete must not announce itself: UserChangingEvent is a veto point, and a handler
            //that saw it without a matching UserChangedEvent would record a deletion that never happened.
            result.Succeeded.Should().BeFalse();
            eventPublisher.Events.Should().BeEmpty();
        }

        [Fact]
        public async Task Update_UserIdNotFound_ResolvesByUserName()
        {
            //Arrange
            //The name fallback in LoadExistingUser was added for this path, and stays here: only the delete
            //path stopped using it.
            var incoming = new ApplicationUser { UserName = UserName };
            var stored = new ApplicationUser { Id = "stored-id", UserName = UserName };

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(incoming.Id, CancellationToken.None))
                .ReturnsAsync((ApplicationUser)null);
            userStoreMock.Setup(x => x.FindByNameAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(stored);

            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, new EventPublisherStub());

            //Act
            var result = await userManager.UpdateAsync(incoming);

            //Assert
            result.Succeeded.Should().BeTrue();
            userStoreMock.Verify(x => x.UpdateAsync(stored, CancellationToken.None), Times.Once);
        }
    }
}
